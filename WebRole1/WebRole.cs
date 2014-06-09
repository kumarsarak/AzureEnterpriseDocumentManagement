using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace WebRole1
{
    public class WebRole : RoleEntryPoint
    {
        public override bool OnStart()
        {
            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
            // ======  Added =======
            /*
            TimeSpan tsOneMinute = TimeSpan.FromMinutes(1);

            DiagnosticMonitorConfiguration dmc =

            DiagnosticMonitor.GetDefaultInitialConfiguration();

            // Transfer logs to storage every minute

            dmc.Logs.ScheduledTransferPeriod = tsOneMinute;

            // Transfer verbose, critical, etc. logs

            dmc.Logs.ScheduledTransferLogLevelFilter = LogLevel.Verbose;

            // Start up the diagnostic manager with the given configuration

            DiagnosticMonitor.Start("Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString", dmc);
             */
            //===============

            return base.OnStart();
        }
    }
}
