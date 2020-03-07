using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace TechWizWebAPI.Controllers
{
    public class SkillsController: ApiController
    {
        [Authorize]
        // GET: api/Skill
        public ArrayList Get() {
            return new SkillAndContactPersistence().getSkills();
        }
    }
}