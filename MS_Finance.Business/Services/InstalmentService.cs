using MS_Finance.Business.Interfaces;
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
using System.Data.Entity;
using System.Data.Objects;

namespace MS_Finance.Services
{
    public class InstalmentService : DefaultPersistentService<ContractInstallment>,  IInstalmentService
    {

        protected IContractsService ContractsService;
        protected IContractFineService ContractFineService;
        protected IExcessService ExcessService;
        protected IFinePaymentService FinePaymentService;
        protected IContractSettlementService ContractSettlementService;

        public InstalmentService(IUnitOfWork UoW,
            IContractsService ContractsService,
            IContractFineService ContractFineService,
            IExcessService ExcessService,
            IFinePaymentService FinePaymentService,
            IContractSettlementService ContractSettlementService)
            :base(UoW)
        {
            this.ContractsService = ContractsService;
            this.ContractFineService = ContractFineService;
            this.ExcessService = ExcessService;
            this.FinePaymentService = FinePaymentService;
            this.ContractFineService = ContractFineService;
            this.ContractSettlementService = ContractSettlementService;
        }

        public IQueryable<ContractInstallment> GetAll()
        {
            return base
                .GetAll();
        }

        public ContractInstallment GetById(string id)
        {
            return base.GetSingle(x => x.Id == id);
        }

        public void Create(ContractInstallment installment)
        {
            base.Add(installment);
        }

        public void Update(ContractInstallment installment)
        {
            base.Update(installment);
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }


        public bool CreateInstalment(ContractInstalmentModel instalmentModel)
        {
            var instalmentToBePaid = GetInstalmentToBePaid(instalmentModel.ContractId);
            var contract = ContractsService.GetById(instalmentModel.ContractId);

            var fineForCurrentInstalment = CalculateFineForCurrentInstalmentAndUpdateExcess(contract.Id, instalmentToBePaid.DueDate, instalmentModel.PaidDate);
            var fineForPreviousUnsettleInstalments = CalculateFineForPreviousUnsettleInstalments(contract.Id, instalmentModel.PaidDate);

            decimal actualCurrentInstalmentPayment  = instalmentModel.PaidAmount;
            
            decimal totalFineForCurrentAndPreviousUnsettleInstalments = fineForCurrentInstalment + fineForPreviousUnsettleInstalments;

            if (totalFineForCurrentAndPreviousUnsettleInstalments > 0)
                AddOrUpdateFine(contract.Id, totalFineForCurrentAndPreviousUnsettleInstalments);

            RecoverFineByExcess(instalmentModel.ContractId, instalmentModel.PaidDate);
            RecoverFineByCurrentInstalment(instalmentModel.ContractId,instalmentModel.PaidDate, ref actualCurrentInstalmentPayment);

            RecoverUnsettledInstalmentsByExcess(instalmentModel.ContractId);
            RecoverUnsettledInstalmentsByCurrentInstalment(instalmentModel.ContractId, ref actualCurrentInstalmentPayment);

            decimal excessOrUnsettleAmount = GetExcessOrUnsettleAmountAtCurrentInstalment(contract.Id, instalmentModel.PaidAmount);

            if (excessOrUnsettleAmount > 0)
                AddOrUpdateExcess(contract.Id, excessOrUnsettleAmount);

            var unsettleAmount = excessOrUnsettleAmount < 0 ? -excessOrUnsettleAmount : 0.0m;

            instalmentToBePaid.PaidAmount           = instalmentModel.PaidAmount;
            instalmentToBePaid.PaidDate             = instalmentModel.PaidDate;
            instalmentToBePaid.LateDays             = (instalmentModel.PaidDate - instalmentModel.DueDate).Days;
            instalmentToBePaid.UnsettleAmount       = unsettleAmount;
            instalmentToBePaid.CreatedByUserId      = instalmentModel.CreatedByUserId;
            instalmentToBePaid.CreatedByUserName    = instalmentModel.CreatedByUserName;
            instalmentToBePaid.Paid                 = unsettleAmount > 0 ? (int)InstalmentPaymentStatus.PartialyPaid : (int)InstalmentPaymentStatus.Completed;
            instalmentToBePaid.Approved             = false;

            base.Update(instalmentToBePaid);

            return true;
        }


