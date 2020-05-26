using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFramework
{
    public class UserToPool
    {
        private Stack<UserToken> pool;

        public UserToPool(int max)
        {
            pool = new Stack<UserToken>(max);
        }

        /// <summary>
        /// 一つobjを取得する
        /// </summary>
        public UserToken pop()
        {
            return pool.Pop();
        }

        /// <summary>
        /// 一つobjを開放する
        /// </summary>
        public void push(UserToken token)
        {
            if(token != null)
            {
                pool.Push(token);
            }
        }

        public int Size
        {
            get { return pool.Count; }
        }


    }
}
