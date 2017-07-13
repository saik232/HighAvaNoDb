using HighAvaNoDb.Commands;
using HighAvaNoDb.Common;
using HighAvaNoDb.Infrastructure.Exceptions;
using HighAvaNoDb.Repository;
using log4net;
using System;
using ZooKeeperNet;

namespace HighAvaNoDb.Tasks.Monitor
{
    public class CacheMonitorTask : ITask
    {
        protected ILog logger = LogManager.GetLogger(typeof(CacheMonitorTask));

        private const int FAIL_RETRY_TIMES = 3;

        private Guid serverId;
        private string host;
        private int port;
        private int failTimes;
        private readonly IServerInstRepository repository;

        public CacheMonitorTask(string host, int port)
        {
            this.host = host;
            this.port = port;

            repository = HAContext.Current.ContainerManager.Resolve<IServerInstRepository>();
            init();
        }

        private void init()
        {
            serverId = repository.GetByHostAndPort(host, port);
            if (serverId == Guid.Empty)
            {
                serverId = repository.Add(host, port);
                if (serverId == Guid.Empty)
                {
                    throw new Exception(String.Format("error on init. {0}:{1}", host, port));
                }
            }
        }

        private void reInit()
        {
            init();
        }

        public void Execute()
        {
            try
            {
                HAContext.Current.CommandBus.Send(new PingCommand(Guid.NewGuid(), serverId.ToString(), host, port, -1));

                bool pong = repository.IsConnected(serverId);

                if (pong)
                {
                    logger.Info(string.Format("[Monitor] ping success [{0}:{1}]", host, port));
                    failTimes = 0;
                    if (!repository.IsZKRegistered(serverId))
                    {
                        try
                        {
                            HAContext.Current.CommandBus.Send(new RegisterZkCommand(Guid.NewGuid(), serverId, -1));
                        }
                        catch (Exception e)
                        {
                            logger.Error("[Zk-REG] ERROR", e);
                        }
                    }
                }
                else
                {
                    ++failTimes;
                    if (failTimes >= FAIL_RETRY_TIMES)
                    {
                        logger.Info(string.Format("[Monitor] exceed max-retry times [{0}:{1}]", host, port));
                        try
                        {
                            if (!repository.IsZKRegistered(serverId))
                            {
                                HAContext.Current.CommandBus.Send(new UnRegisterZkCommand(Guid.NewGuid(), serverId, -1));
                            }
                        }
                        catch (Exception e)
                        {
                            logger.Error("[Zk-UnReg] ERROR", e);
                        }
                        logger.Error(string.Format("[Monitor] exceed max-retry times,ping failed [{0}:{1}]", host, port));

                    }
                    logger.Warn(string.Format("[Monitor] ping failed [{0}:{1}]", host, port));
                }

            }
            catch (ServerInstNotExistsException ex)
            {
                logger.Error(string.Format("ServerInstNotExists [{0}:{1}].", host, port), ex);
                failTimes = 0;
                reInit();
            }
            catch (KeeperException ex)
            {
                logger.Error(string.Format("[Monitor]error [{0}:{1}]", host, port), ex);
                ++failTimes;
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("[Monitor]error [{0}:{1}]", host, port), ex);
                ++failTimes;
            }
            finally
            {
            }
        }
    }
}
