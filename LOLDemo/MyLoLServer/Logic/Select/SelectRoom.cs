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
    public class SelectRoom:AbsMulitHandler,HandlerInterface
    {
        public ConcurrentDictionary<int, SelectModel> teamRed = new ConcurrentDictionary<int, SelectModel>();
        public ConcurrentDictionary<int, SelectModel> teamBlue = new ConcurrentDictionary<int, SelectModel>();

        //今ルームにおる人数
        int EnterCount = 0;

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
            ScheduleUtil.Instance.Schedule(delegate
            {
                //20秒経って、全員入ってない場合、ルーム解散



            }, 20 * 1000);


        }


        public void ClientClose(UserToken token, string error)
        {
            
        }


        public void MessageReceive(UserToken token, SokectModel message)
        {
            
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
