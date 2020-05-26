using NetFramework;
using NetFramework.auto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLoLServer.Logic
{
    public class AbsMulitHandler : AbsOnceHandler
    {
        public List<UserToken> list = new List<UserToken>();

        /// <summary>
        /// ルームに入るか判断
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool Enter(UserToken token)
        {
            if(list.Contains(token))
            {
                return false;
            }
            list.Add(token);
            return true;
        }

        /// <summary>
        /// ルームにおるか判断
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool IsEntered(UserToken token)
        {
            return list.Contains(token);
        }

        /// <summary>
        /// ルームに離れるか判断
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool Leave(UserToken token)
        {
            if(list.Contains(token))
            {
                list.Remove(token);
                return true;
            }
            return false;
        }

        #region Brocastメソッド

        public void Brocast(int command,object message,UserToken exToken = null)
        {
            Brocast(Area, command, message,exToken);
        }

        public void Brocast(int area,int command, object message,UserToken exToken = null)
        {
            Brocast(Type, area, command, message,exToken);
        }

        public void Brocast(byte type,int area,int command, object message, UserToken exToken = null)
        {
            byte[] value = MessageEncoding.encode(CreateSocketModel(type, area, command, message));
            value = LengthEncoding.encode(value);
            foreach (UserToken token in list)
            {
                if(token != exToken)
                {
                    byte[] bs = new byte[value.Length];
                    Array.Copy(value, 0, bs, 0, value.Length);
                    token.writed(bs);
                }
            }
        }
        #endregion


    }
}
