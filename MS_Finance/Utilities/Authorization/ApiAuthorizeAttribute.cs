using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace MS_Finance.Utilities.Authorization
{
    public class ApiAuthorizeAttribute : AuthorizeAttribute
    {
        
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (!IsAuthorized(actionContext))
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "You are not authorized");
            }
        }

    }
}