using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyLoLServer.Tool
{
    /// <summary>
    /// シングルスレッドデリゲート
    /// </summary>
    public delegate void ExecutorDelegate();

    /// <summary>
    /// シングルスレッド対象処理
    /// </summary>
    public class ExecutorPool
    {
        private static ExecutorPool instance;

        /// <summary>
        /// スレッド同期lock
        /// </summary>
        Mutex tex = new Mutex();

        /// <summary>
        /// シングルトン
        /// </summary>
        public static ExecutorPool Instance { get { if (instance == null) { instance = new ExecutorPool(); } return instance; } }

        /// <summary>
        /// シングルロジック処理
        /// </summary>
        /// <param name="d"></param>
        public void execute(ExecutorDelegate d)
        {
            lock(this)
            {
                tex.WaitOne();
                d();
                tex.ReleaseMutex();
            }
        }





    }
}
