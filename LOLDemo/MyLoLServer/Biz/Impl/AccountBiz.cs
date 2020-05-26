using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyLoLServer.Cache;
using NetFramework;

namespace MyLoLServer.Biz.Impl
{
    public class AccountBiz : IAccountBiz
    {

        IAccountCache accountCache = CacheFactory.accountCache;

        public int Create(UserToken token, string account, string password)
        {
            if (accountCache.HasAccount(account)) return 1;
            accountCache.Add(account, password);
            return 0;
        }

        public int Login(UserToken token, string account, string password)
        {
            //入力ルール違反
            if (account == null || password == null) return -4;
            //カウンター存在しません
            if (!accountCache.HasAccount(account)) return -1;
            //カウンターログイン中
            if (accountCache.IsOnline(account)) return -2;
            //パスワード間違った
            if (!accountCache.Match(account, password)) return -3;
            //検証出来た
            accountCache.Online(token, account);
            return 0;
        }

        public int Get(UserToken token)
        {
            return accountCache.GetId(token);
        }

        public void Close(UserToken token)
        {
            accountCache.OffLine(token);
        }


    }
}