        private void RecoverUnsettledInstalmentsByCurrentInstalment(string contractId, ref decimal currentInstalmentAmount)
        {
            var partialyPaidInstalments = UoW.ContractInstallments.GetAll()
                                            .Where(x => x.Paid == (int)InstalmentPaymentStatus.PartialyPaid && x.Contract.Id == contractId)
                                            .ToList();

            foreach (var item in partialyPaidInstalments)
            {
                var paidAmount = 0.0m;
                var unsettleAmount = item.UnsettleAmount;
                var difference = currentInstalmentAmount - unsettleAmount;

                if (difference > 0)
                {
                    item.UnsettleAmount = 0;
                    paidAmount = unsettleAmount;
                }
                else
                {
                    item.UnsettleAmount = unsettleAmount - currentInstalmentAmount;
                    paidAmount = currentInstalmentAmount;
                }

                currentInstalmentAmount -= paidAmount;

                if (item.UnsettleAmount == 0)
                    item.Paid = (int)InstalmentPaymentStatus.Completed;
            }

            UoW.Commit();
        }


        private void RecoverUnsettledInstalmentsByExcess(string contractId)
        {
            var partialyPaidInstalments = UoW.ContractInstallments.GetAll()
                                .Where(x => x.Paid == (int)InstalmentPaymentStatus.PartialyPaid && x.Contract.Id == contractId)
                                .ToList();

            var contractExcess = UoW.Excesses.GetAll()
                                    .Where(x => x.Contract.Id == contractId)
                                    .FirstOrDefault();

            if (contractExcess == null || contractExcess.Amount <= 0)
                return;

            var excess = contractExcess.Amount;
            if (excess <= 0)
                return;

            foreach (var item in partialyPaidInstalments)
            {
                if (excess <= 0)
                    break;

                var unsettleAmount = item.UnsettleAmount;
                var difference = excess - unsettleAmount;

                if (difference > 0)
                {
                    item.UnsettleAmount = 0;
                    excess -= unsettleAmount;
                }
                else
                {
                    item.UnsettleAmount = unsettleAmount - excess;
                    excess = 0;
                }
            }

            UoW.Commit();
        }



        private void RecoverFineByExcess(string contractId, DateTime paidDate)
        {

            var contract = UoW.Contracts.GetSingle(x => x.Id == contractId);

            var contractFine    = UoW.ContractFines.GetAll()
                                    .Where(x => x.Contract.Id == contractId)
                                    .FirstOrDefault();

            var contractExcess  = UoW.Excesses.GetAll()
                                    .Where(x => x.Contract.Id == contractId)
                                    .FirstOrDefault();

            var fine = contractFine != null ? contractFine.Fine : 0.0m;
            var excess = contractExcess != null ? contractExcess.Amount : 0.0m;
            var paidAmount = 0.0m;

            if (contractFine == null || contractExcess == null || excess <= 0)
                return;

            var difference = fine - excess;

            if (difference > 0)
            {
                fine = difference;
                paidAmount = excess;
                excess = 0;
            }
            else
            {
                paidAmount = fine;
                excess = excess - fine;
                fine = 0;
            }

            UoW.FinePayments.Add(new FinePayment()
            {
                Contract = contract,
                Amount = paidAmount,
                CreatedOn = paidDate
            });

            contractFine.Fine = fine;
            contractExcess.Amount = excess;

            UoW.Commit();
        }


        private void RecoverFineByCurrentInstalment(string contractId,DateTime paidDate, ref decimal currentInstalmentAmount)
        {
            var contract = UoW.Contracts.GetSingle(x => x.Id == contractId);

            var contractFine = UoW.ContractFines.GetAll()
                        .Where(x => x.Contract.Id == contractId)
                        .FirstOrDefault();

            if (contractFine == null || contractFine.Fine <= 0)
                return;

            var fine = contractFine.Fine;
            var difference = currentInstalmentAmount - fine;
            var paidAmount = 0.0m;

            if (difference > 0)
            {
                paidAmount = fine;
                contractFine.Fine = 0;
            }
            else
            {
                paidAmount = currentInstalmentAmount;
                contractFine.Fine = fine - currentInstalmentAmount;
            }

            currentInstalmentAmount -= paidAmount;

            UoW.FinePayments.Add(new FinePayment()
            {
                Contract = contract,
                Amount = paidAmount,
                CreatedOn = paidDate
            });

            UoW.Commit();
        }

