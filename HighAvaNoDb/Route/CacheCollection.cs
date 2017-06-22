using System.Collections.Generic;
using HighAvaNoDb.Domain;
using System;
using System.Collections;

namespace HighAvaNoDb.Route
{

    public class CacheCollection : IDictionary<string, Shard>
    {
        #region field
        private readonly string name;
        private readonly IDictionary<string, Shard> shards;
        /// <summary>
        /// Cached
        /// </summary>
        private readonly IDictionary<string, Shard> activeShards;
        private readonly Router router;
        #endregion

        public CacheCollection(string name, IDictionary<string, Shard> shards, IDictionary<string, object> props, Router router)
        {
            if (string.IsNullOrWhiteSpace(name) || shards == null)
            {
                throw new ArgumentNullException("Name or shards is null.");
            }

            this.name = name;

            this.shards = shards;
            this.activeShards = new Dictionary<string, Shard>();
            foreach (var item in shards)
            {
                if (item.Value.State == LiveState.ACTIVE)
                {
                    activeShards.Add(item.Key, item.Value);
                }
            }

            this.router = router;
        }

        #region Property


        /// <summary>
        /// Name
        /// </summary>
        public virtual string Name
        {
            get
            {
                return name;
            }
        }

        public virtual Shard getShard(string shardName)
        {
            return shards[shardName];
        }

        public virtual ICollection<Shard> Shards
        {
            get
            {
                return shards.Values;
            }
        }

        public virtual ICollection<Shard> ActiveShards
        {
            get
            {
                return activeShards.Values;
            }
        }

        public virtual IDictionary<string, Shard> ShardsMap
        {
            get
            {
                return shards;
            }
        }

        public virtual IDictionary<string, Shard> ActiveShardsMap
        {
            get
            {
                return activeShards;
            }
        }

        public virtual Router Router
        {
            get
            {
                return router;
            }
        }
        #endregion


        #region IDictionary implement

        public int Count
        {
            get
            {
                return shards.Count;
            }
        }

        public ICollection<string> Keys
        {
            get
            {
                return shards.Keys;
            }
        }

        public ICollection<Shard> Values
        {
            get
            {
                return shards.Values;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        Shard IDictionary<string, Shard>.this[string key]
        {
            set
            {
                throw new NotSupportedException("___");
            }
            get
            {
                return this[key];
            }
        }

        public Shard this[string key]
        {
            get
            {
                return shards[key];
            }
        }

        public IEnumerator GetEnumerator()
        {
            return shards.GetEnumerator();
        }

        public bool ContainsKey(string key)
        {
            return shards.ContainsKey(key);
        }

        public void Add(string key, Shard value)
        {
            if (value.State == LiveState.ACTIVE)
            {
                shards.Add(key, value);
                activeShards.Add(key, value);
            }
            else
            {
                shards.Add(key, value);
            }
        }

        public bool Remove(string key)
        {
            return shards.Remove(key) || activeShards.Remove(key);
        }

        public bool TryGetValue(string key, out Shard value)
        {
            return shards.TryGetValue(key, out value);
        }

        public void Add(KeyValuePair<string, Shard> item)
        {
            if (item.Value.State == LiveState.ACTIVE)
            {
                activeShards.Add(item.Key, item.Value);
            }
            else
            {
                shards.Add(item.Key, item.Value);
            }
        }

        public bool Contains(KeyValuePair<string, Shard> item)
        {
            return shards.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, Shard>[] array, int arrayIndex)
        {
            shards.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<string, Shard> item)
        {
            return shards.Remove(item) || activeShards.Remove(item);
        }

        public void Clear()
        {
            shards.Clear();
            activeShards.Clear();
        }

        IEnumerator<KeyValuePair<string, Shard>> IEnumerable<KeyValuePair<string, Shard>>.GetEnumerator()
        {
            return shards.GetEnumerator();
        }
        #endregion
    }

}