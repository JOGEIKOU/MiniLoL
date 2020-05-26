using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetFramework;
using NetFramework.auto;

namespace MyLoLServer.Logic
{
    public interface HandlerInterface
    {
        void ClientClose(UserToken token, string error);

        //void ClientConnect(UserToken token);

        void MessageReceive(UserToken token, SokectModel message);
    }
}
