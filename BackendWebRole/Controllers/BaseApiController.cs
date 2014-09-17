using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

using BackendWebRole.Filters;
using SalesAdvisorQueueUtils.Queue;

namespace BackendWebRole.Controllers
{
    /*
     * The entire application needs to be locked down several ways, so we need all controllers to inherit from this
     * base class to enforce security requirements.
     */
    [AuthorizeIPAddress]
    [RequireHttps]
    public class BaseApiController : ApiController
    {
        public static QueueHandler protectedQueue;

        /// <summary>
        /// Returns a simple dictionary with a key called success set to true
        /// </summary>
        /// <returns></returns>
        protected Dictionary<String, Boolean> GetSimpleSuccessResponse()
        {
            Dictionary<String, Boolean> response = new Dictionary<String, Boolean>();
            response["success"] = true;
            return response;
        }
    }
}
