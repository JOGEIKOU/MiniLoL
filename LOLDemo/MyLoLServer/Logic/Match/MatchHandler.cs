using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyLoLServer.Dao.Model;
using NetFramework;
using NetFramework.auto;
using Protocol;
using System.Collections.Concurrent;
using MyLoLServer.Tool;

namespace MyLoLServer.Logic.Match
{
    /// <summary>
    /// ノーマルマッチング処理
    /// </summary>
    public class MatchHandler :  AbsOnceHandler,HandlerInterface
    {
        //スレッド同一保証辞書型]
        //ユーザーとルームのマップ
        ConcurrentDictionary<int, int> userRoom = new ConcurrentDictionary<int, int>();
        //ルームidとルームモデルのマップ
        ConcurrentDictionary<int, MatchRoom> roomMap = new ConcurrentDictionary<int, MatchRoom>();
        //使い捨たルームを回収、GCストレスの解消
        ConcurrentStack<MatchRoom> roomCache = new ConcurrentStack<MatchRoom>();
        //ルームID＋＋
        ConcurrentInt index = new ConcurrentInt();


        public void ClientClose(UserToken token, string error)
        {
            Leave(token);
        }

        public void MessageReceive(UserToken token, SokectModel message)
        {
            switch(message.command)
            {
                case MatchProtocol.ENTER_CREQ:
                    Enter(token);
                    break;
                case MatchProtocol.LEAVE_CREQ:
                    Leave(token);
                    break;
            }
        }

        /// <summary>
        /// ユーザーノーマルマッチング入る
        /// </summary>
        /// <param name="token"></param>
        private void Enter(UserToken token)
        {
            int userId = GetUserId(token);
            Console.WriteLine("ノーマルマッチング開始" + userId);
            //ユーザーがノーマルマッチング中？？
            if (!userRoom.ContainsKey(userId))
            {
                MatchRoom room = null;
                bool isenter = false;
                //今　待っているルームあるか
                if(roomMap.Count > 0)
                {
                    //全て待っている状況
                    foreach (MatchRoom iteroom in roomMap.Values)
                    {
                        //満員ではない場合
                        if(iteroom.teamMax*2>iteroom.teamRed.Count + iteroom.teamBule.Count)
                        {
                            room = iteroom;
                            //もしteamRed満員ではない、teamRedに入る or teamBuleに入る
                            if (room.teamRed.Count < room.teamMax)
                            {
                                room.teamRed.Add(userId);
                            }
                            else
                            {
                                room.teamBule.Add(userId);
                            }
                            //ユーザーとルームのマップ関係追加
                            isenter = true;
                            userRoom.TryAdd(userId, room.id);
                            break;
                        }
                    }

                    if(!isenter)
                    {
                        //待っているルームも満員
                        //キャッシュリストから満員でないルーム、or 新しいルームをクリエイト
                        if (roomCache.Count > 0)
                        {
                            roomCache.TryPop(out room);
                            room.teamRed.Add(userId);
                            roomMap.TryAdd(room.id, room);
                            userRoom.TryAdd(userId, room.id);
                        }
                        else
                        {
                            room = new MatchRoom();
                            room.id = index.GetAndAdd();
                            room.teamRed.Add(userId);
                            roomMap.TryAdd(room.id, room);
                            userRoom.TryAdd(userId, room.id);
                        }
                    }
                }
                else
                {
                    //満員ルームないの場合
                    //キャッシュリストから満員でないルーム、or 新しいルームをクリエイト
                    if (roomCache.Count > 0)
                    {
                        roomCache.TryPop(out room);
                        room.teamRed.Add(userId);
                        roomMap.TryAdd(room.id, room);
                        userRoom.TryAdd(userId, room.id);
                    }
                    else
                    {
                        room = new MatchRoom();
                        room.id = index.GetAndAdd();
                        room.teamRed.Add(userId);
                        roomMap.TryAdd(room.id, room);
                        userRoom.TryAdd(userId, room.id);
                    }
                }
                //どんな方式ルームに入っても、ルーム満員判断
                //もし満員なら、このルームをキャッシュリストに入れって
                if(room.teamRed.Count == room.teamBule.Count && room.teamRed.Count == room.teamMax)
                {
                    //ここでキャンピオン選びモジュールを知らせ
                    //クライアントも知らせ
                    //todo
                    EventUtil.createSelect(room.teamRed, room.teamBule);
                    WriteToUsers(room.teamRed.ToArray(), Type, 0, MatchProtocol.ENTER_SELECT_BRO,null);
                    WriteToUsers(room.teamBule.ToArray(), Type, 0, MatchProtocol.ENTER_SELECT_BRO, null);


                    //remove ユーザーとルームのマップ
                    foreach (int iteroom in room.teamRed)
                    {
                        int i;
                        userRoom.TryRemove(iteroom, out i);
                    }
                    foreach (int iteroom in room.teamBule)
                    {
                        int i;
                        userRoom.TryRemove(iteroom, out i);
                    }
                    //reset room data 
                    room.teamRed.Clear();
                    room.teamBule.Clear();
                    //このルームを待ちリストを移除
                    roomMap.TryRemove(room.id, out room);
                    //ルームをキャッシュリストに入れる
                    roomCache.Push(room);
                }
            }
        }

        /// <summary>
        /// ユーザーノーマルマッチング離れる
        /// </summary>
        /// <param name="token"></param>
        private void Leave(UserToken token)
        {
            //ユーザー主キーIDを取得する
            int userId = GetUserId(token);
            Console.WriteLine("ノーマルマッチングキャンセル" + userId);
            //ユーザーがroomとのマップ関係あるか
            if (!userRoom.ContainsKey(userId))
            {
                return;
            }
            //ユーザー居っているルームIDを取得
            int roomId = userRoom[userId];
            //ルーム存在かを判断
            if(roomMap.ContainsKey(roomId))
            {
                MatchRoom room = roomMap[roomId];
                //ユーザー自分のチームから削除
                if(room.teamRed.Contains(userId))
                {
                    room.teamRed.Remove(userId);
                }
                else if(room.teamBule.Contains(userId))
                {
                    room.teamBule.Remove(userId);
                }
                //ユーザーとルームのマップ関係を削除
                userRoom.TryRemove(userId, out roomId);
                //このユーザーはこのルームの最後の人なら、このルームを削除、ちなみにキャッシュリストに入る
                if(room.teamRed.Count + room.teamBule.Count == 0)
                {
                    roomMap.TryRemove(roomId, out room);
                    roomCache.Push(room);
                }
            }
        }

        public override byte Type
        {
            get
            {
                return GameProtocol.TYPE_MATCH;
            }
        }
    }
}