        private decimal GetExcessOrUnsettleAmountAtCurrentInstalment(string contractId, decimal currentPaymentAmount)
        {
            var contract = ContractsService.GetById(contractId);
            var completedAndPartialyPaidInstalments = this.GetAll()
                                                        .Where(x => x.Paid != (int)InstalmentPaymentStatus.NotPaid && x.Contract.Id == contractId)
                                                        .ToList();

            var finePayment = FinePaymentService.GetAll()
                                .Where(x => x.Contract.Id == contractId);


            var totalPaidFine = finePayment.FirstOrDefault() != null ? finePayment.Sum(x => x.Amount) : 0.0m;

            var expectedTotal = ((completedAndPartialyPaidInstalments != null ? completedAndPartialyPaidInstalments.Count + 1 : 1) * contract.Insallment);
            var paidTotal = completedAndPartialyPaidInstalments != null ? completedAndPartialyPaidInstalments.Sum(x => x.PaidAmount) : 0.0m;

            return ((paidTotal + currentPaymentAmount) - expectedTotal) - totalPaidFine;
        }

        public void AddOrUpdateFine(string contractId, decimal fine)
        {
            var contract = UoW.Contracts.GetSingle(x => x.Id == contractId);
            var contractFine = UoW.ContractFines.GetAll()
                                    .Where(x => x.Contract.Id == contractId)
                                    .FirstOrDefault();

            if (contractFine != null)
            {
                contractFine.Fine += fine;
            }
            else
            {
                UoW.ContractFines.Add(new ContractFine()
                {
                    Contract = contract,
                    Fine = fine
                });
            }

            UoW.Commit();
           
        }

        public void AddOrUpdateExcess(string contractId, decimal excess)
        {
            var contract = UoW.Contracts.GetSingle(x => x.Id == contractId);
            var contractExcess = UoW.Excesses.GetAll()
                                    .Where(x => x.Contract.Id == contractId)
                                    .FirstOrDefault();

            if (contractExcess != null)
            {
                contractExcess.Amount += excess;
            }
            else
            {
                UoW.Excesses.Add(new Excess()
                {
                    Contract = contract,
                    Amount = excess
                });
            }

            UoW.Commit();

        }

        public decimal CalculateFineForCurrentInstalmentAndUpdateExcess(string contractId, DateTime dueDate, DateTime paidDate)
        {
            var contract = UoW.Contracts.GetSingle(x => x.Id == contractId);
            var contractExcess = ExcessService.GetExcessForContract(contract.Id);
            var excess = contractExcess != null ? contractExcess.Amount : 0.0m;

            var fine = CalculateFine(contract.Insallment, dueDate, paidDate);

            return fine;

        }

        public decimal CalculateFineForPreviousUnsettleInstalments(string contractId, DateTime paidDate)
        {
            var fine = 0.0m;

            var partialyPaidInstalments = GetPartialyPaidInstalments(contractId);

            foreach (var item in partialyPaidInstalments)
            {
                var actualUnsettleAmount = item.UnsettleAmount;
                fine += CalculateFine(actualUnsettleAmount, item.DueDate, paidDate);
            }

            return fine;
        }


        private decimal CalculateFine(decimal amount, DateTime dueDate, DateTime paidDate)
        {
            var timeSpan = (paidDate.Date - dueDate.Date).Days;
            var fine = 0.0m;

            if (timeSpan > 7 && timeSpan <= 30)
            {
                fine = amount * 0.1m;
            }
            else if (timeSpan > 30)
            {
                fine = amount * 0.2m;
            }

            return fine;
        }

        public List<ContractInstallment> GetPartialyPaidInstalments(string contractId)
        {
            var partialyPaidInstalments = this.GetAll()
                                            .Where(x => x.Paid == (int)InstalmentPaymentStatus.PartialyPaid && x.Contract.Id == contractId)
                                            .ToList();

            return partialyPaidInstalments;
        }


