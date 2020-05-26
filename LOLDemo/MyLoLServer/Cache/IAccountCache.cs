using NetFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLoLServer.Cache
{
    public interface IAccountCache
    {
        /// <summary>
        /// カウンター存在か？
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        bool HasAccount(string account);

        /// <summary>
        /// IDとパスワードマーチングか
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        bool Match(string account, string password);

        /// <summary>
        /// カウンターonlineか
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        bool IsOnline(string account);

        /// <summary>
        /// ユーザーID取得
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        int GetId(UserToken token);

        /// <summary>
        /// ユーサーOnline
        /// </summary>
        /// <param name="token"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        void Online(UserToken token, string account);

        /// <summary>
        /// ユーサーOffline
        /// </summary>
        /// <param name="token"></param>
        void OffLine(UserToken token);

        /// <summary>
        /// カウンダ追加
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        void Add(string account, string password);

    }
}
