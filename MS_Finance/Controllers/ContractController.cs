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
        private ILocationsService _locationService;
        private IImportFacadeService _importFacadeService;

        public ContractController(ContractsService contractsService, FileUploadService fileUploadService, LocationsService locationService, ImportFacadeService importService)
        {
            this._contractsService = contractsService;
            this._fileUploadService = fileUploadService;
            this._locationService = locationService;
            this._importFacadeService = importService;
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

            string uploadedFileName = string.Empty;
            //string imagePath;
            // CHECK THE FILE COUNT.
            for (int iCnt = 0; iCnt <= hfc.Count - 1; iCnt++)
            {
                System.Web.HttpPostedFile hpf = hfc[iCnt];

                if (hpf.ContentLength > 0)
                {

                    uploadedFileName = Path.GetFileName(hpf.FileName);
                    uploadedFileName = uploadedFileName.Replace(" ","");
                    uploadedFileName = Guid.NewGuid().ToString("N") + uploadedFileName;

                    //CHECK IF THE SELECTED FILE(S) ALREADY EXISTS IN FOLDER. (AVOID DUPLICATE)
                    if (!File.Exists(sPath + uploadedFileName))
                    {
                        // SAVE THE FILES IN THE FOLDER.
                        hpf.SaveAs(sPath + uploadedFileName);
                        iUploadedCnt = iUploadedCnt + 1;
                    }
                    // CHECK IF THE SELECTED FILE(S) ALREADY EXISTS IN FOLDER. (AVOID DUPLICATE)
                    //if (!File.Exists(sPath + Path.GetFileName(hpf.FileName)))
                    //{
                    //    // SAVE THE FILES IN THE FOLDER.
                    //    hpf.SaveAs(sPath + Path.GetFileName(hpf.FileName));
                    //    iUploadedCnt = iUploadedCnt + 1;
                    //}
                }
                if (iCnt <= hfc.Count - 1)
                {
                    _contractsService.UploadContractFiles(contractId, uploadedFileName, createdByUserId, createdByUserName);
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

        [HttpGet]
        public HttpResponseMessage GetDocumentCharge(decimal amount)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _contractsService.GetDocumentCharge(amount));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public HttpResponseMessage GetDocumentChargeReport(DateTime from, DateTime to)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _contractsService.GetDocumentChargeReport(from, to));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public HttpResponseMessage AddContractLocation(ContractLocationModel model)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _locationService.CreateNewLocation(model));
        }

        [HttpGet]
        public HttpResponseMessage GetAllLocations()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _locationService.GetAllLocations());
        }

        [HttpGet]
        public HttpResponseMessage GenerateContractNumber(string code)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _contractsService.GenerateContractNumber(code));
        }

        [HttpGet]
        public HttpResponseMessage ImportCustomers(string filePath)
        {
            _importFacadeService.ImportCustomers(filePath);
            return Request.CreateResponse(HttpStatusCode.OK, true);
        }

        [HttpGet]
        public HttpResponseMessage ImportGuarentors(string filePath)
        {
            _importFacadeService.ImportGuarentors(filePath);
            return Request.CreateResponse(HttpStatusCode.OK, true);
        }

        [HttpGet]
        public HttpResponseMessage ImportBrokers(string filePath)
        {
            _importFacadeService.ImportBrokers(filePath);
            return Request.CreateResponse(HttpStatusCode.OK, true);
        }

        [HttpGet]
        public HttpResponseMessage ImportContracts(string filePath)
        {
            _importFacadeService.ImportContracts(filePath);
            return Request.CreateResponse(HttpStatusCode.OK, true);
        }

        [HttpGet]
        public HttpResponseMessage ImportInstallments(string filePath)
        {
            _importFacadeService.ImportInstallments(filePath);
            return Request.CreateResponse(HttpStatusCode.OK, true);
        }
    }
}
