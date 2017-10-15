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

    [RoutePrefix("api/Customer")]
    public class CustomerController : ApiController
    {
        private CustomerService _customerService;

        public CustomerController()
        {
            _customerService = new CustomerService();
        }

        [HttpPost]
        public virtual HttpResponseMessage CreateCustomer(CustomerModel customer) 
        {
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
