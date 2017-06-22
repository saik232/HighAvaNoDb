using System;

namespace HighAvaNoDb.Commands
{
    public class UnRegisterZkCommand : Command
    {
        public Guid ServerInstId { set; get; }

        public UnRegisterZkCommand(Guid id, Guid serverInstId, int version)
            :base(id,version)
        {
            this.ServerInstId = serverInstId;
        }
    }
}