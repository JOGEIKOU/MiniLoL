using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLoLServer.Tool
{
    public class TimeTaskModel
    {
        //タスク　ロジック
        private TimeEvent execut;
        //タスク実行の時間
        public long time;
        //タスクid
        public int id;

        public TimeTaskModel(int id , TimeEvent execut,long time)
        {
            this.id = id;
            this.execut = execut;
            this.time = time;
        }

        public void Run()
        {
            execut();
        }
    }
}
