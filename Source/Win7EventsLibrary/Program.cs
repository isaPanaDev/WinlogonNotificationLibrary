﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceProcess;
namespace Win7EventsLibrary
{
    class Program
    {
        static void Main(string[] args)
        {

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new ListenerService() 
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
