using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TechWizWebAPI.Models;
using System.Collections;

namespace TechWizWebAPI.Controllers
{
    public class RequestController : ApiController
    {
        // GET: api/Request
        public ArrayList Get()
        {
            RequestPersistence rp = new RequestPersistence();
            return rp.getRequests();
        }

        // GET: api/Request/5
        public Request Get(long id)
        {
            RequestPersistence rp = new RequestPersistence();
            Request request = rp.getRequest(id);
            return request;
        }

        // POST: api/Request
        public HttpResponseMessage Post([FromBody]Request value)
        {
            RequestPersistence rp = new RequestPersistence();
            long id;
            id = rp.saveRequest(value);
            value.requestID = id;
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);
            response.Headers.Location = new Uri(Request.RequestUri, String.Format("request/{0}", id));
            return response;

        }

        // PUT: api/Request/5
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
