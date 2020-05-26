using MyLoLServer.Biz;
using MyLoLServer.Tool;
using NetFramework;
using NetFramework.auto;
using Protocol;
using Protocol.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MyLoLServer.Logic.Login
{
    public class LoginHandler : AbsOnceHandler, HandlerInterface
    {
        IAccountBiz accountBiz = BizFactory.accountBiz;

        public void ClientClose(NetFramework.UserToken token, string error)
        {
            ExecutorPool.Instance.execute(
            delegate ()
            {
                 //ロジック処理
                 accountBiz.Close(token);
             }
             );
        }

        public void MessageReceive(NetFramework.UserToken token, NetFramework.auto.SokectModel message)
        {
            switch (message.command)
            {
                case LoginProtocol.LOGIN_CREQ:
                    login(token, message.GetMessage<AccountInfoDTO>());
                    break;
                case LoginProtocol.REG_CREQ:
                    register(token, message.GetMessage<AccountInfoDTO>());
                    break;
            }
        }

        /// <summary>
        /// ログインメソッド
        /// </summary>
        /// <param name="token"></param>
        /// <param name="value"></param>
        public void login(UserToken token, AccountInfoDTO value)
        {
            ExecutorPool.Instance.execute(
                delegate ()
                {
                    //ロジック処理
                    int res = accountBiz.Login(token, value.account, value.password);
                    Write(token, LoginProtocol.LOGIN_SRES, res);
                }
                );
        }

        /// <summary>
        /// 登録メソッド
        /// </summary>
        /// <param name="token"></param>
        /// <param name="value"></param>
        public void register(UserToken token, AccountInfoDTO value)
        {
            ExecutorPool.Instance.execute(
                delegate ()
                {
                    //ロジック処理
                    int res = accountBiz.Create(token, value.account, value.password);
                    Write(token, LoginProtocol.REG_SRES, res);
                }
                );
        }

        public void ClientConnect(UserToken token)
        {

        }


        public override byte Type
        {
            get
            {
                return GameProtocol.TYPE_LOGIN;
            }
        }


    }
}
