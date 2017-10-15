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
    [RoutePrefix("api/Broker")]
    public class BrokerController : ApiController
    {
        private BrokerService _brokerService;

        public BrokerController()
        {
            _brokerService = new BrokerService();
        }

        

        [HttpPost]
        public virtual HttpResponseMessage CreateBroker(BrokerModel broker)
        {
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
