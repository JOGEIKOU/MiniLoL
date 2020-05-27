using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public void Init(List<int> teamRed, List<int> teamBlue)
        {
            teamRed.Clear();
            teamBlue.Clear();

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
                   Brocast(SelectProtocol.DESTORY_BRO, null);
                   EventUtil.destorySelect(Area);
               }
               else
               {
                   missionId = ScheduleUtil.Instance.Schedule(delegate
                   {
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
                            //todo
                        }
                       else
                       {

                       }
                       missionId = -1;
                   }, 20 * 1000);
               }
           }, 20 * 1000);
        }

        private void Destory()
        {
            Brocast(SelectProtocol.DESTORY_BRO, null);
            EventUtil.destorySelect(Area);
            if (missionId != -1)
            {
                ScheduleUtil.Instance.RemoveMission(missionId);
            }
            EnterCount = 0;
        }

        public void ClientClose(UserToken token, string error)
        {
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
                    break;
                case SelectProtocol.TALK_CREQ:
                    break;
                case SelectProtocol.READY_CREQ:
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
    


        public override byte Type
        {
            get
            {
                return GameProtocol.TYPE_SELECT;
            }
        }

    }
}
