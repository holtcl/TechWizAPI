﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using TechWizWebAPI.Models;
using System.Collections;
using System.Web.Http;
using System.Net.Http;
using System.Security.Claims;

namespace TechWizWebAPI.Controllers
{
    public class UserController : ApiController
    {
        [Authorize]
        // GET: api/User
        public User Get()
        {
            var identity = User.Identity as ClaimsIdentity;
            if (identity == null)
            {
                return null;
            }
            else
            {
                UserPersistence up = new UserPersistence();
                return up.getUser((long)Int64.Parse(identity.FindFirst("UserID").Value));
            }
        }

        [Authorize]
        // GET: api/User/5
        public User Get(long id)
        {
            UserPersistence up = new UserPersistence();
            User user = up.getUser(id);
            return user;

        }

        // POST: api/User
        public HttpResponseMessage Post([FromBody]User user)
        {
            // This interface doesn't support creating wizards
            user.isWizard = false;
            /*if(user == null ||  String.IsNullOrEmpty(user.UserName) || String.IsNullOrEmpty(user.FirstName) || String.IsNullOrEmpty(user.LastName) || String.IsNullOrEmpty(user.Email) || String.IsNullOrEmpty(user.Password))
                return Request.CreateResponse(HttpStatusCode.BadRequest);
                */
            //check that no values are null or empty
            if (user.GetType().GetProperties()
                .Where(pi => pi.PropertyType == typeof(string))
                .Select(pi => (string)pi.GetValue(user))
                .Any(value => string.IsNullOrEmpty(value)))
                return Request.CreateResponse(HttpStatusCode.BadRequest, "All fields are required.");
            

            UserPersistence up = new UserPersistence();
            long id;
            id =up.saveUser(user);
            user.ID = id;
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);
            response.Headers.Location = new Uri(Request.RequestUri, String.Format("user/{0}", id));
            return response;
        }

        [Authorize]
        // PUT: api/User/5
        public HttpResponseMessage Put(long id, [FromBody]User user)
        {
            if (user.GetType().GetProperties()
                .Where(pi => pi.PropertyType == typeof(string))
                .Select(pi => (string)pi.GetValue(user))
                .Any(value => string.IsNullOrEmpty(value)))
                return Request.CreateResponse(HttpStatusCode.BadRequest);


            UserPersistence up = new UserPersistence();
            bool recordExisted = false;
            recordExisted = up.updateUser(id, user);
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

        [Authorize]
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
