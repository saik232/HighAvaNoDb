using System;
using System.Collections.Generic;

namespace HighAvaNoDb.Domain
{
    /// <summary>
    /// Value object
    /// </summary>
    public partial class Server : IEqualityComparer<Server>, ICloneable
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
            if (String.IsNullOrWhiteSpace(obj.Host))
            {
                int intHost;
                if (int.TryParse(obj.Host.Replace(".", ""), out intHost))
                {
                    return obj.Port + intHost;
                }
            }
            return obj.Port;
        }

        public object Clone()
        {
            Server ser = new Server();
            ser.Host = this.Host;
            ser.IsAlive = this.IsAlive;
            ser.NodeName = this.NodeName;
            ser.paramStr = this.paramStr;
            ser.Path = this.Path;
            ser.Port = this.Port;
            ser.ShardName = this.ShardName;
            return ser;
        }
    }
}
