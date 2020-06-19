using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetFramework;
using NetFramework.auto;
using MyLoLServer.Logic;
using MyLoLServer.Logic.Login;
using Protocol;
using MyLoLServer.Logic.User;
using MyLoLServer.Logic.Match;
using MyLoLServer.Logic.Select;
using MyLoLServer.Logic.Fight;

namespace MyLoLServer
{
    public class HandlerCenter : AbsHandlerCenter
    {
        HandlerInterface login;
        HandlerInterface user;
        HandlerInterface match;
        HandlerInterface select;
        HandlerInterface fight;

        public HandlerCenter()
        {
            login = new LoginHandler();
            user = new UserHandler();
            match = new MatchHandler();
            select = new SelectChampionHandler();
            fight = new FightHandler();
        }

        public override void ClientClose(UserToken token, string error)
        {
            Console.WriteLine("クライアント断絶しました。");
            select.ClientClose(token, error);
            match.ClientClose(token, error);
            fight.ClientClose(token, error);

            user.ClientClose(token, error);
            login.ClientClose(token,error);
        }

        public override void ClientConnect(UserToken token)
        {
            Console.WriteLine("クライアント接続しました。");
        }

        public override void MessageReceive(UserToken token, object message)
        {
            SokectModel model = message as SokectModel;
            switch(model.type)
            {
                case GameProtocol.TYPE_LOGIN:
                    login.MessageReceive(token, model);
                    break;
                case GameProtocol.TYPE_USER:
                    user.MessageReceive(token,model);
                    break;
                case GameProtocol.TYPE_MATCH:
                    match.MessageReceive(token, model);
                    break;
                case GameProtocol.TYPE_SELECT:
                    select.MessageReceive(token, model);
                    break;
                case GameProtocol.TYPE_FIGHT:
                    fight.MessageReceive(token, model);
                    break;
                default:
                    //未知モジュール、クライアントは違法ツールを使うかも
                    break;
            }
        }
    }
}
