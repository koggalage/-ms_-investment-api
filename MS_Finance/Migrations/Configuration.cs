namespace MS_Finance.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MS_Finance.AuthContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MS_Finance.AuthContext context)
        {

            //var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new AuthContext()));

            //var user = new ApplicationUser()
            //{
            //    UserName = "SuperPowerUser",
            //    Email = "taiseer.joudeh@mymail.com",
            //    EmailConfirmed = true,
            //    //FirstName = "Taiseer",
            //    //LastName = "Joudeh",
            //    //Level = 1,
            //    //JoinDate = DateTime.Now.AddYears(-3)
            //};

            //manager.Create(user, "123456");

            if (!context.Roles.Any(r => r.Name == "admin"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "admin" };

                manager.Create(role);
            }

            if (!context.Users.Any(u => u.UserName == "founder"))
            {
                var store = new UserStore<IdentityUser>(context);
                var manager = new UserManager<IdentityUser>(store);
                var user = new IdentityUser { UserName = "founder" };

                manager.Create(user, "123456");
                manager.AddToRole(user.Id, "admin");
            }

            base.Seed(context);
        }
    }
}
