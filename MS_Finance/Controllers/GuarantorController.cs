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
    public class GuarantorController : BaseApiController
    {

        private GuarantorService _guarantorService;

        public GuarantorController()
        {
            _guarantorService = new GuarantorService();
        }

        [HttpPost]
        public virtual HttpResponseMessage CreateGuarantor(GuarantorModel guarantor)
        {
            _guarantorService.CreateGuarantor(guarantor);

            return Request.CreateResponse(HttpStatusCode.OK, true);
        }

    }
}
