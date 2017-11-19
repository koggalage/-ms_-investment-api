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

namespace MS_Finance.Controllers
{

    [RoutePrefix("api/Contract")]
    public class GuarantorController : BaseApiController
    {

        private IGuarantorService _guarantorService;

        public GuarantorController(GuarantorService guarantorService)
        {
            this._guarantorService = guarantorService;
        }

        [HttpPost]
        public virtual HttpResponseMessage CreateGuarantor(GuarantorModel guarantor)
        {
            guarantor.CreatedByUserId = User.Identity.GetUserId();
            guarantor.CreatedByUserName = User.Identity.Name;
            guarantor.CreatedOn = DateTime.Now;

            _guarantorService.CreateGuarantor(guarantor);

            return Request.CreateResponse(HttpStatusCode.OK, true);
        }

        [HttpPost]
        public virtual HttpResponseMessage GetGuarantorExistency(string guarantorNIC)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _guarantorService.IsGuarantorExist(guarantorNIC));
        }

    }
}
