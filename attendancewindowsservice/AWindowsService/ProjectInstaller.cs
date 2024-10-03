using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Configuration.Install;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace AWindowsService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            //InitializeComponent();

            var config = ConfigurationManager.OpenExeConfiguration(Assembly.GetAssembly(typeof(ProjectInstaller)).Location);

            var process = new ServiceProcessInstaller { Account = ServiceAccount.LocalSystem };
            var serviceAdmin = new ServiceInstaller
            {
                StartType = ServiceStartMode.Automatic,
                ServiceName = config.AppSettings.Settings["WindowServiceName"].Value,
                //DisplayName = 
            };
            Installers.Add(process);
            Installers.Add(serviceAdmin);
            serviceAdmin.AfterInstall += serviceProcessInstaller1_AfterInstall;
        }
       
        private void serviceProcessInstaller1_AfterInstall(object sender, InstallEventArgs e)
        {
            ServiceInstaller serviceInstaller = (ServiceInstaller)sender;
            using (ServiceController sc = new ServiceController(serviceInstaller.ServiceName))
            {
                sc.Start();
            }
        }
    }
}
