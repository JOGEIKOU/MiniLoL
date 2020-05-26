using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyLoLServer.Dao.Model;
using NetFramework;

namespace MyLoLServer.Cache.Impl
{
    public class AccountCache : IAccountCache
    {
        public int index = 0;
        /// <summary>
        /// ユーサオブジェクトとカウンダのマップする
        /// </summary>
        Dictionary<UserToken, string> OnlineAccMap = new Dictionary<UserToken, string>();
        /// <summary>
        /// カウンダと自分属性のマップする
        /// </summary>
        Dictionary<string, AccountModel> accMap = new Dictionary<string, AccountModel>();

        public int GetId(UserToken token)
        {
            //辞書の中にこのカウンダ存在か
            if (!OnlineAccMap.ContainsKey(token)) return -1;
            //カウンダIDを戻す
            return accMap[OnlineAccMap[token]].id;
        }

        public bool HasAccount(string account)
        {
            return accMap.ContainsKey(account);
        }

        public bool IsOnline(string account)
        {
            //辞書の中にこのカウンダあるか
            return OnlineAccMap.ContainsValue(account);
        }

        public bool Match(string account, string password)
        {
            //カウンダ存在か
            if (!HasAccount(account)) return false;
            //カウンダのパスワード正しいか？
            return accMap[account].password.Equals(password);
            
        }

        public void OffLine(UserToken token)
        {
            if (OnlineAccMap.ContainsKey(token)) OnlineAccMap.Remove(token);
        }

        public void Online(UserToken token, string account)
        {

            //map 追加
            OnlineAccMap.Add(token, account);
        }

        public void Add(string account, string password)
        {
            //カウンダ実例化
            AccountModel model = new AccountModel();
            model.account = account;
            model.password = password;
            model.id = index;
            accMap.Add(account, model);
            index++;
        }
    }
}
