using System;

namespace HighAvaNoDb.Commands
{
    public class PingCommand : Command
    {
        public string Host { get; private set; }
        public string ServerId { get; private set; }
        public int Port { get; private set; }

        public PingCommand(Guid aggregareId, string id, string host, int port, int version)
            :base(aggregareId, version)
        {
            this.ServerId = id;
            this.Host = host;
            this.Port = port;
        }
    }
}
