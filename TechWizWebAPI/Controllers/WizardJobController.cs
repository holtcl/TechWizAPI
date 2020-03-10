using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TechWizWebAPI.Models;
using System.Collections;
using System.Security.Claims;

namespace TechWizWebAPI.Controllers
{
    public class WizardJobController : ApiController
    {
        [Authorize]
        // GET: api/WizardJob
        public HttpResponseMessage Get()
        {
            var identity = User.Identity as ClaimsIdentity;
            if (identity == null)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            else
            {
                if (bool.Parse(identity.FindFirst("isWizard").Value))
                {
                    int user = (int)Int64.Parse(identity.FindFirst("UserID").Value);
                    RequestPersistence rp = new RequestPersistence();
                    ArrayList returnObj = rp.getRequestsForDisplayForWizardId(user);
                    return Request.CreateResponse(HttpStatusCode.OK, returnObj);
                }
                else {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized);
                }
            }
        }
        // GET: api/WizardJob/5
        public Request Get(long id)
        {
            RequestPersistence rp = new RequestPersistence();
            Request request = rp.getRequest(id);
            return request;
        }

        [Authorize]
        // POST: api/WizardJob/requestid/
        // This accepts the job
        public HttpResponseMessage Post(long requestid, [FromBody]Request value)
        {
            var identity = User.Identity as ClaimsIdentity;
            if (identity == null)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            else
            {
                if (bool.Parse(identity.FindFirst("isWizard").Value))
                {
                    int user = (int)Int64.Parse(identity.FindFirst("UserID").Value);
                    RequestPersistence rp = new RequestPersistence();
                    return Request.CreateResponse(HttpStatusCode.OK, rp.acceptRequestAsWizard(requestid, user));
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized);
                }
            }
        }

        // PUT: api/WizardJob/5
        public HttpResponseMessage Put(long id, [FromBody]Request value)
        {
            RequestPersistence rp = new RequestPersistence();
            bool recordExisted = false;
            recordExisted = rp.updateRequest(id, value);

            HttpResponseMessage response;
            if (recordExisted)
            {
                response = Request.CreateResponse(HttpStatusCode.NoContent);
            }
            else
            {
                response = Request.CreateResponse(HttpStatusCode.NotFound);
            }
            return response;
        }



        // DELETE: api/WizardJob/5
        public HttpResponseMessage Delete(long id)
        {
            RequestPersistence rp = new RequestPersistence();
            bool recordExisted = false;
            recordExisted = rp.deleteRequest(id);

            HttpResponseMessage response;
            if (recordExisted)
            {
                response = Request.CreateResponse(HttpStatusCode.NoContent);
            }
            else
            {
                response = Request.CreateResponse(HttpStatusCode.NotFound);
            }
            return response;
        }
    }
}
