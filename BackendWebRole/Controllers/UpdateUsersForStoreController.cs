using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using BackendWebRole;
using SalesAdvisorQueueUtils.MessageHandlers;

namespace BackendWebRole.Controllers
{
    public class UpdateUsersForStoreController : BaseApiController
    {
        // PUT api/values/5
        public Dictionary<String, Boolean> Put(Dictionary<String, dynamic> dict)
        {
            Dictionary<String, Boolean> result = this.GetSimpleSuccessResponse();
            try
            {
                UpdateUsersForStoreMessage msg = new UpdateUsersForStoreMessage();
                msg.storeid = dict["storeid"];
                msg.EnqueueSelfAsMessage(BaseApiController.protectedQueue);
            }
            catch (Exception e)
            {
                result["success"] = false;
                throw e;
            }
            return result;
        }
    }
}
