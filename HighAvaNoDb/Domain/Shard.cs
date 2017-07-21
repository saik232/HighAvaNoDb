using HighAvaNoDb.Route;
using System.Collections.Generic;

namespace HighAvaNoDb.Domain
{
    /// <summary>
    /// 分片
    /// </summary>
    public class Shard : AggregateRoot
    {
        public string Name { set; get; }
        /// <summary>
        /// 先不使用Range
        /// </summary>
        public Range Range { get; set; }
        public static object ACTIVE { get; set; }
        public LiveState State { get; set; }
        /// <summary>
        /// 分片的所有主从redis
        /// </summary>
        public List<Server> Servers { set; get; }
    }
}
