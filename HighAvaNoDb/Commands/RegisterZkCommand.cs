using System;

namespace HighAvaNoDb.Commands
{
    public class RegisterZkCommand : Command
    {
        public Guid ServerInstId { set; get; }

        public RegisterZkCommand(Guid id, Guid serverInstId, int version)
            :base(id,version)
        {
            this.ServerInstId = serverInstId;
        }
    }
}