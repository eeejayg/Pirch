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
    public class UpdateUsersController : BaseApiController
    {
        // GET api/update
        /*
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }
        */
        // PUT api/updateuser
        public Dictionary<String, String> Post(Dictionary<String, dynamic> dict)
        {
            Dictionary<String, String> result = new Dictionary<String, String>();
            try
            {
                UpdateUserMessage msg = new UpdateUserMessage();
                msg.guid = dict["guid"];
                msg.EnqueueSelfAsMessage(BaseApiController.protectedQueue);
                result["success"] = "true";
            }
            catch (Exception e)
            {
                result["success"] = "false";
                // We should be logging these
            }
            return result;
        }

        // PUT api/values/5
        public Dictionary<String, Boolean> Put([FromBody]string guid)
        {
            Dictionary<String, Boolean> result = this.GetSimpleSuccessResponse();
            try
            {
                UpdateUserMessage msg = new UpdateUserMessage();
                msg.guid = guid;
                msg.EnqueueSelfAsMessage(BaseApiController.protectedQueue);
            }
            catch (Exception e)
            {
                result["success"] = false;
                // we should be logging these
            }
            return result;
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}