using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAdvisorWorkerRole.Logging
{
    /*
     * The intent here is to enable logging in a more robust way in the future, when we have time for it.
     */
    class DebugLog
    {
        public static void Log(String message)
        {
            Debug.WriteLine(message);
        }
    }
}
