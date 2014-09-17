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

using Dapper;

namespace SalesAdvisorWorkerRole.Services
{
    [ServiceBehavior(
        InstanceContextMode = InstanceContextMode.Single
        )]
    class ProjectServiceWorker : ProjectService
    {
        private SqlConnection getConnection()
        {
            return DBAdapter.getInstance().getConnection();
        }


        /// <summary>
        /// Creates a record attaching a project and a customer
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public int ProjectAttachCustomer(int CustomerId, int ProjectId){
             using (var connection = this.getConnection()){
                  DynamicParameters DynamicParameters = new DynamicParameters();
                  DynamicParameters.Add("@FKCustomer", CustomerId);
                  DynamicParameters.Add("@FKProject", ProjectId);
                  DynamicParameters.Add("@newId", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
                  connection.Execute("ProjectToCustomers_Create", DynamicParameters, commandType: CommandType.StoredProcedure);
                  return DynamicParameters.Get<int>("newId");
               }
        }


        /// <summary>
        /// This checks a customer for a project of a certain status.  It either returns
        /// the first we find or null
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <param name="StatusId"></param>
        /// <returns></returns>
        public Project ProjectCollectFirstIfExists(int CustomerId, int StatusId)
        {
            //
            //  Start here, make the query and return project or null
            //
            using (var connection = this.getConnection())
            {
                try
                {
                    Project ProjectThatMayExist = connection.Query<Project>("Project_CollectFirstByStatus", new { StatusId = StatusId, CustomerId = CustomerId }, commandType: CommandType.StoredProcedure).First();
                    return ProjectThatMayExist;
                }
                catch (InvalidOperationException e)
                {
                    return null;
                }
            }
        }




        /// <summary>
        ///  Feed this an existing project ID and we create the whole quote.
        /// </summary>
        /// <param name="project">Project ID to Turn into a Quote</param>
        /// <returns></returns>
        ///  Here we copy an entire project into a quote.  To do so we need to
        ///  1.  Copy the project row itself, setting a new FKParent and updating the ProjectStatus
        ///  2.  Copy all the rooms, setting new FKParentRoom and new FKProject
        ///  3.  Copy all the ProductInstances, setting a new FKParentInstance and FKRoom
        public int ProjectCreateFromProject(Project project,  int statusId)
        {
            using (var connection = this.getConnection())
            {
                DynamicParameters DynamicParameters = new DynamicParameters();
                DynamicParameters.Add("@OldProjectId", project.FKParentProject);
                DynamicParameters.Add("@StatusId", statusId);  //Hard coded to the quote status
                DynamicParameters.Add("@NewQuoteId", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
                connection.Execute("Quote_CreateFromProject", DynamicParameters, commandType: CommandType.StoredProcedure);
                return DynamicParameters.Get<int>("NewQuoteId");
            }
        }


        /// <summary>
        /// Creates a brand new project according to the incoming project
        /// </summary>
        /// <returns></returns>
        public int ProjectCreateNew(Project project)
        {
            using (var connection = this.getConnection())
            {
                DynamicParameters DynamicParameters = new DynamicParameters();
                DynamicParameters.Add("@Name", project.Name);
                //  TODO - These date objects get created in C# as 1//011001.  This makes the DB puke.  
                //  I'm kicking the can on this for now -BMW
                DynamicParameters.Add("@Deadline", null);  //Hard coded to the quote status
                DynamicParameters.Add("@Created", null);
                DynamicParameters.Add("@Sunset", null);  //Hard coded to the quote status
                DynamicParameters.Add("@FKParentProject", project.FKParentProject);
                DynamicParameters.Add("@FKProjectType", project.FKProjectType);  //Hard coded to the quote status
                DynamicParameters.Add("@FKStatus", project.FKStatus);
                DynamicParameters.Add("@newid", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
                connection.Execute("Project_Create", DynamicParameters, commandType: CommandType.StoredProcedure);
                return DynamicParameters.Get<int>("newid");
            }
        }


        /// <summary>
        /// Deletes all customer => project connections
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public bool ProjectDeleteFromUsers(int ProjectId)
        {
            using (var connection = this.getConnection())
            {
                DynamicParameters ParamsForProc = new DynamicParameters();
                ParamsForProc.Add("@ProjectId", ProjectId);
                connection.Execute("ProjectToCustomers_DeleteByProjectId", ParamsForProc, commandType: CommandType.StoredProcedure);
                return true;
            }
        }

        /// <summary>
        /// Returns all the children of the project id of type status
        /// </summary>
        /// <returns></returns>
        public List<Project> ProjectFindChildren(int ProjectId, int StatusId)
        {
            using (var connection = this.getConnection())
            {
                // TODO - This function only collects direct descendents, which is fine for beta. 
                // later, it will have to collect all children from up the inheritance chain.  This should
                // be nothing more than a modification of the sproc. -BMW
                List<Project> projects = connection.Query<Project>("Project_CollectChildrenByStatus", new { ProjectId = ProjectId, StatusId = StatusId}, commandType: CommandType.StoredProcedure).ToList();
                return projects;
            }
        }

    }
}
