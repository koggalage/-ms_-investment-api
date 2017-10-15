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
        private BrokerRepository _brokerRepository;
        private GuarantorRepository _guarantorRepository;
        private CustomerRepository _customerRepository;

        public ContractsService()
        {
            _contractsRepository = new ContractsRepository();
            _brokerRepository = new BrokerRepository();
            _guarantorRepository = new GuarantorRepository();
            _customerRepository = new CustomerRepository();
        }

        public GetCustomerDetailsVM GetCustomerDetailsModel()
        {
            var customersList = _contractsRepository.GetCustomers();

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

        public List<SearchOptionsModel> GetContractsBySearchTerm(string searchTerm)
        {
            return _contractsRepository.GetContractsBySearchTerm(searchTerm);
        }

        public bool CreateContract(ContractModel contractModel)
        {

            _contractsRepository.CreateContract(contractModel);

            return true;
        }

        public GetBrokerDetailsVM GetBrokersModel()
        {
            var brokersList = _contractsRepository.GetBrokers();

            var Model = new GetBrokerDetailsVM();

            Model.BrokerDetails = new List<BrokerModel>();
            foreach (var broker in brokersList)
            {
                Model.BrokerDetails.Add(new BrokerModel()
                {
                    Name = broker.Name,
                    NIC = broker.NIC
                });
            }

            return Model;
        }

        public List<Customer> GetCustomersForOpenContractsModel()
        {
            return _contractsRepository.GetCustomersForOpenContracts();
        }

        public List<ContractModel> GetVehicleNoByCustomerIdModel(string customerId)
        {
            return _contractsRepository.GetVehicleNoByCustomerIdModel(customerId);
        }

    }
}