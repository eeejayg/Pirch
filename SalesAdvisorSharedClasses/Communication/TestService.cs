using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

using SalesAdvisorSharedClasses.Models;

namespace SalesAdvisorSharedClasses.Communication
{
    public class TestServiceInfo
    {
        public static readonly String ENDPOINT_NAME = "net.tcp://{0}/TestService";
    }

    [ServiceContract]
    public interface TestService
    {
        [OperationContract]
        void TestCall();
    }
}
