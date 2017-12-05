using MS_Finance.Business.Interfaces;
using MS_Finance.Model.Models;
using MS_Finance.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using System.Web.Security;
using System.IO;
using MS_Finance.Business.Services;

namespace MS_Finance.Controllers
{
    [RoutePrefix("api/Contract")]
    public class ContractController : BaseApiController
    {

        private IContractsService _contractsService;
        private IFileUploadService _fileUploadService;

        public ContractController(ContractsService contractsService, FileUploadService fileUploadService)
        {
            this._contractsService = contractsService;
            this._fileUploadService = fileUploadService;
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
            contract.CreatedByUserId = User.Identity.GetUserId();
            contract.CreatedByUserName = User.Identity.Name;

            return Request.CreateResponse(HttpStatusCode.OK, _contractsService.CreateContract(contract));
        }

        [HttpGet]
        [HttpPost]
        public virtual HttpResponseMessage LoadBrokerDetails()
        {
            return Request.CreateResponse<GetBrokerDetailsVM>(HttpStatusCode.OK, _contractsService.GetBrokersModel());
        }

        [HttpGet]
        public virtual HttpResponseMessage GetActiveContracts()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _contractsService.GetOpenOrClosedContracts(true));
        }

        public virtual HttpResponseMessage GetCustomersForOpenContracts()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _contractsService.GetCustomersForOpenContractsModel());
        }

        [HttpGet]
        public virtual HttpResponseMessage GetVehicleNoByCustomerId(string customerId)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _contractsService.GetVehicleNoByCustomerIdModel(customerId));
        }

        [HttpGet]
        public virtual HttpResponseMessage GetMonthlyInstallment(decimal Amount, int NoOfInstallments)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _contractsService.GetMonthlyInstallmentModel(Amount, NoOfInstallments));
        }

        [HttpGet]
        public virtual HttpResponseMessage GetClosedContracts() 
        {
            return Request.CreateResponse(HttpStatusCode.OK, _contractsService.GetOpenOrClosedContracts(false));
        }

        [HttpPost]
        public string UplaoadFile(string contractId)
        {

            var createdByUserId = User.Identity.GetUserId();
            var createdByUserName = User.Identity.Name;

            int iUploadedCnt = 0;

            // DEFINE THE PATH WHERE WE WANT TO SAVE THE FILES.
            string sPath = "";
            sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/ContractFiles/");

            System.Web.HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;

            string imagePath;
            // CHECK THE FILE COUNT.
            for (int iCnt = 0; iCnt <= hfc.Count - 1; iCnt++)
            {
                System.Web.HttpPostedFile hpf = hfc[iCnt];

                if (hpf.ContentLength > 0)
                {
                    // CHECK IF THE SELECTED FILE(S) ALREADY EXISTS IN FOLDER. (AVOID DUPLICATE)
                    if (!File.Exists(sPath + Path.GetFileName(hpf.FileName)))
                    {
                        // SAVE THE FILES IN THE FOLDER.
                        hpf.SaveAs(sPath + Path.GetFileName(hpf.FileName));
                        iUploadedCnt = iUploadedCnt + 1;
                    }
                }
                if (iCnt <= hfc.Count - 1)
                {
                    _contractsService.UploadContractFiles(contractId, Path.GetFileName(hpf.FileName), createdByUserId, createdByUserName);
                }
            }


            // RETURN A MESSAGE (OPTIONAL).
            if (iUploadedCnt > 0)
            {
                return iUploadedCnt + " Files Uploaded Successfully";
            }
            else
            {
                return "Upload Failed";
            }
        }

        
    }
}
