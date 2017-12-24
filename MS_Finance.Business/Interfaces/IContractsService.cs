using MS_Finance.Model.Models;
using MS_Finance.Model.Repositories.OA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MS_Finance.Business.Interfaces
{
    public interface IContractsService
    {
        IQueryable<Contract> GetAll();

        IQueryable<Contract> GetAllWithIncludes(params Expression<Func<Contract, object>>[] properties);

        Contract GetById(string id);

        void Create(Contract contract);

        void Update(Contract contract);


        GetCustomerDetailsVM GetCustomerDetailsModel();

        List<SearchOptionsModel> GetContractsBySearchTerm(string searchTerm);

        bool CreateContract(ContractModel contractModel);

        GetBrokerDetailsVM GetBrokersModel();

        List<ContractModel> GetActiveContracts();

        List<Customer> GetCustomersForOpenContractsModel();

        List<ContractModel> GetVehicleNoByCustomerIdModel(string customerId);

        decimal GetMonthlyInstallmentModel(decimal Amount, int NoOfInstallments);

        int GetRunningContractsCount(DateTime from, DateTime to);

        ContractReportModel GetOpenOrClosedContracts(bool open = true);

        void UploadContractFiles(string contractId, string imageName, string createdByUserId, string createdByUserName);

        decimal GetRate(int type, DateTime validFor);

        decimal GetDocumentCharge(decimal amount);

        DocumentChargeRecordModel GetDocumentChargeReport(DateTime from, DateTime to);
    }
}
