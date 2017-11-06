using MS_Finance.Model;
using MS_Finance.Model.Models;
using MS_Finance.Model.Repositories.Interfaces;
using MS_Finance.Model.Repositories.OA;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MS_Finance.Repositories
{
    public class ContractsRepository : IContractsRepository
    {
        MSDataContext _context;

        public ContractsRepository()
        {
            _context = new MSDataContext();
        }

        public List<Customer> GetCustomers()
        {
            return _context.Customers.ToList();       
        }

        public List<SearchOptionsModel> GetContractsBySearchTerm(string searchString) 
        {
            searchString = !string.IsNullOrEmpty(searchString) ? searchString.ToLower() : string.Empty;

            var result = (from a in _context.Contracts
                          where a.IsOpen && (a.VehicleNo.ToLower() == searchString || a.Customer.NIC.ToLower() == searchString)
                          select new SearchOptionsModel { VehicleNumber = a.VehicleNo, Name = a.Customer.Name, NIC = a.Customer.NIC, ContractId = a.Id })
                          .ToList();

            return result;
        }

        public List<Broker> GetBrokers()
        {
            return _context.brokers.ToList();
        }

        public void CreateContract(ContractModel contractModel)
        {

            var customer = _context.Customers.Where(x => x.Id == contractModel.CustomerId).FirstOrDefault();
            var broker = _context.brokers.Where(x => x.Id == contractModel.BrokerId).FirstOrDefault();

            var conract = new Contract()
            {
                ContractNo = contractModel.ContractNo,
                Amount = contractModel.Amount,
                NoOfInstallments = contractModel.NoOfInstallments,
                Insallment = contractModel.Insallment,
                Type = contractModel.Type,
                VehicleNo = contractModel.VehicleNo,
                IsOpen = true
            };

            _context.Contracts.Add(conract);

            if (customer != null)
            {
                _context.Customers.Attach(customer);
            }
            if (broker != null)
            {
                _context.brokers.Attach(broker);
            }

            conract.Customer = customer;
            conract.Broker = broker;

            _context.SaveChanges();
        }

        public List<ContractModel> GetActiveContracts()
        {

            return _context.Contracts.Where(x => x.IsOpen).Select(x => new ContractModel()
            {
                Id = x.Id,
                Amount = x.Amount,
                VehicleNo = x.VehicleNo,
                CustomerName = x.Customer != null ? x.Customer.Name : string.Empty,
                ContractNo = x.Customer != null ? x.Customer.MobileNumber : string.Empty,
            }).ToList();
        }

        public List<Customer> GetCustomersForOpenContracts()
        {
            var result = (from a in _context.Contracts
                          where a.IsOpen == true
                          select a.Customer)
                          .GroupBy(a => a.Id)
                          .Select(s => s.FirstOrDefault())
                          .ToList();


            return result;
        }

        public List<ContractModel> GetVehicleNoByCustomerIdModel(string customerId)
        {
            return _context.Contracts.Where(c => c.Customer.Id == customerId && c.IsOpen)
                .Select(x => new ContractModel()
                {
                    Id = x.Id,
                    VehicleNo = x.VehicleNo
                }).ToList();

        }

    }
}