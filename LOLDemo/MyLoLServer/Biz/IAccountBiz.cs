using NetFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLoLServer.Biz
{
    public interface IAccountBiz
    {
        /// <summary>
        /// カウントをクリエイト
        /// </summary>
        /// <param name="token"></param>
        /// <param name="account">カウンターID</param>
        /// <param name="password">パスワード</param>
        /// <return>戻り値：0 = 成功</return> 
        /// <return>戻り値：1 = このIDはほかのユーザーがクリエイトしました</return> 
        /// <return>戻り値：2 = IDルール違反</return> 
        /// <return>戻り値：3 = パスワードルール違反</return> 
        int Create(UserToken token , string account , string password);

        /// <summary>
        /// ログイン
        /// </summary>
        /// <param name="token"></param>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <return>戻り値：0 = 成功</return> 
        /// <return>戻り値：1 = パスワード間違いました</return> 
        /// <return>戻り値：-1 = カウンター存在しません</return> 
        /// <return>戻り値：-2 = カウンターログイン中</return> 
        ///  <return>戻り値：-3 = パスワード間違った</return> 
        /// <return>戻り値：-4 = 入力ルール違反</return> 
        int Login(UserToken token, string account, string password);

        /// <summary>
        /// クライアントから断絶された
        /// </summary>
        /// <param name="token"></param>
        void Close(UserToken token);


        /// <summary>
        /// ID取得する
        /// </summary>
        /// <param name="token"></param>
        /// <returns>ユーザーのログインIDを戻す</returns>
        int Get(UserToken token);
    }
}
