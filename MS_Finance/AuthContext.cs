using Microsoft.AspNet.Identity.EntityFramework;
using MS_Finance.Model.Repositories.OA;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MS_Finance
{

    //public class ApplicationUser : IdentityUser 
    //{ 
        
    //}


    public class AuthContext : IdentityDbContext<IdentityUser>
    {
        public AuthContext()
            : base("AuthContext", throwIfV1Schema: false)
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        public static AuthContext Create()
        {
            return new AuthContext();
        }

        public DbSet<Department> Departments { get; set; }
    }
}