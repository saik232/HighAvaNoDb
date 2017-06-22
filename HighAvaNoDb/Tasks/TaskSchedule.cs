using System.Collections.Generic;
using System.Linq;

namespace HighAvaNoDb.Tasks
{
    public class TaskSchedule
    {
        private static readonly Dictionary<string, ScheduleTask> scheduleTasks = new Dictionary<string, ScheduleTask>();
        /// <summary>
        /// 后期使用xml或二进制存储，用户界面添加任务
        /// </summary>
        public void CreateScheduleTasks()
        {
            ScheduleTask st1 = new ScheduleTask()
            {
                Seconds = 2,
                Type = "HighAvaNoDb.Tasks.Monitor.CacheMonitorTask",
                //装箱
                Parameters = new Dictionary<string, object>() { { "host", "127.0.0.1" }, { "port", 355 } }
            };
            scheduleTasks.Add(st1.Name, st1);
        }

        public List<ScheduleTask> GetAllTasks()
        {
            return scheduleTasks.Values.ToList();
        }
    }
}
