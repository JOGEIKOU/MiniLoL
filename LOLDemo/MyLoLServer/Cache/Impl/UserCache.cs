using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyLoLServer.Dao.Model;
using NetFramework;

namespace MyLoLServer.Cache.Impl
{
    public class UserCache : IUserCache
    {
        /// <summary>
        /// ユーザーIDとモデルのマップ
        /// </summary>
        Dictionary<int, UserModel> idToModel = new Dictionary<int, UserModel>();

        /// <summary>
        /// ユーザーIDとカウンターIDのマップ
        /// </summary>
        Dictionary<int, int> accToUid = new Dictionary<int, int>();

        Dictionary<int, UserToken> idToToken = new Dictionary<int, UserToken>();
        Dictionary<UserToken , int> tokenToId = new Dictionary<UserToken , int>();

        int index = 0;
        public bool CreatAccount(UserToken token, string msgName, int accountId)
        {
            UserModel usermodel = new UserModel();
            usermodel.name = msgName;
            usermodel.id = index++ ;
            usermodel.accountId = accountId;
            List<int> list = new List<int>();
            for(int i = 0;i<9;i++)
            {
                list.Add(i);
            }
            usermodel.championList = list;
            //クリエイトできた、カウンターIDとユーザーのセット
            accToUid.Add(accountId, usermodel.id);
            //クリエイトできた、ユーザーIDとユーザーモデルのセット
            idToModel.Add(usermodel.id, usermodel);
            return true;
        }

        public bool HasCampion(UserToken token)
        {
            return tokenToId.ContainsKey(token);
        }

        public bool HasCampionByAccountId(int id)
        {
            return accToUid.ContainsKey(id);
        }

        public UserModel GetUserInfo(UserToken token)
        {
            if (!HasCampion(token)) return null;

            return idToModel[tokenToId[token]];
        }

        public UserModel GetUserInfo(int id)
        {
            return idToModel[id];
        }

        public void UserOffline(UserToken token)
        {
            if (tokenToId.ContainsKey(token))
            {
                if (idToModel.ContainsKey(tokenToId[token]))
                {
                    idToToken.Remove(tokenToId[token]);
                }
                tokenToId.Remove(token);
            }
        }

        public UserToken GetToken(int id)
        {
            return idToToken[id];
        }


        public UserModel UserOnline(UserToken token, int id)
        {
            idToToken.Add(id, token);
            tokenToId.Add(token, id);
            return idToModel[id];
        }


        public UserModel GetByAccountId(int accId)
        {
            if (!accToUid.ContainsKey(accId)) return null;

            return idToModel[accToUid[accId]];
        }

        public bool isOnline(int id)
        {
            return idToToken.ContainsKey(id);
        }

    }
}
