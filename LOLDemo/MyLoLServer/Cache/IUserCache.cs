using MyLoLServer.Dao.Model;
using NetFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLoLServer.Cache
{
    public interface IUserCache
    {
        /// <summary>
        /// カウンターをクリエイト
        /// </summary>
        /// <param name="token"></param>
        /// <param name="msgName"></param>
        /// <returns></returns>
        bool CreatAccount(UserToken token,string msgName,int accountId);

        /// <summary>
        /// キャンピオンを持ってるか
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        bool HasCampion(UserToken token);

        /// <summary>
        /// ユーザーIDでキャンピオンを持ってるか判断
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        bool HasCampionByAccountId(int id);

        /// <summary>
        /// ユーザーコネクトで情報取得
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        UserModel GetUserInfo(UserToken token);

        /// <summary>
        /// ユーザーIDで情報取得
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        UserModel GetUserInfo(int id);

        /// <summary>
        /// ユーザーログイン
        /// </summary>
        /// <param name="token"></param>
        void UserOffline(UserToken token);

        /// <summary>
        /// ユーザーOfline
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        UserModel UserOnline(UserToken token,int id);

        /// <summary>
        /// idでコネクトする
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        UserToken GetToken(int id);

        /// <summary>
        /// カウンターIDでキャラクターを取得する
        /// </summary>
        /// <param name="accId"></param>
        /// <returns></returns>
        UserModel GetByAccountId(int accId);

        /// <summary>
        /// ユーサはOnlieか
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool isOnline(int id);
    }
}
