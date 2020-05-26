using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace MyLoLServer.Tool
{
    public delegate void TimeEvent();
    public class ScheduleUtil
    {
        private static ScheduleUtil util;
        public static ScheduleUtil Instance
        {
            get
            {
                if(util == null)
                {
                    util = new ScheduleUtil();
                }
                return util;
            }
        }

        Timer timer;

        private ConcurrentInt index = new ConcurrentInt();

        //待ち実行タスク
        private Dictionary<int, TimeTaskModel> mission = new Dictionary<int, TimeTaskModel>();
        /// <summary>
        /// 待ち削除リスト
        /// </summary>
        private List<int> removeList = new List<int>();

        private ScheduleUtil()
        {
            timer = new Timer(200);
            timer.Elapsed += Callback;
            timer.Start();
        }

        void Callback(object sender,ElapsedEventArgs e)
        {
            lock(mission)
            {
                lock(removeList)
                {
                    foreach (int item in removeList)
                    {
                        mission.Remove(item);
                    }
                    removeList.Clear();
                    foreach (TimeTaskModel item in mission.Values)
                    {
                        if(item.time <= DateTime.Now.Ticks)
                        {
                            item.Run();
                            removeList.Add(item.id);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// タスクタイム単位ms
        /// </summary>
        /// <param name="task"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        public int Schedule(TimeEvent task,long delay)
        {
            //ミリ秒ー＞マイクロ秒
            return Schedukemms(task,delay*1000*1000);
        }

        /// <summary>
        /// タスク単位マイクロ秒
        /// </summary>
        /// <param name="task"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        private int Schedukemms(TimeEvent task, long delay)
        {
            lock(mission)
            {
                int id = index.GetAndAdd();
                TimeTaskModel model = new TimeTaskModel(id, task, DateTime.Now.Ticks + delay);
                mission.Add(id, model);
                return id;
            }
        }

        /// <summary>
        /// 待ちミッションリストを削除
        /// </summary>
        /// <param name="id"></param>
        public void RemoveMission(int id)
        {
            lock(removeList)
            {
                removeList.Add(id);
            }
        }


        public int Schedule(TimeEvent task,DateTime time)
        {
            long t = time.Ticks - DateTime.Now.Ticks;
            t = Math.Abs(t);
            return Schedukemms(task, t);
        }

        public int TimeSchedule(TimeEvent task,long time)
        {
            long t = time - DateTime.Now.Ticks;
            t = Math.Abs(t);
            return Schedukemms(task, t);
        }


    }
}
