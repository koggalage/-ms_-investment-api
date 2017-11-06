using MS_Finance.Business.Interfaces;
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
    public class ContractsService : IContractsService
    {
        private IContractsRepository _contractsRepository;
        private IBrokerRepository _brokerRepository;
        private IGuarantorRepository _guarantorRepository;
        private ICustomerRepository _customerRepository;

        public ContractsService(
            CustomerRepository customerRepository, 
            ContractsRepository contractsRepository, 
            BrokerRepository brokerRepository,
            GuarantorRepository guarantorRepository)
        {
            this._contractsRepository = contractsRepository;
            this._brokerRepository = brokerRepository;
            this._guarantorRepository = guarantorRepository;
            this._customerRepository = customerRepository;
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

        public List<ContractModel> GetActiveContracts()
        {
            return _contractsRepository.GetActiveContracts();
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