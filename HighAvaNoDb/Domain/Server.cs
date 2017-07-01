using System;
using System.Collections.Generic;

namespace HighAvaNoDb.Domain
{
    public partial class Server : AggregateRoot, IEqualityComparer<Server>
    {
        //not null
        public string ShardName { set; get; }
        //not null
        public string Host { set; get; }
        //not null
        public int Port { set; get; }
        public string paramStr { set; get; }
        public bool IsAlive { set; get; }

        public override string ToString()
        {
            return String.Format("{0}:{1},{2},[Shard:{3}]", Host, Port, paramStr, ShardName);
        }

        public bool Equals(Server x, Server y)
        {
            return x.Host == y.Host && y.Port == y.Port;
        }

        //may be a litte complexed
        public int GetHashCode(Server obj)
        {
            return obj.Port;
        }
    }
}
