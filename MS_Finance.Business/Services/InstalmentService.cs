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

        public InstalmentService(IUnitOfWork UoW,
            IContractsService ContractsService,
            IContractFineService ContractFineService,
            IExcessService ExcessService,
            IFinePaymentService FinePaymentService)
            :base(UoW)
        {
            this.ContractsService = ContractsService;
            this.ContractFineService = ContractFineService;
            this.ExcessService = ExcessService;
            this.FinePaymentService = FinePaymentService;
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
            var fineForPreviousUnsettleInstalments = CalculateFineForPreviousUnsettleInstalments(contract, instalmentModel.PaidDate);

            decimal actualCurrentInstalmentPayment  = instalmentModel.PaidAmount;
            
            decimal totalFineForCurrentAndPreviousUnsettleInstalments = fineForCurrentInstalment + fineForPreviousUnsettleInstalments;

            if (totalFineForCurrentAndPreviousUnsettleInstalments > 0)
                AddOrUpdateFine(contract.Id, totalFineForCurrentAndPreviousUnsettleInstalments);

            RecoverFineByExcess(instalmentModel.ContractId);
            RecoverFineByCurrentInstalment(instalmentModel.ContractId, ref actualCurrentInstalmentPayment);

            RecoverUnsettledInstalmentsByExcess(instalmentModel.ContractId);
            RecoverUnsettledInstalmentsByCurrentInstalment(instalmentModel.ContractId, ref actualCurrentInstalmentPayment);

            decimal excessOrUnsettleAmount = GetExcessOrUnsettleAmountAtCurrentInstalment(contract.Id, actualCurrentInstalmentPayment);

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


            base.Update(instalmentToBePaid);

            return true;
        }


        private void RecoverUnsettledInstalmentsByCurrentInstalment(string contractId, ref decimal currentInstalmentAmount)
        {
            var partialyPaidInstalments = UoW.ContractInstallments.GetAll()
                                            .Where(x => x.Paid == (int)InstalmentPaymentStatus.PartialyPaid)
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
                                .Where(x => x.Paid == (int)InstalmentPaymentStatus.PartialyPaid)
                                .ToList();

            var contractExcess = UoW.Excesses.GetAll()
                                    .Where(x => x.Contract.Id == contractId)
                                    .FirstOrDefault();

            if (contractExcess == null || contractExcess.Amount <= 0)
                return;

            var excess = contractExcess.Amount;

            foreach (var item in partialyPaidInstalments)
            {
                var unsettleAmount = item.UnsettleAmount;
                var difference = excess - unsettleAmount;

                if (difference > 0)
                {
                    item.UnsettleAmount = 0;
                }
                else
                {
                    item.UnsettleAmount = unsettleAmount - excess;
                }
            }

            UoW.Commit();
        }



        private void RecoverFineByExcess(string contractId)
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

            if (contractFine == null || contractExcess == null)
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
                Amount = paidAmount
            });

            contractFine.Fine = fine;
            contractExcess.Amount = excess;

            UoW.Commit();
        }


        private void RecoverFineByCurrentInstalment(string contractId, ref decimal currentInstalmentAmount)
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
                Amount = paidAmount
            });

            UoW.Commit();
        }

        private decimal GetExcessOrUnsettleAmountAtCurrentInstalment(string contractId, decimal currentPaymentAmount)
        {
            var contract = ContractsService.GetById(contractId);
            var completedAndPartialyPaidInstalments = this.GetAll()
                                                        .Where(x => x.Paid != (int)InstalmentPaymentStatus.NotPaid)
                                                        .ToList();

            var totalPaidFine = FinePaymentService.GetAll()
                                .Where(x => x.Contract.Id == contractId)
                                .Sum(x => x.Amount);

            var expectedTotal = (completedAndPartialyPaidInstalments != null ? completedAndPartialyPaidInstalments.Count + 1 : 1) * contract.Insallment;
            var paidTotal = completedAndPartialyPaidInstalments != null ? completedAndPartialyPaidInstalments.Sum(x => x.PaidAmount) : 0.0m;

            return ((paidTotal) + currentPaymentAmount) - expectedTotal;
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

        public decimal CalculateFineForPreviousUnsettleInstalments(Contract contract, DateTime paidDate)
        {
            var fine = 0.0m;

            var partialyPaidInstalments = GetPartialyPaidInstalments(contract.Id);

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
                                            .Where(x => x.Paid == (int)InstalmentPaymentStatus.PartialyPaid)
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
                TotalPayble = GetTotalPaybleAmount(contractId, paidDate)
            };

            return model;
        }


        public decimal GetTotalPaybleAmount(string contractId, DateTime paidDate)
        {
            var contract = UoW.Contracts.GetSingle(x => x.Id == contractId);
            var partialyPaidInstalments = GetPartialyPaidInstalments(contractId);

            var unsettleInstalmentAmount = partialyPaidInstalments!= null ? partialyPaidInstalments.Sum(x => x.UnsettleAmount) : 0.0m;
            var fineForPartialyPaidInstalments = CalculateFineForPreviousUnsettleInstalments(contract, paidDate);
            var previousFine = ContractFineService.GetAll().Sum(x => x.Fine);
            var instalment = contract.Insallment;

            return (unsettleInstalmentAmount + fineForPartialyPaidInstalments + previousFine + instalment);
        }
    }
}