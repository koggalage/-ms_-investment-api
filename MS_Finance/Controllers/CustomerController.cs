using MS_Finance.Model.Models;
using MS_Finance.Services;
using MS_Finance.Utilities.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MS_Finance.Controllers
{

    [RoutePrefix("api/Customer")]
    public class CustomerController : BaseApiController
    {
        private CustomerService _customerService;

        public CustomerController()
        {
            _customerService = new CustomerService();
        }

        //[ApiAuthorize(Roles="admin")]
        [HttpPost]
        public virtual HttpResponseMessage CreateCustomer(CustomerModel customer) 
        {
            var userSessionModel = UserSessionModel; 

			var userId = userSessionModel.UserId; //get userid

			var roles = userSessionModel.Roles; //get roles


            _customerService.CreateCustomer(customer);

            return Request.CreateResponse(HttpStatusCode.OK, true);
        }

        [HttpPost]
        public virtual HttpResponseMessage GetCustomerExistency(string customerNIC)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _customerService.IsCustomerExist(customerNIC));
        }

    }
}
