using MS_Finance.Model.Models;
using MS_Finance.Model.Repositories.OA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS_Finance.Business.Interfaces
{
    public interface IInstalmentService
    {

        IQueryable<ContractInstallment> GetAll();

        //ContractInstallment GetById(Guid id);

        //void Create(ContractInstallment installment);

        //void Update(ContractInstallment installment);

        //void Delete(Guid id);

        bool CreateInstalment(ContractInstalmentModel instalmentModel);

        List<ContractInstallment> GetInstalmentsForContract(string contractId);

        ContractInstallment GetInstalmentToBePaid(string contractId);

        List<ContractInstallment> GetPartialyPaidInstalments(string contractId);

        decimal CalculateFineForPreviousUnsettleInstalments(string contractId, DateTime paidDate);

        void AddOrUpdateFine(string contractId, decimal fine);

        void AddOrUpdateExcess(string contractId, decimal excess);

        decimal CalculateFineForCurrentInstalmentAndUpdateExcess(string contractId, DateTime dueDate, DateTime paidDate);

        ContractInstalmentModel GetCurrentInstalmentDetails(string contractId, DateTime paidDate);

        RevenueRecordModel GetRevenueReport(DateTime from, DateTime to);

        RevenueRecordModel GetAccruedRevenueReport(DateTime from, DateTime to);

        decimal GetTotalPaidPayment(string contractId);

        List<ContractCloseModel> GetContractsToBeClosed();

        bool CloseContract(string contractId, decimal settlementAmount, string createdByUserId, string createdByUserName, DateTime closedDate);

        decimal GetPaybleAtContractClosingDate(string contractId, DateTime closedDate);

        List<InstalmentApproveModel> GetInstalmentsToBeApproved();

        bool ApproveInstalment(string instalmentId);

        int GetNumberOfInstalments(DateTime from, DateTime to);

        decimal GetAccuredRevenue(DateTime from, DateTime to);

        decimal GetRevenue(DateTime from, DateTime to);

        int GetNumberOfContracts(DateTime from, DateTime to);

        List<RevenueRecord> GetInstalmentsList(DateTime from, DateTime to);

        ContractDetailModel GetContractDetails(string contractId);
    }
}
