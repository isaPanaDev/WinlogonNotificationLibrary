using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;

namespace Win7EventsLibrary
{
    /// <summary>
    /// Project Installer for Service
    /// Installs the service under Windows Services and Starts it automatically after installation
    /// 
    /// </summary>
    
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }

        //Automatically Start the service with Local System Account after installing it.
        private void serviceInstaller1_AfterInstall(object sender, InstallEventArgs e)
        {
            ServiceController servCon = new ServiceController(this.serviceInstaller1.ServiceName, Environment.MachineName);
            servCon.Start();
        }

        private void serviceProcessInstaller1_AfterInstall(object sender, InstallEventArgs e)
        {

        }
    }
}
