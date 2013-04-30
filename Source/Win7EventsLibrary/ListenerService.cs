using System;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using System.Threading;
using System.Data;

namespace Win7EventsLibrary
{
    public partial class ListenerService : ServiceBase
    {
        #region Declaration and Initialize
        string applicationExecutePath=null;
        string Applicationwindow =null;
        string delayapptimeinsec = null;
        Int32 DelayAppLaunchbyNanoSeconds;
        private readonly string _logPath;
        
        EventSubXMLManagement  xmlMgr = new EventSubXMLManagement();

        public ListenerService()
        {
            InitializeComponent();
            Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            ManagedSENS.SensLogon.DisplayLock += SensLogon_DisplayLock;
            ManagedSENS.SensLogon.DisplayUnlock += SensLogon_DisplayUnlock;
            ManagedSENS.SensLogon.Logon += SensLogon_Logon;
            ManagedSENS.SensLogon.Logoff += SensLogon_Logoff;
            ManagedSENS.SensLogon.StartScreenSaver += SensLogon_StartScreenSaver;
            ManagedSENS.SensLogon.StopScreenSaver += SensLogon_StopScreenSaver;
            ManagedSENS.SensLogon.StartShell += SensLogon_StartShell;

            _logPath = System.Configuration.ConfigurationManager.AppSettings["LogFilePath"].ToString();
            //if (System.Configuration.ConfigurationManager.AppSettings["DelaySecondsForAppLaunch"] != null)
            //{
            //    DelayAppLaunchbyNanoSeconds = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["DelaySecondsForAppLaunch"].ToString()) * 1000;

            //}
        }
        #endregion
       
        #region Handle System Events - Core Functionality

        private void SensLogon_Logoff(string userName)
        {
            System.Data.DataRow dr = xmlMgr.GetEventDetails("Logoff");
            if (dr != null)
            {
                delayapptimeinsec = dr["Delaytime"].ToString();
                DelayAppLaunchbyNanoSeconds = Convert.ToInt32(delayapptimeinsec)*1000;
            }
            WriteToLog(String.Format("\nEvent Caught:Logoff, Delay Launch Seconds {0}\n", Convert.ToString(DelayAppLaunchbyNanoSeconds / 1000)));
            Thread.Sleep(DelayAppLaunchbyNanoSeconds);
            WriteToLog(String.Format("Event Caught: Logoff, User:({0})", userName));
            var processEventThread = new Thread(ProcessSensLogOffEvent);
            processEventThread.Start(EventSubXMLManagement.Event.LogOff);
           // processEventThread.Priority = ThreadPriority.Highest;
        }

        private void SensLogon_Logon(string userName)
        {
            System.Data.DataRow dr = xmlMgr.GetEventDetails("Logon");
            if (dr != null)
            {
                delayapptimeinsec = dr["Delaytime"].ToString();
                DelayAppLaunchbyNanoSeconds = Convert.ToInt32(delayapptimeinsec) * 1000;
            }
            WriteToLog(String.Format("\nEvent Caught: Logon, Delay Launch Seconds {0}\n", Convert.ToString(DelayAppLaunchbyNanoSeconds / 1000)));
            Thread.Sleep(DelayAppLaunchbyNanoSeconds);
            
            WriteToLog(String.Format("Event Caught: Logon, User:({0})", userName));
            var processEventThread = new Thread(ProcessSensLogOnEvent);
            processEventThread.Start(EventSubXMLManagement.Event.LogOn);
        }

        private void SensLogon_DisplayUnlock(string userName)
        {
            System.Data.DataRow dr = xmlMgr.GetEventDetails("DisplayUnlock");
            if (dr != null)
            {
                delayapptimeinsec = dr["Delaytime"].ToString();
                DelayAppLaunchbyNanoSeconds = Convert.ToInt32(delayapptimeinsec) * 1000;
            }
            WriteToLog(String.Format("\nEvent Caught: DisplayUnlock, Delay Launch Seconds {0}\n", Convert.ToString(DelayAppLaunchbyNanoSeconds / 1000)));
            Thread.Sleep(DelayAppLaunchbyNanoSeconds);
            WriteToLog(String.Format("Event Caught: DisplayUnlock, User:({0})", userName));
            var processEventThread = new Thread(ProcessSensDisplayUnlockEvent);
            processEventThread.Start(EventSubXMLManagement.Event.DisplayUnlock);
        }

