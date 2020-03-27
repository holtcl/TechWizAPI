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
        [HttpGet]
        [Route("api/WizardJob")]
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

        [Authorize]
        [HttpPost]
        [Route("api/WizardJob/{requestId}")]
        // POST: api/WizardJob/requestid/
        public HttpResponseMessage Post(long requestId, [FromBody]FormUrlEncodedContent content)
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
                    return Request.CreateResponse(HttpStatusCode.OK, rp.acceptRequestAsWizard(requestId, user));
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized);
                }
            }
        }

        [Authorize]
        [HttpPut]
        [Route("api/WizardJob/{requestId}/{hours}")]
        // PUT: api/WizardJob/requestid/hoursworked
        // sets the number of hours worked for the job
        public HttpResponseMessage Put(long requestId, int hours, [FromBody]Request value)
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
                    int user = (int)long.Parse(identity.FindFirst("UserID").Value);
                    RequestPersistence rp = new RequestPersistence();
                    bool returnObj = rp.submitHoursWorkedAsWizard(user, (int) requestId, hours);
                    if (returnObj)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, returnObj);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest);
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized);
                }
            }
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
