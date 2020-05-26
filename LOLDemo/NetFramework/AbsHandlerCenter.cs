using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFramework
{
    public abstract class AbsHandlerCenter
    {
        /// <summary>
        /// クライアントアクセス
        /// </summary>
        /// <param name="token">クライアント対象</param>
        public abstract void ClientConnect(UserToken token);

        /// <summary>
        /// クライアントメッセージ承知した
        /// </summary>
        /// <param name="token">送信したクライアント</param>
        /// <param name="message">メッセージ内容</param>
        public abstract void MessageReceive(UserToken token, object message);

        /// <summary>
        /// クライアント断絶された
        /// </summary>
        /// <param name="token">断絶されたクライアント</param>
        /// <param name="error">断絶される時エラーメッセージ</param>
        public abstract void ClientClose(UserToken token, string error);
    }
}
