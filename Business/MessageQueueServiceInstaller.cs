using System;
using System.ComponentModel;
using System.Configuration;
using System.Configuration.Install;
using System.Reflection;
using System.ServiceProcess;

namespace MessageQueue.Business
{
    [RunInstaller(true)]
    public class MessageQueueServiceInstaller : Installer
    {
        public MessageQueueServiceInstaller()
        {
            var spi = new ServiceProcessInstaller();
            var si = new ServiceInstaller();

            spi.Account = ServiceAccount.LocalSystem;
            spi.Username = null;
            spi.Password = null;
            si.DisplayName = GetConfigurationValue("ServiceName");
            si.ServiceName = GetConfigurationValue("ServiceName");
            si.StartType = ServiceStartMode.Automatic;

            Installers.Add(spi);
            Installers.Add(si);
        }

        private string GetConfigurationValue(string key)
        {
            Assembly service = Assembly.GetAssembly(typeof(MessageQueueServiceInstaller));
            Configuration config = ConfigurationManager.OpenExeConfiguration(service.Location);
            if (config.AppSettings.Settings[key] != null)
            {
                return config.AppSettings.Settings[key].Value;
            }
            throw new IndexOutOfRangeException("Settings collection does not contain the requested key: " + key);
        }
    }
}
