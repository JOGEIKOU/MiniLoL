using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyLoLServer.Tool
{
    public class ConcurrentInt
    {
        int value;
        Mutex tex = new Mutex();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ConcurrentInt() { }
        public ConcurrentInt(int value)
        {
            this.value = value;
        }

        /// <summary>
        /// int++
        /// </summary>
        /// <returns></returns>
        public int GetAndAdd()
        {
            lock(this)
            {
                tex.WaitOne();
                value++;
                tex.ReleaseMutex();
                return value;
            }
        }

        /// <summary>
        /// int--
        /// </summary>
        /// <returns></returns>
        public int GetAndReduce()
        {
            lock (this)
            {
                tex.WaitOne();
                value--;
                tex.ReleaseMutex();
                return value;
            }
        }

        /// <summary>
        /// reset value
        /// </summary>
        public void Reset()
        {
            tex.WaitOne();
            value=0;
            tex.ReleaseMutex();
        }

        public int GetValue()
        {
            return value;
        }
    }
}
