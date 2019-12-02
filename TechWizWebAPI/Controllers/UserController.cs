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
    public class UserController : ApiController
    {
        // GET: api/User
        public ArrayList Get()
        {
            UserPersistence up = new UserPersistence();
            return up.getUsers();
        }

        // GET: api/User/5
        public User Get(long id)
        {
            UserPersistence up = new UserPersistence();
            User user = up.getUser(id);
            return user;

        }

        // POST: api/User
        public HttpResponseMessage Post([FromBody]User value)
        {
            UserPersistence up = new UserPersistence();
            long id;
            id =up.saveUser(value);
            value.ID = id;
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);
            response.Headers.Location = new Uri(Request.RequestUri, String.Format("user/{0}", id));
            return response;
        }

        // PUT: api/User/5
        public HttpResponseMessage Put(long id, [FromBody]User value)
        {
            UserPersistence up = new UserPersistence();
            bool recordExisted = false;
            recordExisted = up.updateUser(id, value);
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

        // DELETE: api/User/5
        public HttpResponseMessage Delete(long id)
        {
            UserPersistence up = new UserPersistence();
            bool recordExisted = false;
            recordExisted = up.deleteUser(id);

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