        private void SensLogon_DisplayLock(string userName)
        {
            System.Data.DataRow dr = xmlMgr.GetEventDetails("DisplayLock");
            if (dr != null)
            {
                delayapptimeinsec = dr["Delaytime"].ToString();
                DelayAppLaunchbyNanoSeconds = Convert.ToInt32(delayapptimeinsec) * 1000;
            }
            WriteToLog(String.Format("\nEvent Caught: DisplayLock, Delay Launch Seconds {0}\n", Convert.ToString(DelayAppLaunchbyNanoSeconds / 1000)));
            Thread.Sleep(DelayAppLaunchbyNanoSeconds);
            WriteToLog(String.Format("Event Caught: DisplayLock, User:({0})", userName));
            
            var processEventThread = new Thread(ProcessSensDisplayLockEvent);
            processEventThread.Priority = ThreadPriority.Highest;
            processEventThread.Start(EventSubXMLManagement.Event.DisplayLock);
            
            
        }
        private void SensLogon_StartScreenSaver(String userName)
        {
            System.Data.DataRow dr = xmlMgr.GetEventDetails("StartScreenSaver");
            if (dr != null)
            {
                delayapptimeinsec = dr["Delaytime"].ToString();
                DelayAppLaunchbyNanoSeconds = Convert.ToInt32(delayapptimeinsec) * 1000;
            }
            WriteToLog(String.Format("\nEvent Caught: StartScreenSaver,Delay Launch Seconds {0}\n", Convert.ToString(DelayAppLaunchbyNanoSeconds / 1000)));
            Thread.Sleep(DelayAppLaunchbyNanoSeconds);
            WriteToLog(String.Format("Event Caught: StartScreenSaver, User:({0})", userName));
            var processEventThread = new Thread(ProcessSensStartScreenSaverEvent);
            processEventThread.Start(EventSubXMLManagement.Event.LogOff);
        }
        private void SensLogon_StopScreenSaver(String userName)
        {
            WriteToLog(String.Format("Event Caught: StopScreenSaver, User:({0})", userName));
            var processEventThread = new Thread(ProcessSensStopScreenSaverEvent);
            processEventThread.Start(EventSubXMLManagement.Event.LogOff);
        }
        protected override void OnStart(string[] args)
        {
            System.Data.DataRow dr = xmlMgr.GetEventDetails("servicestart");
            if (dr != null)
            {
                delayapptimeinsec = dr["Delaytime"].ToString();
                DelayAppLaunchbyNanoSeconds = Convert.ToInt32(delayapptimeinsec) * 1000;
            }
            WriteToLog(String.Format("\nEvent Caught: OnStart, Delay Launch Seconds {0}\n",Convert.ToString(DelayAppLaunchbyNanoSeconds/1000)));
            Thread.Sleep(DelayAppLaunchbyNanoSeconds);
            WriteToLog(String.Format("Event Caught: OnStart()"));
            var processEventThread = new Thread(ProcessSensOnStartEvent);
            processEventThread.Start(EventSubXMLManagement.Event.ServiceStart);
           
        }

        protected override void OnStop()
        {
            System.Data.DataRow dr = xmlMgr.GetEventDetails("serviceStop");
            if (dr != null)
            {
                delayapptimeinsec = dr["Delaytime"].ToString();
                DelayAppLaunchbyNanoSeconds = Convert.ToInt32(delayapptimeinsec) * 1000;
            }
            WriteToLog(String.Format("\nEvent Caught: OnStop,Delay Launch Seconds {0}\n", Convert.ToString(DelayAppLaunchbyNanoSeconds / 1000)));
            Thread.Sleep(DelayAppLaunchbyNanoSeconds);
            WriteToLog(String.Format("Event Caught: OnStop()"));
            var processEventThread = new Thread(ProcessSensOnStopEvent);
            processEventThread.Start(EventSubXMLManagement.Event.ServiceStop);
        }
        protected  void SensLogon_StartShell(String userName)
       {
           //Optional
           // WriteToLog(String.Format("SensLogon_StartShell({0})", userName));
           //var processEventThread = new Thread(ProcessSensStartShellEvent);
           //processEventThread.Start(EventSubXMLManagement.Event.StartShell);
           
       }

        #endregion

