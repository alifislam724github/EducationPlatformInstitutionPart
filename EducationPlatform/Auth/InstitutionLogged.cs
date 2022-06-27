using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EducationPlatform.Auth
{
    public class InstitutionLogged: AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {

            var value = httpContext.Session["instituteEmail"];
            if (value != null)
            {
                return true;
            }
            return false;
        }
    }
}