        public ContractInstallment GetInstalmentToBePaid(string contractId)
        {

            var nextInstalment = this.GetAll()
                                    .Where(x => x.Contract.Id == contractId && x.Paid == (int)InstalmentPaymentStatus.NotPaid)
                                    .OrderBy(x => x.DueDate)
                                    .FirstOrDefault();

            return nextInstalment;
        }


        public List<ContractInstallment> GetInstalmentsForContract(string contractId)
        {
            return this.GetAll().Where(x => x.Contract.Id == contractId).ToList();
        }

        public ContractDetailModel GetContractDetails(string contractId)
        {
            var contract = ContractsService.GetAllWithIncludes(x => x.Customer, c => c.Broker, c => c.Guarantor)
                            .Where(t => t.Id == contractId)
                            .FirstOrDefault();

            var fine = FinePaymentService.GetAll().Where(x => x.Contract.Id == contractId).ToList().Select(x => x.Amount).Sum();

            var instalments = GetInstalmentsForContract(contractId);

            var model = new ContractDetailModel()
            {
                CustomerName = contract.Customer.Name,
                CustomerNIC = contract.Customer.NIC,
                CustomerAddress = contract.Customer.Address,
                CustomerContactNo = contract.Customer.MobileNumber,
                CustomerOccupation = contract.Customer.Occupation,

                BrokerName = contract.Broker != null? contract.Broker.Name : string.Empty,
                BrokerAddress = contract.Broker != null? contract.Broker.Address : string.Empty,
                BrokerContactNo = contract.Broker != null? contract.Broker.ContactNo : string.Empty,
                BrokerNIC = contract.Broker != null? contract.Broker.NIC : string.Empty,
                BrokerOccupation = contract.Broker != null ? contract.Broker.Occupation : string.Empty,

                GuarantorName = contract.Guarantor.Name,
                GuarantorAddress = contract.Guarantor.Address,
                GuarantorContactNo = contract.Guarantor.ContactNo,
                GuarantorNIC = contract.Guarantor.NIC,

                LicenceExpireDate = contract.LicenceExpireDate,
                Instalments = instalments,
                TotalPaidAmount = instalments.Select(x => x.PaidAmount).ToList().Sum(),
                TotalFinePaid = fine
            };

            return model;
        }

        public ContractModel GetContractForInstalment(string contractId)
        {
            var contract = ContractsService.GetById(contractId);

            var result = new ContractModel()
            {
                ContractCreatedOn = contract.CreatedOn,
            };

            return result;
        }

        public ContractInstalmentModel GetCurrentInstalmentDetails(string contractId, DateTime paidDate)
        {
            var currentInstalment = GetInstalmentToBePaid(contractId);

            var model = new ContractInstalmentModel()
            {
                DueDate = currentInstalment.DueDate,
                TotalPayble = GetTotalPaybleAmountForCurrentInstalment(contractId, paidDate)
            };

            return model;
        }


        private decimal GetTotalPaybleAmountForCurrentInstalment(string contractId, DateTime paidDate)
        {
            var contract = UoW.Contracts.GetSingle(x => x.Id == contractId);
            var partialyPaidInstalments = GetPartialyPaidInstalments(contractId);

            var unsettleInstalmentAmount = partialyPaidInstalments!= null ? partialyPaidInstalments.Sum(x => x.UnsettleAmount) : 0.0m;
            var fineForPartialyPaidInstalments = CalculateFineForPreviousUnsettleInstalments(contract.Id, paidDate);
            var previousFine = ContractFineService.GetAll().Sum(x => x.Fine);
            var instalment = contract.Insallment;

            return (unsettleInstalmentAmount + fineForPartialyPaidInstalments + previousFine + instalment);
        }

        public RevenueRecordModel GetRevenueReport(DateTime from, DateTime to)
        {

            var payments = this.GetAll().Where(x => x.PaidDate >= from && x.PaidDate <= to)
                                .Select(x => new RevenueRecord() { AmountWithFine = x.PaidAmount, ContractNo = x.Contract.VehicleNo, Customer = x.Contract.Customer.Name, PaidDate = x.PaidDate })
                                .OrderBy(x => x.PaidDate)
                                .ToList();

            var fine = FinePaymentService.GetAll()
                        .Where(x => x.CreatedOn >= from && x.CreatedOn <= to)
                        .Select(l => l.Amount)
                        .ToList().Sum();

            var result = new RevenueRecordModel()
            {
                 RevenueRecords = payments,
                 TotalWithFine = payments.Select(x => x.AmountWithFine).ToList().Sum(),
                 Fine = fine
            };

            return result;
        }

