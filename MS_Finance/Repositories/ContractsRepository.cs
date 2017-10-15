using MS_Finance.Model.Models;
using MS_Finance.Model.Repositories.OA;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MS_Finance.Repositories
{
    public class ContractsRepository
    {
        AuthContext _context;

        public ContractsRepository()
        {
            _context = new AuthContext();
        }

        public List<Customer> GetCustomers()
        {
            return _context.Customers.ToList();       
        }

        public List<SearchOptionsModel> GetContractsBySearchTerm(string searchString) 
        {
            var result = (from a in _context.Contracts
                          where a.VehicleNo == searchString
                          select new SearchOptionsModel { VehicleNumber = a.VehicleNo })
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
                //Customer = _customerRepository.GetSingle(contractModel.CustomerId),
                Amount = contractModel.Amount,
                NoOfInstallments = contractModel.NoOfInstallments,
                Insallment = contractModel.Insallment,
                Type = contractModel.Type,
                VehicleNo = contractModel.VehicleNo,
                //Guarantor = _guarantorRepository.GetSingle(contractModel.GuarantorId),
                //Broker = _brokerRepository.GetSingle(contractModel.BrokerId),
            };

            _context.Contracts.Add(conract);

            _context.Customers.Attach(customer);
            _context.brokers.Attach(broker);

            conract.Customer = customer;
            conract.Broker = broker;

            _context.SaveChanges();

            //_context.Contracts.Attach(contract);
            //_context.Entry(contract).State = EntityState.Modified;

            //_context.SaveChanges();
        }

    }
}