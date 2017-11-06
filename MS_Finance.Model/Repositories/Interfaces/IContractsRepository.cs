using MS_Finance.Model.Models;
using MS_Finance.Model.Repositories.OA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS_Finance.Model.Repositories.Interfaces
{
    public interface IContractsRepository
    {
        List<Customer> GetCustomers();

        List<SearchOptionsModel> GetContractsBySearchTerm(string searchString);

        List<Broker> GetBrokers();

        void CreateContract(ContractModel contractModel);

        List<ContractModel> GetActiveContracts();

        List<Customer> GetCustomersForOpenContracts();

        List<ContractModel> GetVehicleNoByCustomerIdModel(string customerId);
    }
}
