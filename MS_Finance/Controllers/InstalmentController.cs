using MS_Finance.Business.Interfaces;
using MS_Finance.Model.Models;
using MS_Finance.Model.Repositories.OA;
using MS_Finance.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using System.Web.Security;

namespace MS_Finance.Controllers
{
    [RoutePrefix("api/Instalment")]
    public class InstalmentController : BaseApiController
    {
        private IInstalmentService _instalmentService;

        public InstalmentController(InstalmentService instalmentService)
        {
            this._instalmentService = instalmentService;
        }

        [HttpPost]
        public virtual HttpResponseMessage CreateInstalment(ContractInstalmentModel instalment) 
        {
            instalment.CreatedByUserId = User.Identity.GetUserId();
            instalment.CreatedByUserName = User.Identity.Name;

            _instalmentService.CreateInstalment(instalment);

            return Request.CreateResponse(HttpStatusCode.OK, true);
        }

        [HttpGet]
        public virtual HttpResponseMessage GetInstalmentsForContract(string contractId)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _instalmentService.GetInstalmentsForContract(contractId));
        }

        [HttpGet]
        public virtual HttpResponseMessage GetCurrentInstalmentDetails(string contractId)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _instalmentService.GetCurrentInstalmentDetails(contractId));
        }
    }
}
