using NetFramework;
using NetFramework.auto;
using MyLoLServer.Dao.Model;
using MyLoLServer.Biz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLoLServer.Logic
{
    public class AbsOnceHandler
    {
        public IUserBiz userBiz = BizFactory.userBiz;

        private byte _Type;
        private int _area;

        public  virtual byte Type { get => _Type; set => _Type = value; }
        public virtual int Area { get => _area; set => _area = value; }

        public void SetArea(int _area)
        {
            this._area = _area;
        }

        /// <summary>
        /// 接続対象でユーサを取得する
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public UserModel GetUser(UserToken token)
        {
            return userBiz.GetUserInfo(token);
        }

        /// <summary>
        /// 接続対象idでユーサを取得する
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public UserModel GetUser(int id)
        {
            return userBiz.GetUserInfo(id);
        }

        /// <summary>
        /// 接続対象でユーザーIDを取得する
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public int GetUserId(UserToken token)
        {
            UserModel user = GetUser(token);
            if (user == null) return -1;
            return user.id;
        }

        /// <summary>
        /// ユーザーIDで接続を取得する
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UserToken GetToken(int id)
        {
            return userBiz.GetToken(id);
        }


        #region アクセス対象でメッセージ送信メソッド

        public void Write(UserToken token,int command)
        {
            Write(token, command, null);
        }

        public void Write(UserToken token, int command , object message)
        {
            Write(token, Area, command, message);
        }

        public void Write(UserToken token,int area, int command, object message)
        {
            Write(token, Type, area, command, message);
        }

        public void Write(UserToken token,byte type,int area, int command, object message)
        {
            byte[] value = MessageEncoding.encode(CreateSocketModel(type,area,command,message));
            value = LengthEncoding.encode(value);
            token.writed(value);
        }

        #endregion

        #region ユーザーIDでメッセージ送信メソッド
        public void Write(int id , int command)
        {
            Write(id, command, null);
        }

        public void Write(int id, int command, object message)
        {
            Write(id, Area, command, message);
        }

        public void Write(int id, int area, int command, object message)
        {
            Write(id, Type, area, command, message);
        }

        public void Write(int id, byte type, int area, int command, object message)
        {
            UserToken token = GetToken(id);
            if (token == null) return;
            Write(token, type, area, command, message);
        }

        public void WriteToUsers(int[] users,byte type,int area,int command , object message)
        {
            byte[] value = MessageEncoding.encode(CreateSocketModel(type, area, command, message));
            value = LengthEncoding.encode(value);
            foreach (int uid in users)
            {
                UserToken token = userBiz.GetToken(uid);
                if (token == null) continue;
                byte[] bs = new byte[value.Length];
                Array.Copy(value, 0, bs, 0, value.Length);
                token.writed(bs);
            }
        }


        #endregion

        public SokectModel CreateSocketModel(byte type,int area, int command , object message)
        {
            return new SokectModel(type, area, command, message);
        }
    }
}
