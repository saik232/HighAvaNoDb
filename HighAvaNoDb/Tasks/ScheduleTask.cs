using System;
using System.Collections.Generic;

namespace HighAvaNoDb.Tasks
{
    /// <summary>
    /// Schedule task
    /// </summary>
    [Serializable]
    public partial class ScheduleTask
    {
        public Dictionary<string, object> Parameters { set; get; }

        private string name;
        /// <summary>
        /// 唯一名称
        /// </summary>
        public string Name
        {
            get
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    name = Guid.NewGuid().ToString();
                }
                return name;
            }
            set
            {
                name = value;
            }
        }

        /// <summary>
        /// 执行间隔时间
        /// </summary>
        public int Seconds { get; set; }

        /// <summary>
        /// ITask
        /// </summary>
        public string Type { get; set; }
    }
}
