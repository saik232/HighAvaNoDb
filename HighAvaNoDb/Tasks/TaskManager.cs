using HighAvaNoDb.Common;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace HighAvaNoDb.Tasks
{
    /// <summary>
    /// 任务管理
    /// </summary>
    public class TaskManager
    {
        private static readonly TaskManager taskManager = new TaskManager();
        private readonly List<TaskThread> taskThreads = new List<TaskThread>();

        private TaskManager()
        {
        }
        
        /// <summary>
        /// 加载任务
        /// </summary>
        public void Initialize()
        {
            this.taskThreads.Clear();

            var taskSchedule = HAContext.Current.ContainerManager.Resolve<TaskSchedule>();
            var scheduleTasks = taskSchedule.GetAllTasks();

            foreach (var scheduleTask in scheduleTasks)
            {
                TaskThread taskThread = new TaskThread(scheduleTask);
                this.taskThreads.Add(taskThread);
            }
        }

        /// <summary>
        /// 启动任务
        /// </summary>
        public void Start()
        {
            foreach (var taskThread in this.taskThreads)
            {
                taskThread.InitTimer();
            }
        }

        /// <summary>
        /// 停止任务
        /// </summary>
        public void Stop()
        {
            foreach (var taskThread in this.taskThreads)
            {
                taskThread.Dispose();
            }
        }

        /// <summary>
        /// 调度新任务
        /// </summary>
        /// <param name="task"></param>
        public void ScheduleTask(ScheduleTask task)
        {
            TaskThread taskThread = new TaskThread(task);
            this.taskThreads.Add(taskThread);
            taskThread.InitTimer();
        }

        /// <summary>
        /// 取消任务
        /// </summary>
        /// <param name="name"></param>
        public void CancelTask(string name)
        {
            foreach (var item in taskThreads)
            {
                if (item.Name == name)
                {
                    item.Dispose();
                    break;
                }
            }
        }

        /// <summary>
        /// static TaskManager
        /// </summary>
        public static TaskManager Instance
        {
            get
            {
                return taskManager;
            }
        }

        /// <summary>
        /// 获取TaskThread
        /// </summary>
        public IList<TaskThread> TaskThreads
        {
            get
            {
                return new ReadOnlyCollection<TaskThread>(this.taskThreads);
            }
        }
    }
}
