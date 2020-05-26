using MyLoLServer.Dao.Model;
using NetFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLoLServer.Biz
{
    public interface IUserBiz
    {
        /// <summary>
        /// 召喚使いをクリエイト
        /// </summary>
        /// <param name="token"></param>
        /// <param name="msgUserName"></param>
        /// <returns></returns>
        bool CreateAccount(UserToken token, string msgUserName);

        /// <summary>
        /// ユーザー情報取得
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        UserModel GetUserInfo(UserToken token);

        /// <summary>
        /// ユーザーID情報取得
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        UserModel GetUserInfo(int id);

        /// <summary>
        /// カウンターOnline
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        UserModel UserOnline(UserToken token);

        /// <summary>
        /// カウンターOffline
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        void UserOffline(UserToken token);

        /// <summary>
        /// IDでオブジェクトをアクセス
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        UserToken GetToken(int id);

        /// <summary>
        /// カウンダの接続対象で取得
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        UserModel GetByAccount(UserToken token);
    }
}
