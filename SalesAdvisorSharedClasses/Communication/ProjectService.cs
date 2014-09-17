using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

using SalesAdvisorSharedClasses.Models;

namespace SalesAdvisorSharedClasses.Communication
{
    public class ProjectServiceInfo
    {
        public static readonly String ENDPOINT_NAME = "net.tcp://{0}/ProjectService";
    }

    [ServiceContract]
    public interface ProjectService
    {

        [OperationContract]
        int ProjectAttachCustomer(int CustomerId, int ProjectId);

        [OperationContract]
        Project ProjectCollectFirstIfExists(int CustomerId, int ProjectId);


        [OperationContract]
        int ProjectCreateFromProject(Project project, int StatusId);

        [OperationContract]
        int ProjectCreateNew(Project project);

        [OperationContract]
        List<Project> ProjectFindChildren(int ProjectId, int StatusId);

        [OperationContract]
        bool ProjectDeleteFromUsers(int ProjectId);


    }
}
