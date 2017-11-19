﻿using MS_Finance.Business.Interfaces;
using MS_Finance.Business.Models.EnumsAndConstants;
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
            : base(UoW)
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
            var customer = UoW.Customers.GetSingle(x => x.Id == contractModel.CustomerId);
            var broker = UoW.Brokers.GetSingle(x => x.Id == contractModel.BrokerId);


            var contract = new Contract()
            {
                ContractNo          = contractModel.ContractNo,
                Amount              = contractModel.Amount,
                NoOfInstallments    = contractModel.NoOfInstallments,
                Insallment          = contractModel.Insallment,
                Type                = contractModel.Type,
                VehicleNo           = contractModel.VehicleNo,
                IsOpen              = true,
                CreatedByUserId     = contractModel.CreatedByUserId,
                CreatedByUserName   = contractModel.CreatedByUserName,
                LicenceExpireDate   = DateTime.Now.AddYears(2),
                CreatedOn           = DateTime.Now,
                Customer            = customer,
                Broker              = broker
            };


            try
            {

                var emptyInstalments = CreateEmptyInstalmentsForContract(contractModel, contract.Id);
                contract.ContractInstallments = emptyInstalments;
                UoW.Contracts.Add(contract);
                UoW.Commit();

            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }


        private List<ContractInstallment> CreateEmptyInstalmentsForContract(ContractModel contractModel, string contractId)
        {

            var noOfInstalments = contractModel.NoOfInstallments;
            var instalments = new List<ContractInstallment>();
            var dueDate = DateTime.Now.AddMonths(1);

            while (noOfInstalments > 0)
            {
                var instalment = new ContractInstallment()
                {
                    DueDate             = dueDate,
                    Paid                = (int)InstalmentPaymentStatus.NotPaid,
                    CreatedByUserId     = contractModel.CreatedByUserId,
                    CreatedByUserName   = contractModel.CreatedByUserName
                };

                dueDate = dueDate.AddMonths(1);
                instalments.Add(instalment);
                noOfInstalments--;
            }

            return instalments;
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
                Id              = x.Id,
                Amount          = x.Amount,
                VehicleNo       = x.VehicleNo,
                CustomerName    = x.Customer != null ? x.Customer.Name : string.Empty,
                ContractNo      = x.Customer != null ? x.Customer.MobileNumber : string.Empty,
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


        public decimal GetMonthlyInstallmentModel(decimal Amount, int NoOfInstallments)
        {
            double interestRate = (NoOfInstallments <= 6) ? 0.30 : (NoOfInstallments > 6) ? 0.36 : double.NaN;
            decimal Insallment = ((Amount * Convert.ToDecimal(interestRate)) + Amount) / 12;
            Insallment = Math.Round(Insallment, 2);
            return Insallment;
        }


    }
}