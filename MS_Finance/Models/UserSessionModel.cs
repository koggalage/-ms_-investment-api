using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MS_Finance.Models
{
    public class UserSessionModel
    {
        public System.Guid UserId { get; set; }

        public string[] Roles { get; set; }
    }
}