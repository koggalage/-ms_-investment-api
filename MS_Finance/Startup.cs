using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using MS_Finance.Providers;
using Ninject;
using Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using Ninject.Web.Common.OwinHost;
using Ninject.Web.WebApi.OwinHost;
using MS_Finance.Business.Interfaces;
using MS_Finance.Services;
using MS_Finance.Business;
using MS_Finance.Model.Repositories.Interfaces;
using MS_Finance.Model.Repositories.OA;
using MS_Finance.Business.Services;

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

            app.UseNinjectMiddleware(CreateKernel).UseNinjectWebApi(config);

            //app.UseWebApi(config);
        }

        private static StandardKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());

            kernel.Bind<IUnitOfWork>().To<UnitOfWork>();
            kernel.Bind<ICustomerService>().To<CustomerService>();
            kernel.Bind<IBrokerService>().To<BrokerService>();
            kernel.Bind<IContractsService>().To<ContractsService>();
            kernel.Bind<IGuarantorService>().To<GuarantorService>();
            kernel.Bind<IInstalmentService>().To<InstalmentService>();
            kernel.Bind<IContractFineService>().To<ContractFineService>();
            kernel.Bind<IExcessService>().To<ExcessService>();
            kernel.Bind<IFinePaymentService>().To<FinePaymentService>();
            kernel.Bind<IContractSettlementService>().To<ContractSettlementService>();
            kernel.Bind<IFileUploadService>().To<FileUploadService>();

            return kernel;
        }
    }
}