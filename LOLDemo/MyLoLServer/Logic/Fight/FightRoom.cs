using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetFramework;
using NetFramework.auto;
using Protocol.Dto;

namespace MyLoLServer.Logic.Fight
{
    public class FightRoom : AbsMulitHandler, HandlerInterface
    {

        public void Init(SelectModel[] teamRed,SelectModel[] teamBlue)
        {

        }


        public void ClientClose(UserToken token, string error)
        {
            
        }

        public void MessageReceive(UserToken token, SokectModel message)
        {
            
        }
    }
}
