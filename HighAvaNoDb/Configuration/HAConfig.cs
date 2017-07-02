using System;
using System.Configuration;

namespace HighAvaNoDb.Configuration
{
    public class HAConfig
    {
        public string RedisCachingConnectionString { get; internal set; }

        public string ZKConnectString { set; get; }

        public TimeSpan ZKSessionTimeOut { set; get; }
    }
}