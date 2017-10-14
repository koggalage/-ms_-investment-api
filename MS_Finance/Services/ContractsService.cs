using MS_Finance.Model.Models;
using MS_Finance.Model.Repositories.OA;
using MS_Finance.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MS_Finance.Services
{
    public class ContractsService
    {
        private ContractsRepository _contractsRepository;

        public ContractsService()
        {
            _contractsRepository = new ContractsRepository();
        }

        public GetCustomerDetailsVM GetCustomerDetailsModel()
        {
            var customersList = _contractsRepository.GetCustomersr();

            var Model = new GetCustomerDetailsVM();

            Model.CustomerDetails = new List<CustomerModel>();
            foreach (var customer in customersList)
            {
                Model.CustomerDetails.Add(new CustomerModel()
                {
                    Name = customer.Name,
                    NIC = customer.NIC
                });
            }

            return Model;
        }
    }
}