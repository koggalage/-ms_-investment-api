using MS_Finance.Model.Models;
using MS_Finance.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MS_Finance.Controllers
{
    [RoutePrefix("api/Contract")]
    public class ContractController : ApiController
    {

        private ContractsService _contractsService;

        public ContractController()
        {
            _contractsService = new ContractsService();
        }

        [HttpGet]
        public virtual HttpResponseMessage GetCustomerDetails()
        {
            return Request.CreateResponse<GetCustomerDetailsVM>(HttpStatusCode.OK, _contractsService.GetCustomerDetailsModel());
        }
    }
}
