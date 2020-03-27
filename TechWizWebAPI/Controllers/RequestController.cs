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
    public class RequestController : ApiController
    {
        [Authorize]
        // GET: api/Request
        public ArrayList Get()
        {
            var identity = User.Identity as ClaimsIdentity;
            if (identity == null)
            {
                return null;
            }
            else
            {
                int user = (int)Int64.Parse(identity.FindFirst("UserID").Value);
                RequestPersistence rp = new RequestPersistence();
                return rp.getRequestsForDisplayForUserId(user);
            }
        }

        [Authorize]
        // POST: api/Request
        public HttpResponseMessage Post([FromBody]Request value)
        {
            var identity = User.Identity as ClaimsIdentity;
            if (identity == null)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            else
            {
                value.user = (int)Int64.Parse(identity.FindFirst("UserID").Value);
                RequestPersistence rp = new RequestPersistence();
                long id;
                id = rp.createNewRequest(value);
                value.requestID = id;
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);
                response.Headers.Location = new Uri(Request.RequestUri, String.Format("request/{0}", id));
                return response;
            }
        }

        [Authorize]
        [HttpPut]
        [Route("api/Request/{requestId}")]
        // PUT: api/Request/5
        // for accepting a request
        public HttpResponseMessage Put(long requestId, [FromBody]Request value)
        {
            var identity = User.Identity as ClaimsIdentity;
            if (identity == null)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            else
            {
                int user = (int)Int64.Parse(identity.FindFirst("UserID").Value);
                RequestPersistence rp = new RequestPersistence();
                bool returnObj;
                returnObj = rp.completeJob(user, (int)requestId);
                if (returnObj)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, returnObj);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
            }
        }



        // DELETE: api/Request/5
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
