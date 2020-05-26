using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyLoLServer.Cache;
using MyLoLServer.Dao.Model;
using NetFramework;

namespace MyLoLServer.Biz.Impl
{
    /// <summary>
    /// ユーザー処理
    /// </summary>
    public class UserBiz : IUserBiz
    {
        IAccountBiz accBiz = BizFactory.accountBiz;
        IUserCache userCache = CacheFactory.userCache;
        public bool CreateAccount(UserToken token, string msgUserName)
        {
            //カウンターログイン中か
            int accountId = accBiz.Get(token);
            if (accountId == -1) return false;

            //このカウンダ存在したキャンピオンあるか
            if (userCache.HasCampionByAccountId(accountId)) return false;
            userCache.CreatAccount(token, msgUserName,accountId);

            return true;
        }

        public UserModel GetByAccount(UserToken token)
        {
            //カウンターログイン中か
            int accountId = accBiz.Get(token);
            if (accountId == -1) return null;

            return userCache.GetByAccountId(accountId);
        }

        public UserModel GetUserInfo(int id)
        {
            return userCache.GetUserInfo(id);
        }

        public UserModel UserOnline(UserToken token)
        {
            //カウンターログイン中か
            int accountId = accBiz.Get(token);
            if (accountId == -1) return null;
            UserModel usermodel = userCache.GetByAccountId(accountId);
            if (userCache.isOnline(usermodel.id)) return null;
            userCache.UserOnline(token, usermodel.id);
            return usermodel;
        }

        public void UserOffline(UserToken token)
        {
            userCache.UserOffline(token);
        }

        public UserToken GetToken(int id)
        {
            return userCache.GetToken(id);
        }

        public UserModel GetUserInfo(UserToken token)
        {
            return userCache.GetUserInfo(token);
        }
    }
}
