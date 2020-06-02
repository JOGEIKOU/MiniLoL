using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyLoLServer.Logic.Match;
using MyLoLServer.Tool;
using NetFramework;
using NetFramework.auto;

namespace MyLoLServer.Logic.Select
{
    public class SelectChampionHandler : AbsOnceHandler, HandlerInterface
    {
        //スレッド同一保証辞書型]
        //ユーザーとルームのマップ
        ConcurrentDictionary<int, int> userRoom = new ConcurrentDictionary<int, int>();
        //ルームidとルームモデルのマップ
        ConcurrentDictionary<int, SelectRoom> roomMap = new ConcurrentDictionary<int, SelectRoom>();
        //使い捨たルームを回収、GCストレスの解消
        ConcurrentStack<SelectRoom> roomCache = new ConcurrentStack<SelectRoom>();
        //ルームID＋＋
        ConcurrentInt index = new ConcurrentInt();



        public SelectChampionHandler()
        {
            EventUtil.createSelect = Create;
            EventUtil.destorySelect = Destory;
        }

        public void Create(List<int> teamRed, List<int> teamBlue)
        {
            SelectRoom room;
            if(!roomCache.TryPop(out room))
            {
                room = new SelectRoom();
                //主キーIDをあげる
                room.SetArea(index.GetAndAdd());
            }
            //ルーム初期化
            room.Init(teamRed,teamBlue);
            foreach (int  item in teamRed)
            {
                userRoom.TryAdd(item, room.Area);
            }
            foreach (int item in teamBlue)
            {
                userRoom.TryAdd(item, room.Area);
            }
            roomMap.TryAdd(room.Area, room);
        }

        public void Destory(int roomId)
        {
            SelectRoom room;
            if(roomMap.TryRemove(roomId,out room))
            {
                //todoカウンダとルームのマップを削除
                int temp = 0;
                foreach (int item in room.teamRed.Keys)
                {
                    userRoom.TryRemove(item, out temp);
                }
                foreach (int item in room.teamBlue.Keys)
                {
                    userRoom.TryRemove(item, out temp);
                }
                room.list.Clear();
                room.teamRed.Clear();
                room.teamBlue.Clear();
                //ルームをキャッシュリストに入れる
                roomCache.Push(room);
            }
        }

        public void ClientClose(UserToken token, string error)
        {
            int userId = GetUserId(token);
            //当ユーザー自分属したルームあるか
            if(userRoom.ContainsKey(userId))
            {
                int roomId;
                userRoom.TryRemove(userId,out roomId);
                if(roomMap.ContainsKey(roomId))
                {
                    //ルームにクライアントクロスのメッセージを知らせ
                    roomMap[roomId].ClientClose(token, error);
                }
            }
        }

        public void MessageReceive(UserToken token, SokectModel message)
        {
            int userId = GetUserId(token);
            if (userRoom.ContainsKey(userId))
            {
                int roomId = userRoom[userId];
                if (roomMap.ContainsKey(roomId))
                {
                    roomMap[roomId].MessageReceive(token, message);
                }
            }
            //if (roomMap.ContainsKey(message.area))
            //{
            //    roomMap[message.area].MessageReceive(token, message);
            //}
        }

    }
}
