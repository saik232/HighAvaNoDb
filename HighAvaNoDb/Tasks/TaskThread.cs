using Autofac;
using HighAvaNoDb.Common;
using HighAvaNoDb.Infrastructure.Exceptions;
using System;
using System.Threading;

using ILog = log4net.ILog;

namespace HighAvaNoDb.Tasks
{
    /// <summary>
    /// 任务Timer
    /// </summary>
    public partial class TaskThread : IDisposable
    {
        private ILog logger;
        private Timer timer;
        private bool isDisposed;
        private ITask task;
        private ScheduleTask sTask;

        internal TaskThread(ScheduleTask sTask)
        {
            if(sTask==null)
            {
                throw new ArgumentNullException("sTask");
            }

            this.Seconds = sTask.Seconds;
            this.sTask = sTask;
        }

        private ITask CreateTask(ILifetimeScope scope)
        {
            logger = scope.Resolve<ILog>();
            try
            {
                ITask task = null;
                var type2 = System.Type.GetType(sTask.Type);
                if (type2 != null)
                {
                    object instance;
                    if (!HAContext.Current.ContainerManager.TryResolve(type2, scope, out instance))
                    {
                        instance = HAContext.Current.ContainerManager.ResolveWithParameters(type2, sTask.Parameters, scope);
                    }
                    task = instance as ITask;
                }
                return task;
            }
            catch (DependencyException ex)
            {
                logger.Error(string.Format("Error on Task dependency Resolving. Name={0}. Error={1}.", sTask.Name, ex.Message), ex);
                Dispose();
                return null;
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Error on Task Creating. Name={0}. Error={1}.", sTask.Name, ex.Message), ex);
                Dispose();
                return null;
            }
        }

        private void Run()
        {
            if (Seconds <= 0)
                return;

            this.StartedUtc = DateTime.UtcNow;
            this.IsRunning = true;
            task.Execute();
            this.IsRunning = false;
        }

        private void TimerHandler(object state)
        {
            this.timer.Change(-1, -1);
            this.Run();
            this.timer.Change(this.interval, this.interval);
        }

        /// <summary>
        /// Disposes
        /// </summary>
        public void Dispose()
        {
            if ((this.timer != null) && !this.isDisposed)
            {
                lock (this)
                {
                    this.timer.Dispose();
                    this.timer = null;
                    this.isDisposed = true;
                }
            }
        }

        /// <summary>
        /// Inits a timer
        /// </summary>
        public void InitTimer()
        {
            if (task != null)
            {
                task = CreateTask(HAContext.Current.ContainerManager.Scope());
            }

            if (this.timer == null)
            {
                this.timer = new Timer(new TimerCallback(this.TimerHandler), null, this.interval, this.interval);
            }
        }

        /// <summary>
        /// interval
        /// </summary>
        public int Seconds { get; set; }

        /// <summary>
        ///任务开始时间
        /// </summary>
        public DateTime StartedUtc { get; private set; }

        /// <summary>
        /// 是否运行
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// 任务执行间隔时间
        /// </summary>
        private int interval
        {
            get
            {
                return this.Seconds * 1000;
            }
        }

        public string Name
        {
            get { return sTask.Name; }
        }
    }
}
