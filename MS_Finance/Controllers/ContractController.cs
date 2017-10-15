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

        [HttpGet]
        public virtual HttpResponseMessage GetContracts(string searchString) 
        {
            return Request.CreateResponse(HttpStatusCode.OK, _contractsService.GetContractsBySearchTerm(searchString));
        }

        [HttpPost]
        public virtual HttpResponseMessage CreateContract(ContractModel contract)
        {
            _contractsService.CreateContract(contract);

            return Request.CreateResponse(HttpStatusCode.OK, true);
        }



        [HttpGet]
        [HttpPost]
        public virtual HttpResponseMessage LoadBrokerDetails()
        {
            return Request.CreateResponse<GetBrokerDetailsVM>(HttpStatusCode.OK, _contractsService.GetBrokersModel());
        }
    }
}
