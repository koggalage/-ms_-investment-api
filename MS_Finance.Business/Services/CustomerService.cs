using MS_Finance.Business.Interfaces;
using MS_Finance.Business.Services;
using MS_Finance.Model.Models;
using MS_Finance.Model.Repositories.Interfaces;
using MS_Finance.Model.Repositories.OA;
using MS_Finance.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MS_Finance.Services
{
    public class CustomerService : DefaultPersistentService<Customer>,  ICustomerService
    {

        public CustomerService(IUnitOfWork UoW) 
        :base(UoW)
        {
            
        }


        public IList<Customer> GetAll()
        {
            return base
                .GetAll()
                .ToList();
        }

        public Customer GetById(string id)
        {
            return base.GetSingle(x => x.Id == id);
        }

        public void Create(Customer customer)
        {
            base.Add(customer);
        }

        public void Update(Customer customer)
        {
            base.Update(customer);
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Attach(Customer customer)
        {
            base.Attach(customer);
        }

        public bool CreateCustomer(CustomerModel customerModel)
        {
            var customer = new Customer()
            {
                 Name              = customerModel.Name,
                 Address           = customerModel.Address,
                 MobileNumber      = customerModel.Mobile,
                 NIC               = customerModel.NIC,
                 Occupation        = customerModel.Occupation,
                 CreatedDate       = customerModel.CreatedOn,
                 CreatedByUserId   = customerModel.CreatedByUserId,
                 CreatedByUserName = customerModel.CreatedByUserName
            };

            base.Add(customer);

            return true;
        }

        public Customer IsCustomerExist(string customerNIC)
        {
            //var customeByNIC = _customerRepository.GetCustomerByNIC(customerNIC);

            return base
                .GetAll()
                .Where(x => x.NIC == customerNIC).FirstOrDefault();
        }

    }
}