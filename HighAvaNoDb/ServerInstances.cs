using HighAvaNoDb.Domain;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Collections.ObjectModel;

namespace HighAvaNoDb
{
    /// <summary>
    /// ServerInst保存在集合
    /// </summary>
    public class ServerInstances
    {
        private static readonly ConcurrentDictionary<Guid, ServerInst> items;

        static ServerInstances()
        {
            items = new ConcurrentDictionary<Guid, ServerInst>();
        }

        public ServerInst GetById(Guid id)
        {
            if (items.ContainsKey(id))
            {
                return items[id];
            }
            return null;
        }

        public bool Add(ServerInst item)
        {
            return items.TryAdd(item.Id, item);
        }

        public bool Remove(Guid id)
        {
            ServerInst inst;
            return items.TryRemove(id, out inst);
        }

        public IEnumerable<ServerInst> GetByShardName(string shardName)
        { 
            foreach (var item in items)
            {
                if (item.Value.Server.ShardName == shardName)
                {
                    yield return item.Value;
                }
            }
        }

        public ReadOnlyCollection<ServerInst> Items
        {
            get
            {
                return new ReadOnlyCollection<ServerInst>(items.Values.ToList());
            }
        }
    }
}
