using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyLoLServer.Dao.Model;
using MyLoLServer.Tool;
using NetFramework;
using NetFramework.auto;
using Protocol;
using Protocol.Dto;

namespace MyLoLServer.Logic.Select
{
    public class SelectRoom : AbsMulitHandler, HandlerInterface
    {
        public ConcurrentDictionary<int, SelectModel> teamRed = new ConcurrentDictionary<int, SelectModel>();
        public ConcurrentDictionary<int, SelectModel> teamBlue = new ConcurrentDictionary<int, SelectModel>();

        //今ルームにおる人数
        int EnterCount = 0;
        //今ミッションのID
        int missionId = -1;

        List<int> readyList = new List<int>();

        public void Init(List<int> teamRed, List<int> teamBlue)
        {
            //ルームの初期化
            this.teamRed.Clear();
            this.teamBlue.Clear();
            EnterCount = 0;
            //red team
            foreach (int uid in teamRed)
            {
                SelectModel select = new SelectModel();
                select.userId = uid;
                select.name = GetUser(uid).name;
                select.Champion = -1;
                select.Enter = false;
                select.Ready = false;
                this.teamRed.TryAdd(uid, select);
            }
            //blue team
            foreach (int uid in teamBlue)
            {
                SelectModel select = new SelectModel();
                select.userId = uid;
                select.name = GetUser(uid).name;
                select.Champion = -1;
                select.Enter = false;
                select.Ready = false;
                this.teamBlue.TryAdd(uid, select);
            }

            //初期化完了、アラーム設定20秒キャンピオン選択画面、今回チーム解散
            missionId = ScheduleUtil.Instance.Schedule(delegate
           {
                //20秒経って、全員入ってない場合、ルーム解散
                if (EnterCount < teamRed.Count + teamBlue.Count)
               {
                   Destory();
               }
               else
               {
                   //再びタイマー使う　20秒選択
                   missionId = ScheduleUtil.Instance.Schedule(delegate
                   {
                       //20秒
                       bool selectAll = true;
                       foreach (SelectModel item in this.teamRed.Values)
                       {
                           if (item.Champion == -1)
                           {
                               selectAll = false;
                               break;
                           }
                       }
                       if (selectAll)
                       {
                           foreach (SelectModel item in this.teamBlue.Values)
                           {
                               if (item.Champion == -1)
                               {
                                   selectAll = false;
                                   break;
                               }
                           }
                       }
                       if (selectAll)
                       {
                           //selectchampoin終了したが、ready botton押してない場合 戦闘開始
                           StartFight();
                       }
                       else
                       {
                           //ルーム解散
                           Destory();
                       }
                   }, 20 * 1000);
               }
           }, 20 * 1000);
        }

        /// <summary>
        /// ルーム解散
        /// </summary>
        private void Destory()
        {
            Brocast(SelectProtocol.DESTORY_BRO, null);
            //ルームのイベント、自分を潰す
            EventUtil.destorySelect(Area);
            if (missionId != -1)
            {
                ScheduleUtil.Instance.RemoveMission(missionId);
            }
        }

        /// <summary>
        /// クライアント閉じる
        /// </summary>
        /// <param name="token"></param>
        /// <param name="error"></param>
        public void ClientClose(UserToken token, string error)
        {
            //このクライアントをネット断絶される
            Leave(token);
            Brocast(SelectProtocol.DESTORY_BRO, null);
            EventUtil.destorySelect(Area);
        }


        public void MessageReceive(UserToken token, SokectModel message)
        {
            switch (message.command)
            {
                case SelectProtocol.ENTER_CREQ:
                    EnterRoom(token);
                    break;
                case SelectProtocol.SELECT_CREQ:
                    SelectChampion(token, message.GetMessage<int>());
                    break;
                case SelectProtocol.TALK_CREQ:
                    Talk(token, message.GetMessage<string>());
                    break;
                case SelectProtocol.READY_CREQ:
                    Ready(token);
                    break;
            }
        }

