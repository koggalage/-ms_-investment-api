using MS_Finance.Business.Interfaces;
using MS_Finance.Business.Services;
using MS_Finance.Model.Models;
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
    public class ContractRateController : BaseApiController
    {

        private IContractRateService ConrtactRateService;

        public ContractRateController(ContractRateService ConrtactRateService)
        {
            this.ConrtactRateService = ConrtactRateService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public HttpResponseMessage AddNewContractRate(ContractRateModel model)
        {
            model.CreatedByUserId = User.Identity.GetUserId();
            model.CreatedByUserName = User.Identity.Name;

            ConrtactRateService.ObsoletePreviousAndAddNewContractRate(model);

            return Request.CreateResponse(HttpStatusCode.OK, "New rate added successfuly");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public HttpResponseMessage GetAllRates(int type)
        {
            return Request.CreateResponse(HttpStatusCode.OK, ConrtactRateService.GetRates(type));
        }
    }
}
