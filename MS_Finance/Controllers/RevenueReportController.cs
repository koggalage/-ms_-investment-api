using MS_Finance.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MS_Finance.Controllers
{
    [RoutePrefix("api/RevenueReport")]
    public class RevenueReportController : ApiController
    {
        protected IInstalmentService InstalmentService;

        public RevenueReportController(IInstalmentService InstalmentService)
        {
            this.InstalmentService = InstalmentService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public virtual HttpResponseMessage GetRevenueReport(DateTime from, DateTime to)
        {
            return Request.CreateResponse(InstalmentService.GetRevenueReport(from, to));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public virtual HttpResponseMessage GetAccruedRevenueReport(DateTime from, DateTime to)
        {
            return Request.CreateResponse(InstalmentService.GetAccruedRevenueReport(from, to));
        }
    }
}