        /// <summary>
        /// Select画面に入るのを知らせ
        /// </summary>
        private void EnterRoom(UserToken token)
        {
            //カウンター属したルーム判断、そして、Enter状態を修正
            int userId = GetUserId(token);
            if (teamRed.ContainsKey(userId))
            {
                teamRed[userId].Enter = true;
            }
            else if (teamBlue.ContainsKey(userId))
            {
                teamBlue[userId].Enter = true;
            }
            else
            {
                return;
            }

            //カウンダもうルームに入るか？（重複入る検証）
            if (base.Enter(token))
            {
                //エンター人数＋１
                EnterCount++;
            }

            //ルームにEnterできた、ユーザーにルームの情報を送る、そして、ほかのユーザーに人入ったぜを知らせ
            SelectRoomDTO roomDto = new SelectRoomDTO();
            roomDto.teamRed = teamRed.Values.ToArray();
            roomDto.teamBlue = teamBlue.Values.ToArray();
            Write(token, SelectProtocol.ENTER_SRES, roomDto);
            Brocast(SelectProtocol.ENTER_EXBRO, userId, token);
        }
    
        /// <summary>
        /// チャンピオンを選択ロジック
        /// </summary>
        /// <param name="token"></param>
        /// <param name="value"></param>
        private void SelectChampion(UserToken token , int value)
        {
            //ユーザーがルームにおるか？？
            if (base.IsEntered(token)) return;
            UserModel user = GetUser(token);
            //ユーザーこのキャンピオンを持っているか
            if(!user.championList.Contains(value))
            {
                Write(token, SelectProtocol.SELECT_SRES, null);
                return;
            }
            SelectModel selectModel = null;
            if(teamRed.ContainsKey(user.id))
            {
                //normal matchの中に同じチームで同じチャンピオンがでない
                foreach (SelectModel i in teamRed.Values)
                {
                    if (i.Champion == value) return;
                }
                selectModel = teamRed[user.id];
            }
            else
            {
                foreach (SelectModel i in teamBlue.Values)
                {
                    if (i.Champion == value) return;
                }
                selectModel = teamBlue[user.id];
            }
            //チャンピオン選択出来た
            selectModel.Champion = value;
            Brocast(SelectProtocol.SELECT_BRO, selectModel);
        }

        /// <summary>
        /// Selectルームのチャット機能
        /// 将来【悪口、禁止用語】など＊＊＊になる処理をtodo
        /// </summary>
        /// <param name="token"></param>
        /// <param name="value"></param>
        private void Talk(UserToken token, string value)
        {
            //ユーザーがルームにおるか
            if (base.IsEntered(token)) return;
            UserModel user = GetUser(token);
            Brocast(SelectProtocol.TALK_BRO, user.name + ":" + value);
        }

        /// <summary>
        /// ユーサの準備動作
        /// </summary>
        /// <param name="token"></param>
        private void Ready(UserToken token)
        {
            //ユーザーがルームにおるか
            if (base.IsEntered(token)) return;
            int userId = GetUserId(token);
            if (readyList.Contains(userId)) return;
            SelectModel selectModel = null;
            if(teamRed.ContainsKey(userId))
            {
                selectModel = teamRed[userId];
            }
            else
            {
                selectModel = teamBlue[userId];
            }
            //チャンピオン選択しません
            if(selectModel.Champion == -1)
            {
                //ランダム選択
                //todo
            }
            else
            {
                selectModel.Ready = true;
                Brocast(SelectProtocol.READY_BRO, selectModel);
                readyList.Add(userId);
                if(readyList.Count >= teamRed.Count + teamBlue.Count)
                {
                    //全員準備できた、戦闘開始
                    StartFight();
                }
            }
        }

        /// <summary>
        /// 全員準備できた、戦闘開始
        /// </summary>
        private void StartFight()
        {
            if(missionId != -1)
            {
                ScheduleUtil.Instance.RemoveMission(missionId);
                missionId = -1;
            }
            //戦闘モジュールに戦闘シートクリエイトを知らせ
            //todo

            //選択シーンにこのルームを削除を知らせ
            EventUtil.destorySelect(Area);
        }

        public override byte Type
        {
            get
            {
                return GameProtocol.TYPE_SELECT;
            }
        }

    }
}
