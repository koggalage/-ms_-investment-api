using MS_Finance.Model.Models;
using MS_Finance.Model.Repositories.OA;
using System;
using System.Collections.Generic;
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

        public List<Customer> GetCustomersr()
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
    }
}