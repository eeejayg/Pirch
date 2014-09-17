using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace SalesAdvisorWebRole
{
    public class WebRole : RoleEntryPoint
    {
        public override bool OnStart()
        {
            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
            //foreach (var appRole in RoleEnvironment.Roles) {
                //var instances = RoleEnvironment.Roles["SalesAdvisorWorkerRole"].Instances;
            //}
            return base.OnStart();
        }
    }
}
