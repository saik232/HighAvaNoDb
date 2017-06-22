using System.Net;
using StackExchange.Redis;

namespace HighAvaNoDb.Infrastructure.Caching
{
    public class RedisConnectionWrapper : IRedisConnectionWrapper
    {
        private readonly string connectionString;

        private volatile ConnectionMultiplexer connection;
        private readonly object _lock = new object();

        public RedisConnectionWrapper(string connectionString)
        {
            this.connectionString = connectionString;
        }

        private ConnectionMultiplexer GetConnection()
        {
            if (connection != null && connection.IsConnected) return connection;

            lock (_lock)
            {
                if (connection != null && connection.IsConnected) return connection;

                if (connection != null)
                {
                    connection.Dispose();
                }
                connection = ConnectionMultiplexer.Connect(connectionString);
            }

            return connection;
        }

        public IDatabase Database(int? db = null)
        {
            return GetConnection().GetDatabase(db ?? -1);
        }

        public IServer Server(EndPoint endPoint)
        {
            return GetConnection().GetServer(endPoint);
        }

        public IServer Server(string host, int port)
        {
            return GetConnection().GetServer(host, port);
        }

        public EndPoint[] GetEndpoints()
        {
            return GetConnection().GetEndPoints();
        }

        public void FlushDb(int? db = null)
        {
            var endPoints = GetEndpoints();

            foreach (var endPoint in endPoints)
            {
                Server(endPoint).FlushDatabase(db ?? -1);
            }
        }

        public void Dispose()
        {
            if (connection != null)
            {
                connection.Dispose();
            }
        }
    }
}
