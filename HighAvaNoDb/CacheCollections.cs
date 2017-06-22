using HighAvaNoDb.Domain;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Collections.ObjectModel;
using HighAvaNoDb.Route;

namespace HighAvaNoDb
{
    /// <summary>
    /// Shard collection per Group, haven't been implemented
    /// </summary>
    ///castle.dynamicproxy
    public class CacheCollections
    {
        private static readonly ConcurrentDictionary<String, CacheCollection> items;

        static CacheCollections()
        {
            items = new ConcurrentDictionary<String, CacheCollection>();
        }

        public CacheCollection GetByName(String name)
        {
            if (items.ContainsKey(name))
            {
                return items[name];
            }
            return null;
        }

        public bool Add(CacheCollection item)
        {
            return items.TryAdd(item.Name, item);
        }

        public bool Remove(string id)
        {
            CacheCollection col;
            return items.TryRemove(id, out col);
        }

        public ReadOnlyCollection<CacheCollection> Items
        {
            get
            {
                return new ReadOnlyCollection<CacheCollection>(items.Values.ToList());
            }
        }
    }
}
