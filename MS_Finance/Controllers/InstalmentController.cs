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
        public virtual HttpResponseMessage GetCurrentInstalmentDetails(string contractId, DateTime? paidDate)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _instalmentService.GetCurrentInstalmentDetails(contractId, DateTime.Now));
        }

        [HttpGet]
        public virtual HttpResponseMessage GetContractsToBeClosed()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _instalmentService.GetContractsToBeClosed());
        }


        [HttpGet]
        public virtual HttpResponseMessage GetPaybleAtContractClosingDate(string contractId, DateTime paidDate)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _instalmentService.GetPaybleAtContractClosingDate(contractId, paidDate));
        }


        [HttpPost]
        public virtual HttpResponseMessage CloseContract(ContractCloseModel closeModel)
        {
            var userId = User.Identity.GetUserId();
            var userName = User.Identity.Name;

            return Request.CreateResponse(HttpStatusCode.OK, _instalmentService.CloseContract(closeModel.ContractId, closeModel.SettlementAmount, userId, userName, closeModel.ClosedDate));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public virtual HttpResponseMessage GetInstalmentsToBeApproved()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _instalmentService.GetInstalmentsToBeApproved());
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public virtual HttpResponseMessage ApproveInstalment(string instalmentId)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _instalmentService.ApproveInstalment(instalmentId));
        }

        [HttpGet]
        public virtual HttpResponseMessage GetNumberOfInstalmentsForToday()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _instalmentService.GetNumberOfInstalments(DateTime.Now, DateTime.Now));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public virtual HttpResponseMessage GetAccuredRevenueForToday()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _instalmentService.GetAccuredRevenue(DateTime.Now, DateTime.Now));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public virtual HttpResponseMessage GetRevenueForToday()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _instalmentService.GetRevenue(DateTime.Now, DateTime.Now));
        }

        [HttpGet]
        public virtual HttpResponseMessage GetNumberOfContracts()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _instalmentService.GetNumberOfContracts(DateTime.Now, DateTime.Now));
        }

        [HttpGet]
        public virtual HttpResponseMessage GetInstalmentsForToday()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _instalmentService.GetInstalmentsList(DateTime.Now, DateTime.Now));
        }
        
        [HttpGet]
        public virtual HttpResponseMessage GetContractDetails(string contractId)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _instalmentService.GetContractDetails(contractId));
        }

        [HttpGet]
        public virtual HttpResponseMessage GetContractDocuments(string contractId)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _instalmentService.GetContractDocuments(contractId));
        }

    }
}
