using System;

namespace HighAvaNoDb.Commands
{
    public class BeLeaderCommand : Command
    {
        public Guid ServerId { set; get; }

        public BeLeaderCommand(Guid aggregateId,Guid ServerId, int version)
            : base(aggregateId, version)
        {
            this.ServerId = ServerId;
        }
    }
}
