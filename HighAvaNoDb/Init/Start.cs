using log4net;
using System;

namespace HighAvaNoDb.Init
{
    public class Start
    {
        ILog log = LogManager.GetLogger(typeof(Start));

        public void App_Start()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            //log.Error
            //AppDomain.CurrentDomain.
        }
    }
}
