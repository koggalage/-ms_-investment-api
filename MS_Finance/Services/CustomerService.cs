using MS_Finance.Model.Models;
using MS_Finance.Model.Repositories.OA;
using MS_Finance.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MS_Finance.Services
{
    public class CustomerService
    {

        private CustomerRepository _customerRepository;

        public CustomerService()
        {
            _customerRepository = new CustomerRepository();
        }

        public bool CreateCustomer(CustomerModel customerModel)
        {
            var customer = new Customer()
            {
                 Name = customerModel.Name,
                 Address = customerModel.Address,
                 MobileNumber = customerModel.Mobile,
                 NIC = customerModel.NIC,
                 Occupation = customerModel.Occupation,
                 CreatedDate = DateTime.Now
            };

            _customerRepository.CreateCustomer(customer);

            return true;
        }

        public Customer IsCustomerExist(string customerNIC)
        {
            var customeByNIC = _customerRepository.GetCustomerByNIC(customerNIC);

            return customeByNIC;
        }

    }
}