        #region Update Log File for each event handling activity
        private void WriteToLog(string entry)
        {
            try
            {
                if (true)
                {
                    var sw = new StreamWriter(_logPath, true);
                    sw.WriteLine("{0}\t{1}", DateTime.Now, entry);
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        #endregion

        #region Code For Handling Each Event, Invoking the selected Application, etc
        private void ProcessSensLogOnEvent(object e)
        {
            
            var ip = new ProcessRunner();
            System.Data.DataRow dr = xmlMgr.GetEventDetails("Logon");
            if (dr != null)
            {
                applicationExecutePath = dr["ExecutePath"].ToString();
                Applicationwindow = dr["TargetWindow"].ToString();
                          
            }
            WriteToLog(String.Format("Preparing to Start \t{0} {1}", @Applicationwindow, applicationExecutePath));
            ip.InvokeProcess(@Applicationwindow, @applicationExecutePath);
            
        }

        private void ProcessSensLogOffEvent(object e)
        {
                //Thread.Sleep(DelayAppLaunchbyNanoSeconds);
                var ip = new ProcessRunner();
                System.Data.DataRow dr = xmlMgr.GetEventDetails("Logoff");
                if (dr != null)
                {
                    applicationExecutePath=dr["ExecutePath"].ToString();
                    Applicationwindow=dr["TargetWindow"].ToString();
                    
                }
               
                WriteToLog(String.Format("Preparing to Start \t{0} {1}",Applicationwindow, applicationExecutePath));
                ip.InvokeProcess(@Applicationwindow,@applicationExecutePath);
            
        }
        
        private void ProcessSensDisplayLockEvent(object e)
        {
            var ip = new ProcessRunner();
            Thread.Sleep(DelayAppLaunchbyNanoSeconds);
            System.Data.DataRow dr = xmlMgr.GetEventDetails("Displaylock");
            if (dr != null)
            {
                applicationExecutePath = dr["ExecutePath"].ToString();
                Applicationwindow = dr["TargetWindow"].ToString();
              
                
            }
            WriteToLog(String.Format("Preparing to Start \t{0} {1}", Applicationwindow, applicationExecutePath));
            ip.InvokeProcess(@Applicationwindow, @applicationExecutePath);

        }
        private void ProcessSensDisplayUnlockEvent(object e)
        {
            var ip = new ProcessRunner();
            System.Data.DataRow dr = xmlMgr.GetEventDetails("DisplayUnlock");
            if (dr != null)
            {
                applicationExecutePath = dr["ExecutePath"].ToString();
                Applicationwindow = dr["TargetWindow"].ToString();
            }
         
            WriteToLog(String.Format("Preparing to Start \t{0} {1}", Applicationwindow, applicationExecutePath));
            ip.InvokeProcess(@Applicationwindow, @applicationExecutePath);
            
        }
         
         private void ProcessSensStartScreenSaverEvent(object e)
        {//Optional
        }
         private void ProcessSensStopScreenSaverEvent(object e)
         {
             var ip = new ProcessRunner();

             System.Data.DataRow dr = xmlMgr.GetEventDetails("ScreenSaverStop");
             if (dr != null)
             {
                 applicationExecutePath = dr["ExecutePath"].ToString();
                 Applicationwindow = dr["TargetWindow"].ToString();
             }

             WriteToLog(String.Format("Preparing to Start \t{0} {1}", Applicationwindow, applicationExecutePath));
             ip.InvokeProcess(@Applicationwindow, @applicationExecutePath);
         }

         private void ProcessSensOnStartEvent(object e)
         {

             //Thread.Sleep(DelayAppLaunchbyNanoSeconds);
             var ip = new ProcessRunner();
             System.Data.DataRow dr = xmlMgr.GetEventDetails("servicestart");
             if (dr != null)
             {
                 applicationExecutePath = dr["ExecutePath"].ToString();
                 Applicationwindow = dr["TargetWindow"].ToString();

             }
             
             WriteToLog(String.Format("Preparing to Start \t{0} {1}", Applicationwindow, applicationExecutePath));
             ip.InvokeProcess(@Applicationwindow, @applicationExecutePath);

         }
         private void ProcessSensOnStopEvent(object e)
        {

            var ip = new ProcessRunner();
            System.Data.DataRow dr = xmlMgr.GetEventDetails("serviceStop");
            if (dr != null)
            {
                applicationExecutePath = dr["ExecutePath"].ToString();
                Applicationwindow = dr["TargetWindow"].ToString();

            }
            WriteToLog(String.Format("Preparing to Start \t{0} {1}", Applicationwindow, applicationExecutePath));
            ip.InvokeProcess(@Applicationwindow, @applicationExecutePath);
            
        }
         private void ProcessSensStartShellEvent(object e)
         {

             var ip = new ProcessRunner();
             System.Data.DataRow dr = xmlMgr.GetEventDetails("startshell");
             if (dr != null)
             {
                 applicationExecutePath = dr["ExecutePath"].ToString();
                 Applicationwindow = dr["TargetWindow"].ToString();

                 //invoke sequence
             }
               WriteToLog(String.Format("Preparing to Start \t{0} {1}", Applicationwindow, applicationExecutePath));
             ip.InvokeProcess(@Applicationwindow, @applicationExecutePath);

         }
        #endregion
    }
}
