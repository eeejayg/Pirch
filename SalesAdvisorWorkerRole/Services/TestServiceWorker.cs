using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

using SalesAdvisorSharedClasses.Communication;
using SalesAdvisorSharedClasses.Models;
using SalesAdvisorWorkerRole.Adapters;
using SalesAdvisorWorkerRole.Logging;
using SalesAdvisorWorkerRole.MessageHandlers;
using SalesAdvisorWorkerRole.Services;

namespace SalesAdvisorWorkerRole.Services
{
    [ServiceBehavior(
        InstanceContextMode = InstanceContextMode.Single
        )]
    class TestServiceWorker : TestService
    {
        public void TestCall()
        {
            try
            {
                CustomerServiceWorker csw = new CustomerServiceWorker();
                Customers customer = csw.CustomerGetById(50);
                customer.CustomerGuid = Guid.NewGuid().ToString();
                //FLJsonAdapter.getInstance().CreateCustomer(customer);
            }
            catch (Exception e)
            {
                DebugLog.Log(String.Format("Something went wrong in the test service, e.message: {0}", e.Message));
                throw e;
            }
        }
    }
}
