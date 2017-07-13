using HighAvaNoDb.Domain;
using HighAvaNoDb.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HighAvaNoDb.Route
{
    public class ShardBuilder
    {
        private CacheCollections collections;

        private Range range;
        private LiveState state;
        private Shard shard;
        private string name;

        private List<Server> servers;

        public ShardBuilder()
        {
            servers = new List<Server>();
            collections = new CacheCollections();
        }

        public Shard Build()
        {
            if (shard == null)
            {
                shard = new Shard();
            }
            shard.Range = this.range;
            shard.Servers = this.servers;
            shard.State = state;
            shard.Name = name;
            return shard;
        }

        public ShardBuilder AddServer(Server server)
        {
            foreach (var item in collections.Items)
            {
                foreach (var shard in item.Shards)
                {
                    if (shard.Name == server.ShardName)
                    {
                        if (shard.Servers != null && shard.Servers.Contains(server, server as IEqualityComparer<Server>))
                        {
                            throw new UnKnownException(String.Format("server already in shard，server：{0}，shardName：{1}",server,shard.Name));
                        }

                        this.shard = shard;
                        this.range = shard.Range;
                        this.servers = shard.Servers;
                        this.state = shard.State;
                        break;
                    }
                }
            }

            servers.Add(server);
            this.name = server.ShardName;
            return this;
        }

        public ShardBuilder State(LiveState state)
        {
            this.state = state;
            return this;
        }

        public ShardBuilder Range(Range range)
        {
            this.range = range;
            return this;
        }

        public ShardBuilder Name(string name)
        {
            this.name = name;
            return this;
        }
    }
}
