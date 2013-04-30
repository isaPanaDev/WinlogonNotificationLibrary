using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Configuration;
using System.Data;
namespace Win7EventsLibrary
{
    internal class EventSubXMLManagement
    {
        private readonly XmlDocument _logonAppsXmlDoc;
        public DataSet ds = new DataSet();

        public enum Event
        {
            ServiceStart,
            ServiceStop,
            DisplayLock,
            DisplayUnlock,
            LogOn,
            LogOff,
            StartShell
        }


        #region Parse XML and Retrieve Details

        public DataRow GetEventDetails(string eventname)
        {

            ds.Clear();
            string xmlPath1 = System.Configuration.ConfigurationManager.AppSettings["EventSubscriptionXMLPath"].ToString();
            ds.ReadXml(xmlPath1, XmlReadMode.ReadSchema);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (eventname.ToUpper() == dr["EventName"].ToString().ToUpper())
                {
                    return dr;
                }
            }
            return null;
        }
        public string GetApplicationWindow(string eventname)
        {
            DataRow dr1 = GetEventDetails(eventname);
            if (dr1 != null)
            {
                return (dr1["TargetWindow"].ToString());
            }
            return null;
        }
        public string GetApplicationsPath(string eventname)
        {
            DataRow dr1 = GetEventDetails(eventname);
            if (dr1 != null)
            {
                return (dr1["ExecutePath"].ToString());
            }
            return null;
        }
        public string GetApplicationsdelaytime(string eventname)
        {
            DataRow dr1 = GetEventDetails(eventname);
            if (dr1 != null)
            {
                return (dr1["Delaytime"].ToString());
            }
            return null;


        }

        private string GetXmlAttributeNameForEvent(Event e)
        {
            switch (e)
            {
                case Event.DisplayLock:
                    return "display_lock";
                case Event.DisplayUnlock:
                    return "display_unlock";
                case Event.LogOff:
                    return "logoff";
                case Event.LogOn:
                    return "logon";
                case Event.ServiceStart:
                    return "service_start";
                case Event.ServiceStop:
                    return "service_stop";
                case Event.StartShell:
                    return "StartShell";
                default:
                    return null;
            }
        }
        #endregion
    }
}