        public decimal GetRevenue(DateTime from, DateTime to)
        {
            var instalmentPayments = this.GetAll().Where(x => x.PaidDate >= from && x.PaidDate <= to)
                                    .Select(x => x.PaidAmount)
                                    .ToList()
                                    .Sum();

            var contractSettlements = ContractSettlementService.GetAll()
                                        .Where(x => x.CreatedOn >= from && x.CreatedOn <= to)
                                        .Select(x => x.Amount)
                                        .ToList()
                                        .Sum();

            return instalmentPayments + contractSettlements;
        }

        public RevenueRecordModel GetAccruedRevenueReport(DateTime from, DateTime to)
        {
            var contractInstalments = this.GetAllWithIncludes(x=>x.Contract, x => x.Contract.Customer)
                            .Where(x => x.DueDate >= from && x.DueDate <= to)
                            .ToList();

            var paymentsToBePaidList = contractInstalments.Select(x => new RevenueRecord()
            {
                InstalmentAmount = x.Contract.Insallment,
                Customer = x.Contract.Customer.Name,
                Fine = CalculateFine(x.Contract.Insallment, x.DueDate, DateTime.Now) + CalculateFine(x.UnsettleAmount, x.DueDate, DateTime.Now),
                ContractNo = x.Contract.VehicleNo
            }).ToList();


            var result = new RevenueRecordModel()
            {
                RevenueRecords = paymentsToBePaidList,
                TotalInstalmentAmount = paymentsToBePaidList.Sum(x => x.InstalmentAmount),
                Fine = paymentsToBePaidList.Sum(x => x.Fine)
            };

            return result;

        }

        public decimal GetAccuredRevenue(DateTime from, DateTime to)
        {
            var contractInstalments = this.GetAllWithIncludes(x => x.Contract)
                            .Where(x => x.DueDate >= from && x.DueDate <= to)
                            .ToList();

            var accruedRevenue = 0.0m;
            var toDay = DateTime.Now;

            contractInstalments.ForEach(x =>
                accruedRevenue += 
                (x.Contract.Insallment + CalculateFine(x.Contract.Insallment, x.DueDate, toDay) + CalculateFine(x.UnsettleAmount, x.DueDate, toDay))
            );

            return accruedRevenue;
        }


        public List<RevenueRecord> GetInstalmentsList(DateTime from, DateTime to)
        {
            to = to.AddMonths(10);

            var contractInstalments = this.GetAllWithIncludes(x => x.Contract, c => c.Contract.Customer)
                                        .Where(x => x.DueDate >= from && x.DueDate <= to && x.Paid == (int)InstalmentPaymentStatus.NotPaid)
                                        .ToList();

            var revenueRecords = new List<RevenueRecord>();

            contractInstalments.ForEach(x => revenueRecords.Add(new RevenueRecord() { Customer = x.Contract.Customer.Name, InstalmentAmount = x.Contract.Insallment, ContractId = x.Contract.Id }));

            return revenueRecords;
        }


        public List<InstalmentApproveModel> GetInstalmentsToBeApproved()
        {
            var instalments = UoW.ContractInstallments.GetAllWithIncludes(c => c.Contract, c => c.Contract.Customer)
                                .Where(x => !x.Approved && x.Paid != (int)InstalmentPaymentStatus.NotPaid)
                                .OrderBy(x => x.PaidDate)
                                .ToList();

            var instalmentsApprovalList = new List<InstalmentApproveModel>();

            instalments.ForEach(x => instalmentsApprovalList.Add(new InstalmentApproveModel(){
               ContractId           = x.Contract.Id,
               InstalmentId         = x.Id,
               ContractNo           = x.Contract.VehicleNo,
               Customer             = x.Contract.Customer.Name,
               PaidAmount           = x.PaidAmount,
               CustomerContactNo    = x.Contract.Customer.MobileNumber,
               PaidDate             = x.PaidDate
            }));

            return instalmentsApprovalList;
        }

