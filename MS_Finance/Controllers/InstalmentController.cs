using MS_Finance.Model.Models;
using MS_Finance.Model.Repositories.OA;
using MS_Finance.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MS_Finance.Controllers
{
    [RoutePrefix("api/Customer")]
    public class InstalmentController : ApiController
    {
        private InstalmentService _instalmentService;

        public InstalmentController()
        {
            _instalmentService = new InstalmentService();
        }

        [HttpPost]
        public virtual HttpResponseMessage CreateCustomer(ContractInstalmentModel instalment) 
        {
            _instalmentService.CreateInstalment(instalment);

            return Request.CreateResponse(HttpStatusCode.OK, true);
        }
    }
}
