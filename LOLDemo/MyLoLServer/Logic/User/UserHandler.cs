using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyLoLServer.Biz;
using MyLoLServer.Dao.Model;
using MyLoLServer.Tool;
using NetFramework;
using NetFramework.auto;
using Protocol;
using Protocol.Dto;

namespace MyLoLServer.Logic.User
{
    public class UserHandler : AbsOnceHandler, HandlerInterface
    {
        IUserBiz userBiz = BizFactory.userBiz;

        public void ClientClose(UserToken token, string error)
        {
            userBiz.UserOffline(token);
        }

        public void ClientConnect(UserToken token)
        {

        }

        public void MessageReceive(UserToken token, SokectModel message)
        {
            switch(message.command)
            {
                case UserProtocol.CREATE_CREQ:
                    CreateAccount(token, message.GetMessage<string>());
                    break;
                case UserProtocol.INFO_CREQ:
                    AccountInfo(token);
                    break;
                case UserProtocol.ONLINE_CREQ:
                    AccountOnline(token);
                    break;
            }
        }


        private void CreateAccount(UserToken token,string msgUserName)
        {
            ExecutorPool.Instance.execute(
                delegate ()
                {
                    Write(token, UserProtocol.CREATE_SRES,
                    userBiz.CreateAccount(token, msgUserName));
                }
                );
        }

        private void AccountInfo(UserToken token)
        {
            ExecutorPool.Instance.execute(
                delegate ()
                {
                    Write(token, UserProtocol.INFO_SRES,
                       Convert(userBiz.GetByAccount(token)));
                }
                );
        }

        private void AccountOnline(UserToken token)
        {
            ExecutorPool.Instance.execute(
                delegate ()
                {
                    Write(token, UserProtocol.ONLINE_SRES,
                        Convert(userBiz.UserOnline(token)));
                }
                );
        }

        private UserDTO Convert(UserModel userModel)
        {
            if (userModel == null) return null;
            return new UserDTO(userModel.name, userModel.id, userModel.level, userModel.wincount, userModel.losecount, userModel.rancount);
        }

        public override byte Type
        {
            get
            {
                return GameProtocol.TYPE_USER;
            }
        }
    }
}
