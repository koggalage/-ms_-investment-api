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
    public class ContractsService : DefaultPersistentService<Contract>, IContractsService
    {
        protected ICustomerService CustomerService;
        protected IBrokerService BrokerService;
        protected IGuarantorService GuarantorService;

        public ContractsService(IUnitOfWork UoW,
            ICustomerService CustomerService,
            IBrokerService BrokerService,
            IGuarantorService GuarantorService)
            :base(UoW)
        {
            this.CustomerService = CustomerService;
            this.BrokerService = BrokerService;
            this.GuarantorService = GuarantorService;
        }


        public IList<Contract> GetAll()
        {
            return base
                .GetAll()
                .ToList();
        }

        public Contract GetById(string id)
        {
            return base.GetSingle(x => x.Id == id);
        }

        public void Create(Contract contract)
        {
            base.Add(contract);
        }

        public void Update(Contract contract)
        {
            base.Update(contract);
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public GetCustomerDetailsVM GetCustomerDetailsModel()
        {
            var customersList = CustomerService.GetAll();

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
            searchTerm = !string.IsNullOrEmpty(searchTerm) ? searchTerm.ToLower() : string.Empty;

            var result = (from a in base.GetAll()
                          where a.IsOpen && (a.VehicleNo.ToLower() == searchTerm || a.Customer.NIC.ToLower() == searchTerm)
                          select new SearchOptionsModel { VehicleNumber = a.VehicleNo, Name = a.Customer.Name, NIC = a.Customer.NIC, ContractId = a.Id })
                         .ToList();

            return result;
        }

        public bool CreateContract(ContractModel contractModel)
        {
            var customer = UoW.Customers.GetSingle(x => x.Id == contractModel.CustomerId);//  CustomerService.GetById(contractModel.CustomerId);
            var broker =  UoW.Brokers.GetSingle(x => x.Id == contractModel.BrokerId); //BrokerService.GetById(contractModel.BrokerId);


            var contract = new Contract()
            {
                ContractNo = contractModel.ContractNo,
                Amount = contractModel.Amount,
                NoOfInstallments = contractModel.NoOfInstallments,
                Insallment = contractModel.Insallment,
                Type = contractModel.Type,
                VehicleNo = contractModel.VehicleNo,
                IsOpen = true
            };


            if (customer != null)
            {
                UoW.Customers.Attach(customer);
                //CustomerService.Attach(customer);
            }

            if (broker != null)
            {
                UoW.Brokers.Attach(broker);
                //BrokerService.Attach(broker);
            }

            contract.Customer = customer;
            contract.Broker = broker;

            UoW.Contracts.Add(contract);// base.Add(contract);

            return true;
        }


        public GetBrokerDetailsVM GetBrokersModel()
        {
            var brokersList = BrokerService.GetAll();

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
            return base.GetAll().Where(x => x.IsOpen).Select(x => new ContractModel()
            {
                Id = x.Id,
                Amount = x.Amount,
                VehicleNo = x.VehicleNo,
                CustomerName = x.Customer != null ? x.Customer.Name : string.Empty,
                ContractNo = x.Customer != null ? x.Customer.MobileNumber : string.Empty,
            }).ToList();
        }


        public List<Customer> GetCustomersForOpenContractsModel()
        {
            var result = (from a in base.GetAll()
                          where a.IsOpen == true
                          select a.Customer)
              .GroupBy(a => a.Id)
              .Select(s => s.FirstOrDefault())
              .ToList();


            return result;
        }

        public List<ContractModel> GetVehicleNoByCustomerIdModel(string customerId)
        {
            return base.GetAll().Where(c => c.Customer.Id == customerId && c.IsOpen)
                .Select(x => new ContractModel()
                {
                    Id = x.Id,
                    VehicleNo = x.VehicleNo
                }).ToList();            
        }



        //private IContractsRepository _contractsRepository;
        //private IBrokerRepository _brokerRepository;
        //private IGuarantorRepository _guarantorRepository;
        //private ICustomerRepository _customerRepository;

        //public ContractsService(
        //    CustomerRepository customerRepository, 
        //    ContractsRepository contractsRepository, 
        //    BrokerRepository brokerRepository,
        //    GuarantorRepository guarantorRepository)
        //{
        //    this._contractsRepository = contractsRepository;
        //    this._brokerRepository = brokerRepository;
        //    this._guarantorRepository = guarantorRepository;
        //    this._customerRepository = customerRepository;
        //}

        //public GetCustomerDetailsVM GetCustomerDetailsModel()
        //{
        //    var customersList = _contractsRepository.GetCustomers();

        //    var Model = new GetCustomerDetailsVM();

        //    Model.CustomerDetails = new List<CustomerModel>();
        //    foreach (var customer in customersList)
        //    {
        //        Model.CustomerDetails.Add(new CustomerModel()
        //        {
        //            Name = customer.Name,
        //            NIC = customer.NIC
        //        });
        //    }

        //    return Model;
        //}

        //public List<SearchOptionsModel> GetContractsBySearchTerm(string searchTerm)
        //{
        //    return _contractsRepository.GetContractsBySearchTerm(searchTerm);
        //}

        //public bool CreateContract(ContractModel contractModel)
        //{

        //    _contractsRepository.CreateContract(contractModel);

        //    return true;
        //}

        //public GetBrokerDetailsVM GetBrokersModel()
        //{
        //    var brokersList = _contractsRepository.GetBrokers();

        //    var Model = new GetBrokerDetailsVM();

        //    Model.BrokerDetails = new List<BrokerModel>();
        //    foreach (var broker in brokersList)
        //    {
        //        Model.BrokerDetails.Add(new BrokerModel()
        //        {
        //            Name = broker.Name,
        //            NIC = broker.NIC
        //        });
        //    }

        //    return Model;
        //}

        //public List<ContractModel> GetActiveContracts()
        //{
        //    return _contractsRepository.GetActiveContracts();
        //}

        //public List<Customer> GetCustomersForOpenContractsModel()
        //{
        //    return _contractsRepository.GetCustomersForOpenContracts();
        //}

        //public List<ContractModel> GetVehicleNoByCustomerIdModel(string customerId)
        //{
        //    return _contractsRepository.GetVehicleNoByCustomerIdModel(customerId);
        //}

    }
}