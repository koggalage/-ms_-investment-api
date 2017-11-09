using MS_Finance.Business.Interfaces;
using MS_Finance.Model.Models;
using MS_Finance.Services;
using MS_Finance.Utilities.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Microsoft.AspNet.Identity;
using System.Web.Security;

namespace MS_Finance.Controllers
{

    [RoutePrefix("api/Customer")]
    public class CustomerController : BaseApiController
    {
        private ICustomerService _customerService;

        public CustomerController(CustomerService customerService)
        {
            this._customerService = customerService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public virtual HttpResponseMessage CreateCustomer(CustomerModel customer) 
        {
            customer.CreatedByUserId   = User.Identity.GetUserId();
            customer.CreatedByUserName = User.Identity.Name;

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
