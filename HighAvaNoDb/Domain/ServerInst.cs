using HighAvaNoDb.Common;
using HighAvaNoDb.Events;
using HighAvaNoDb.Infrastructure.Caching;
using HighAvaNoDb.Model;
using StackExchange.Redis;
using System;

namespace HighAvaNoDb.Domain
{
    public partial class ServerInst : AggregateRoot
    {
        private ICacheManager cacheManager;
        private IServer cacheServer;

        #region ctor
        public ServerInst(string host, int port, params string[] param)
        {
            string separetor = ",";
            server = new Server() { Host = host, Port = port, paramStr = String.Join(separetor, param) };
            cacheManager = HAContext.Current.ContainerManager.Resolve<ICacheManager>();
        }
        #endregion

        #region Properties

        private Server server;
        public Server Server { get { return server; } }

        //
        public bool IsConnected { set; get; }
        #endregion

        #region Method
        public void Slave(Server master)
        {
            ServerInfo siM = new ServerInfo() { Host = server.Host, Port = server.Port };
            ServerInfo siS = new ServerInfo() { Host = server.Host, Port = server.Port };
            cacheManager.Slave(siM, siS);
            ApplyChange(new ItemSlavedOfEvent(Guid.NewGuid(), master.Id.ToString(),master.Host,master.Port,
                Id.ToString(), server.Host, server.Port, -1));
        }

        public void BeMaster()
        {
            ServerInfo si = new ServerInfo() { Host = server.Host, Port = server.Port };
            cacheManager.BeMaster(si);
            ApplyChange(new ItemSlavedOfNoneEvent(Guid.NewGuid(), Id.ToString(), server.Host, server.Port, -1));
        }

        public TimeSpan Ping()
        {
            ServerInfo si = new ServerInfo() { Host = server.Host, Port = server.Port };
            TimeSpan ts = cacheManager.Ping(si);
            ApplyChange(new ItemPingedEvent(Guid.NewGuid(), Id.ToString(), server.Host, server.Port, ts.Milliseconds, -1));
            return ts;
        }
        public override string ToString()
        {
            return String.Format("Id={0},Host={1},Port={2}",Id,server.Host,server.Port);
        }
        #endregion
    }
}
