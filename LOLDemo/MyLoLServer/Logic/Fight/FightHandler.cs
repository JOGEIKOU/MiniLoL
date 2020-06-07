using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyLoLServer.Logic.Select;
using MyLoLServer.Tool;
using NetFramework;
using NetFramework.auto;
using Protocol.Dto;

namespace MyLoLServer.Logic.Fight
{
    public class FightHandler : AbsMulitHandler, HandlerInterface
    {
        //スレッド同一保証辞書型]
        //ユーザーとルームのマップ
        ConcurrentDictionary<int, int> userRoom = new ConcurrentDictionary<int, int>();
        //ルームidとルームモデルのマップ
        ConcurrentDictionary<int, FightRoom> roomMap = new ConcurrentDictionary<int, FightRoom>();
        //使い捨たルームを回収、GCストレスの解消
        ConcurrentStack<FightRoom> roomCache = new ConcurrentStack<FightRoom>();
        //ルームID＋＋
        ConcurrentInt index = new ConcurrentInt();

        public FightHandler()
        {
            EventUtil.createFight = Create;
            EventUtil.destoryFight = Destory;
        }


        public void Create(SelectModel[] teamRed,SelectModel[] teamBlue)
        {
            FightRoom room;
            if (!roomCache.TryPop(out room))
            {
                room = new FightRoom();
                //主キーIDをあげる
                room.SetArea(index.GetAndAdd());
            }
            //ルーム初期化
            room.Init(teamRed, teamBlue);
            foreach (SelectModel item in teamRed)
            {
                userRoom.TryAdd(item.userId, room.Area);
            }
            foreach (SelectModel item in teamBlue)
            {
                userRoom.TryAdd(item.userId, room.Area);
            }
            roomMap.TryAdd(room.Area, room);
        }

        public void Destory(int roomId)
        {
            FightRoom room;
            if (roomMap.TryRemove(roomId, out room))
            {
                //todoカウンダとルームのマップを削除
                int temp = 0;
                //foreach (int item in room.teamRed.Keys)
                //{
                //    userRoom.TryRemove(item, out temp);
                //}
                //foreach (int item in room.teamBlue.Keys)
                //{
                //    userRoom.TryRemove(item, out temp);
                //}
                //room.list.Clear();
                //room.teamRed.Clear();
                //room.teamBlue.Clear();
                //ルームをキャッシュリストに入れる
                roomCache.Push(room);
            }
        }

        public void ClientClose(UserToken token, string error)
        {
            //戦闘中判定
            if(userRoom.ContainsKey(GetUserId(token)))
            {
                roomMap[userRoom[GetUserId(token)]].ClientClose(token, error);
            }
        }

        public void MessageReceive(UserToken token, SokectModel message)
        {
            roomMap[userRoom[GetUserId(token)]].MessageReceive(token, message);
        }
    }
}
