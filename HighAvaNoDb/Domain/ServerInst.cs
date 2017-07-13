using HighAvaNoDb.Common;
using HighAvaNoDb.Events;
using HighAvaNoDb.Infrastructure.Caching;
using StackExchange.Redis;
using System;
using System.Collections.Generic;

namespace HighAvaNoDb.Domain
{
    public partial class ServerInst : AggregateRoot
    {
        public ICacheManager cacheManager { private set; get; }

        #region ctor
        public ServerInst(string host, int port, params string[] param)
        {
            ServerInfo = new Server();

            if (param != null && param.Length > 0)
            {
                string separetor = ",";
                ServerInfo.paramStr = String.Join(separetor, param);
            }

            cacheManager = HAContext.Current.ContainerManager.Resolve<ICacheManager>(
                new Dictionary<string, object>() { { "connectionString", String.Format("{0}:{1},{2}", host, port, ServerInfo.paramStr) } });
        }

        #endregion

        #region Properties

        public Server ServerInfo { private set; get; }

        //
        public bool IsConnected {private set; get; }
        public bool IsLeader {private set; get; }
        public bool IsZKRegistered { private set; get; }
        #endregion

        #region Method

        public IServer Server
        {
            get
            {
                return cacheManager.GetServer();
            }
        }
        public void SlaveOf(ServerInst master)
        {
            this.Server.SlaveOf(master.Server.EndPoint);
            ApplyChange(new ItemSlavedOfEvent(Guid.NewGuid(), master.Id, master.ServerInfo.Host, master.ServerInfo.Port,
                Id, ServerInfo.Host, ServerInfo.Port, -1));
        }

        public void BeMaster()
        {
            cacheManager.BeMaster();
            ApplyChange(new ItemSlavedOfNoneEvent(Guid.NewGuid(), Id, ServerInfo.Host, ServerInfo.Port, -1));
        }

        public TimeSpan Ping()
        {
            TimeSpan ts = cacheManager.Ping();
            ApplyChange(new ItemPingedEvent(Guid.NewGuid(), Id.ToString(), ServerInfo.Host, ServerInfo.Port, ts.Milliseconds, -1));
            return ts;
        }
        public override string ToString()
        {
            return String.Format("Id={0},Host={1},Port={2}", Id, ServerInfo.Host, ServerInfo.Port);
        }
        #endregion
    }
}
