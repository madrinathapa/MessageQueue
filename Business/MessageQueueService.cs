using System.Configuration;
using System.ServiceProcess;
using System.Threading;
using MessageQueue;

namespace MessageQueueService.Business
{
    class Service : ServiceBase
    {
        protected Thread m_thread;
        public Service()
        {
            ServiceName = ConfigurationManager.AppSettings.Get("ServiceName");
        }
        protected void ServiceMain()
        {
            while (true)
            {
                Program.ProcessRequest();
            }
        }

        protected override void OnStart(string[] args)
        {
            Program.Start(args);
            
            //Create a worker thread
            ThreadStart ts = new ThreadStart(this.ServiceMain);
            m_thread = new Thread(ts);
            m_thread.Start();
        }

        protected override void OnStop()
        {
            Program.Stop();
        }
    }
}
