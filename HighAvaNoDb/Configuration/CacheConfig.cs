using System.Configuration;

namespace HighAvaNoDb.Configuration
{
    public class CacheConfig : ConfigurationSection
    {
        public string RedisCachingConnectionString { get; internal set; }
    }
}