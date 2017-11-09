using MS_Finance.Business;
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
    [RoutePrefix("api/Broker")]
    public class BrokerController : BaseApiController
    {
        private IBrokerService _brokerService;

        public BrokerController(BrokerService brokerService)
        {
            this._brokerService = brokerService;
        }

        
        [HttpPost]
        public virtual HttpResponseMessage CreateBroker(BrokerModel broker)
        {
            broker.CreatedByUserId = User.Identity.GetUserId();
            broker.CreatedByUserName = User.Identity.Name;

            _brokerService.CreateBroker(broker);

            return Request.CreateResponse(HttpStatusCode.OK, true);
        }

        [HttpPost]
        public virtual HttpResponseMessage GetBrokerExistency(string brokerNIC)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _brokerService.IsBrokerExist(brokerNIC));
        }
    }
}
