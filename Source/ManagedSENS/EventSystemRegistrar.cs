﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using EventSystemLib;
using SensEvents;

namespace ManagedSENS
{
    [ComImport, Guid("4E14FBA2-2E22-11D1-9964-00C04FBBB345")]
    class EventSystem { }

    [ComImport, Guid("7542E960-79C7-11D1-88F9-0080C7D771BF")]
    class EventSubcription { }

    [ComImport, Guid("AB944620-79C6-11d1-88F9-0080C7D771BF")]
    class EventPublisher { }

    [ComImport, Guid("cdbec9c0-7a68-11d1-88f9-0080c7d771bf")]
    class EventClass { }

    class EventSystemRegistrar
    {
        private const string ProgIdEventSubscription = "EventSystem.EventSubscription";
        static EventSystemRegistrar() { }

        private static IEventSystem es = null;
        private static IEventSystem EventSystem
        {
            get
            {
                if (es == null)
                    es = new EventSystem() as IEventSystem;
                return es;
            }
        }

        public static void SubscribeToEvents(string description, string subscriptionName, string
         subscriptionID, object subscribingObject, Type subscribingType)
        {
            // activate subscriber
            try
            {
                //create and populate a subscription object
                IEventSubscription sub = new EventSubcription() as IEventSubscription;
                sub.Description = description;
                sub.SubscriptionName = subscriptionName;
                sub.SubscriptionID = subscriptionID;
                //Get the GUID from the ISensLogon interface
                sub.InterfaceID = GetInterfaceGuid(subscribingType);
                sub.SubscriberInterface = subscribingObject;
                //Store the actual Event.
                EventSystem.Store(ProgIdEventSubscription, sub);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static string GetInterfaceGuid(Type theType)
        {
            object[] attributes = theType.GetCustomAttributes(typeof(GuidAttribute), true);
            if (attributes.Length > 0)
            {
                return "{" + ((GuidAttribute)attributes[0]).Value + "}";
            }
            else
            {
                throw new ArgumentException("GuidAttribute not present on the Type.", "theType");
            }
        }

        public static void UnsubscribeToEvents(string subscriptionID)
        {
            try
            {
                string strCriteria = "SubscriptionID == " + subscriptionID;
                int errorIndex = 0;
                EventSystem.Remove("EventSystem.EventSubscription", strCriteria, out errorIndex);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }




    }

    public delegate void SensLogonEventHandler(string userName);

    public class SensLogon
    {
        private static SensLogonInterop _eventCatcher;
        static SensLogon() { }

        //...SensLogonInterop goes here

        #region Event Registration Code

        private static int _registerCount = 0;
        private static bool IsRegistered
        {
            get
            {
                return (_registerCount > 0);
            }
        }

        private static SensLogonEventHandler RegisterEvent(SensLogonEventHandler original,
         SensLogonEventHandler newDel)
        {
            bool shouldRegister = (original == null);
            original = original + newDel;
            if (shouldRegister)
            {
                if (_registerCount <= 0)
                {
                    if (SensLogon._eventCatcher == null)
                        SensLogon._eventCatcher = new SensLogonInterop();
                    _registerCount = 1;
                }
                else
                {
                    //Just count them.
                    _registerCount++;
                }
            }
            return original;
        }

        private static SensLogonEventHandler UnregisterEvent(SensLogonEventHandler original,
         SensLogonEventHandler oldDel)
        {
            original = original - oldDel;
            if (original == null)
            {
                _registerCount--;
                if (_registerCount == 0)
                {
                    //unregister for those events.
                    SensLogon._eventCatcher.Dispose();
                    SensLogon._eventCatcher = null;
                }
            }
            return original;
        }

        #endregion

        #region ISensLogon Event Raising Members

        protected static void OnDisplayLock(string bstrUserName)
        {
            if (SensLogon.displayLock != null)
                SensLogon.displayLock(bstrUserName);
        }
        protected static void OnDisplayUnlock(string bstrUserName)
        {
            if (SensLogon.displayUnlock != null)
                SensLogon.displayUnlock(bstrUserName);
        }
        protected static void OnLogon(string bstrUserName)
        {
            if (SensLogon.logon != null)
                SensLogon.logon(bstrUserName);
        }
        protected static void OnLogoff(string bstrUserName)
        {
            if (SensLogon.logoff != null)
                SensLogon.logoff(bstrUserName);
        }
        protected static void OnStartShell(string bstrUserName)
        {
            if (SensLogon.startShell != null)
                SensLogon.startShell(bstrUserName);
        }
        protected static void OnStartScreenSaver(String bstrUserName)
        {
            if (SensLogon.startScreenSaver != null)
                SensLogon.startScreenSaver(bstrUserName);
        }

        protected static void OnStopScreenSaver(String bstrUserName)
        {
            if (SensLogon.stopScreenSaver != null)
                SensLogon.stopScreenSaver(bstrUserName);
        }

        ///...

        #endregion

        #region Event Declarations

        private static SensLogonEventHandler displayLock = null;
        private static SensLogonEventHandler displayUnlock = null;
        private static SensLogonEventHandler logon = null;
        private static SensLogonEventHandler logoff = null;
        private static SensLogonEventHandler startScreenSaver = null;
        private static SensLogonEventHandler stopScreenSaver = null;
        private static SensLogonEventHandler startShell = null;

        ///...

        public static event SensLogonEventHandler DisplayLock
        {
            add
            {
                SensLogon.displayLock = SensLogon.RegisterEvent(SensLogon.displayLock, value);
            }
            remove
            {
                SensLogon.displayLock = SensLogon.UnregisterEvent(SensLogon.displayLock, value);
            }
        }
        public static event SensLogonEventHandler DisplayUnlock
        {
            add
            {
                SensLogon.displayUnlock = SensLogon.RegisterEvent(SensLogon.displayUnlock, value);
            }
            remove
            {
                SensLogon.displayUnlock = SensLogon.UnregisterEvent(SensLogon.displayUnlock, value);
            }
        }
        public static event SensLogonEventHandler Logon
        {
            add
            {
                SensLogon.logon = SensLogon.RegisterEvent(SensLogon.logon, value);
            }
            remove
            {
                SensLogon.logon = SensLogon.UnregisterEvent(SensLogon.logon, value);
            }
        }
        public static event SensLogonEventHandler Logoff
        {
            add
            {
                SensLogon.logoff = SensLogon.RegisterEvent(SensLogon.logoff, value);
            }
            remove
            {
                SensLogon.logoff = SensLogon.UnregisterEvent(SensLogon.logoff, value);
            }
        }
        public static event SensLogonEventHandler StartScreenSaver
        {
            add
            {
                SensLogon.startScreenSaver = SensLogon.RegisterEvent(startScreenSaver, value);
                // startScreenSaver = RegisterEvent(startScreenSaver, value);
            }

            remove
            {
                SensLogon.startScreenSaver = SensLogon.UnregisterEvent(startScreenSaver, value);
            }
        }

        public static event SensLogonEventHandler StopScreenSaver
        {
            add
            {
                SensLogon.stopScreenSaver = SensLogon.RegisterEvent(stopScreenSaver, value);
            }

            remove
            {
                SensLogon.stopScreenSaver = SensLogon.UnregisterEvent(stopScreenSaver, value);
            }
        }

        public static event SensLogonEventHandler StartShell
        {
            add
            {

                SensLogon.startShell = SensLogon.RegisterEvent(startShell, value);
            }

            remove
            {
                SensLogon.startShell = SensLogon.UnregisterEvent(startShell, value);
            }
        }
        ///...
        ///
        #endregion

        private class SensLogonInterop : ISensLogon, IDisposable
        {
            private const string SubscriptionViewerName = "ManagedSENS.SensLogonInterop";
            private static string SubscriptionViewerID = "{" +
             typeof(SensLogonInterop).GUID.ToString().ToUpper() + "}"; // generate a subscriptionID 
            private const string SubscriptionViewerDesc = "ManagedSENS Event Subscriber";

            private bool registered;

            public SensLogonInterop()
            {
                registered = false;
                EventSystemRegistrar.SubscribeToEvents(SubscriptionViewerDesc, SubscriptionViewerName,
                 SubscriptionViewerID, this, typeof(ISensLogon));
                registered = true;
            }

            #region Cleanup Code

            ~SensLogonInterop()
            {
                this.Dispose(false);
            }

            public void Dispose()
            {
                this.Dispose(true);
            }

            protected void Dispose(bool isExplicit)
            {
                this.Deactivate();
            }

            private void Deactivate()
            {
                if (registered)
                {
                    EventSystemRegistrar.UnsubscribeToEvents(SubscriptionViewerID);
                    registered = false;
                }
            }

            #endregion

            #region ISensLogon Members

            public void DisplayLock(string bstrUserName)
            {
                SensLogon.OnDisplayLock(bstrUserName);
            }
            public void DisplayUnlock(string bstrUserName)
            {
                SensLogon.OnDisplayUnlock(bstrUserName);
            }
            public void Logon(string bstrUserName)
            {
                SensLogon.OnLogon(bstrUserName);
            }
            public void Logoff(string bstrUserName)
            {
                SensLogon.OnLogoff(bstrUserName);
            }
            public void StartShell(string bstrUserName)
            {
                SensLogon.OnStartShell(bstrUserName);
            }
            public void StartScreenSaver(string bstrUserName)
            {
                SensLogon.OnStartScreenSaver(bstrUserName);
            }
            public void StopScreenSaver(string bstrUserName)
            {
                SensLogon.OnStopScreenSaver(bstrUserName);
            }
            //...More ISensLogon memmbers
            #endregion
        }
    }
}
