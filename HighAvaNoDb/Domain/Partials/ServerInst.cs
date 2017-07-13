using HighAvaNoDb.Events;
using HighAvaNoDb.Infrastructure.Exceptions;
using System;

namespace HighAvaNoDb.Domain
{
    public partial class ServerInst : 
        IHandle<ItemPingedEvent>, 
        IHandle<ItemSlavedOfNoneEvent>, 
        IHandle<ItemSlavedOfEvent>,
        IHandle<ItemZkRegisteredEvent>,
        IHandle<ItemZkUnRegisteredEvent>
    {
        public void Handle(ItemSlavedOfEvent e)
        {
            IsLeader = false;
        }

        public void Handle(ItemZkUnRegisteredEvent e)
        {
            IsZKRegistered = false;
        }

        public void Handle(ItemZkRegisteredEvent e)
        {
            IsZKRegistered = true;
        }

        public void Handle(ItemSlavedOfNoneEvent e)
        {
            IsLeader = true;
        }

        public void Handle(ItemPingedEvent e)
        {
            if (e.ServerId != this.Id.ToString())
            {
                throw new HandleBadEventException<ItemPingedEvent>(e, String.Format("ServerInst Id={0}",e.ServerId));
            }

            if (e.Milliseconds < int.MaxValue)
            {
                IsConnected = true;
            }
            else
            {
                IsConnected = false;
            }
        }
    }
}
