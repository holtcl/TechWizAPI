using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace TechWizWebAPI.Controllers
{
    public class ContactMethodsController : ApiController
    {
        [Authorize]
        // GET: api/ContactMethods
        public ArrayList Get()
        {
            return new SkillAndContactPersistence().getContactMethods();
        }
    }
}