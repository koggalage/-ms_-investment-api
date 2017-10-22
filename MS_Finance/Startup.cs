using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using MS_Finance.Providers;
using Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

[assembly: OwinStartup(typeof(MS_Finance.Startup))]
namespace MS_Finance
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();
            config.EnableCors(new EnableCorsAttribute("*", "*", "GET, POST, OPTIONS, PUT, DELETE"));
            WebApiConfig.Register(config);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            
            ConfigureAuth(app);

            app.UseWebApi(config);
        }
    }
}