        public bool ApproveInstalment(string instalmentId)
        {
            try
            {
                var instalment = this.GetById(instalmentId);
                instalment.Approved = true;
                this.Update(instalment);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public List<ContractCloseModel> GetContractsToBeClosed()
        {
            var contracts = UoW.Contracts
                            .GetAllWithIncludes(c => c.Customer).Where(x => x.ContractInstallments.All(c => c.Paid != (int)InstalmentPaymentStatus.NotPaid) && x.IsOpen == true)
                            .ToList();

            //var excess = 

            var contractCloseList = new List<ContractCloseModel>();

            foreach (var item in contracts)
            {
                var excess = ExcessService.GetAll().Where(x => x.Contract.Id == item.Id).Sum(y => y.Amount);

                contractCloseList.Add(new ContractCloseModel()
                {
                    ContractId = item.Id,
                    ContractNo = item.VehicleNo,
                    Customer = item.Customer.Name,
                    Loan = item.Amount,
                    Instalments = item.NoOfInstallments,
                    TotalPayble = (GetTotalPaybleAmountForCurrentInstalment(item.Id, DateTime.Now) - item.Insallment - excess),
                    TotalPaidAmount = GetTotalPaidPayment(item.Id)
                });
            }

            return contractCloseList;
        }

        public decimal GetTotalPaidPayment(string contractId)
        {
            return this.GetAll().Where(x => x.Contract.Id == contractId).Sum(c => c.PaidAmount);
        }

        public decimal GetPaybleAtContractClosingDate(string contractId, DateTime closedDate)
        {
            var excess = ExcessService.GetAll().Where(x => x.Contract.Id == contractId).Sum(y => y.Amount);
            var partialyPaidInstalments = GetPartialyPaidInstalments(contractId);

            var unsettleInstalmentAmount = partialyPaidInstalments != null ? partialyPaidInstalments.Sum(x => x.UnsettleAmount) : 0.0m;
            var fineForPartialyPaidInstalments = CalculateFineForPreviousUnsettleInstalments(contractId, closedDate);
            var previousFine = ContractFineService.GetAll().Sum(x => x.Fine);

            return (unsettleInstalmentAmount + fineForPartialyPaidInstalments + previousFine - excess);
        }

        public bool CloseContract(string contractId, decimal settlementAmount, string createdByUserId, string createdByUserName, DateTime closedDate)
        {
            decimal currentPayment = settlementAmount;

            AddToContractSettlement(contractId, settlementAmount, createdByUserId, createdByUserName, closedDate);

            RecoverFineByExcess(contractId, closedDate);
            RecoverFineByCurrentInstalment(contractId, closedDate, ref currentPayment);

            RecoverUnsettledInstalmentsByExcess(contractId);
            RecoverUnsettledInstalmentsByCurrentInstalment(contractId, ref currentPayment);

            var contract = UoW.Contracts.GetSingle(x => x.Id == contractId);
            contract.IsOpen = false;
            UoW.Commit();

            return true;
        }

        public int GetNumberOfInstalments(DateTime from, DateTime to)
        {

            return this.GetAll()
                       .Where(x => x.DueDate >= from && x.DueDate <= to && x.Paid == (int)InstalmentPaymentStatus.NotPaid)
                       .Count();
        }

        public int GetNumberOfContracts(DateTime from, DateTime to)
        {
            return ContractsService.GetAll().Where(x => x.CreatedOn >= from && x.CreatedOn <= to).Count();
        }

        private void AddToContractSettlement(string contractId, decimal settlementAmount, string createdByUserId, string createdByUserName, DateTime closedDate)
        {
            var contract = UoW.Contracts.GetSingle(x => x.Id == contractId);

            var settlement = new ContractSettlement()
            {
                Amount = settlementAmount,
                Contract = contract,
                CreatedOn = closedDate,
                CreatedByUserId = createdByUserId,
                CreatedByUserName = createdByUserName
            };

            UoW.ContractSettlements.Add(settlement);
            UoW.Commit();
        }
      
    }
}