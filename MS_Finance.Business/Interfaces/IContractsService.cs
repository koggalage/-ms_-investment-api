using MS_Finance.Model.Models;
using MS_Finance.Model.Repositories.OA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS_Finance.Business.Interfaces
{
    public interface IContractsService
    {
        GetCustomerDetailsVM GetCustomerDetailsModel();

        List<SearchOptionsModel> GetContractsBySearchTerm(string searchTerm);

        bool CreateContract(ContractModel contractModel);

        GetBrokerDetailsVM GetBrokersModel();

        List<ContractModel> GetActiveContracts();

        List<Customer> GetCustomersForOpenContractsModel();

        List<ContractModel> GetVehicleNoByCustomerIdModel(string customerId);

        decimal GetMonthlyInstallmentModel(decimal Amount, int NoOfInstallments);
    }
}
