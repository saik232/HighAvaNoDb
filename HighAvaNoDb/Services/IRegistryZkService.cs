using System;

namespace HighAvaNoDb.Services
{
    public interface IRegistryZkService
    {
        //private ZookeeperClient zookeeperClient;
        void RegistryZk(Guid id);
        void UnRegistry(Guid id);
    }
}
