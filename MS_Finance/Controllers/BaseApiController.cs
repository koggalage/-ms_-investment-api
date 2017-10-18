using MS_Finance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace MS_Finance.Controllers
{
    public class BaseApiController : ApiController
    {

        protected UserSessionModel UserSessionModel 
        {
            get
            {
                if (User.Identity.IsAuthenticated)
                {
                    var user = User.Identity as ClaimsIdentity;
                    var userId = user.Claims.FirstOrDefault(o => o.Type == "sub").Value;
                    Guid tempUserId;
                    Guid.TryParse(userId, out tempUserId);

                    return new UserSessionModel
                    {
                        UserId = tempUserId,
                        Roles = user.Claims.FirstOrDefault(o => o.Type == "role").Value.Split(',')
                    };
                }

                return null;
            }
        }

        protected bool IsInRole(string roleName)
        {
            return UserSessionModel != null && UserSessionModel.Roles.Any(r => r == roleName);
        }

    }
}
