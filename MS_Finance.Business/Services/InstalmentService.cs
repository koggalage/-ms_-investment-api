using MS_Finance.Business.Interfaces;
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
    public class InstalmentService : DefaultPersistentService<ContractInstallment>,  IInstalmentService
    {

        protected IContractsService ContractsService;

        public InstalmentService(IUnitOfWork UoW,
            IContractsService ContractsService)
            :base(UoW)
        {
            this.ContractsService = ContractsService;
        }

        public IList<ContractInstallment> GetAll()
        {
            return base
                .GetAll()
                .ToList();
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

            var contract = ContractsService.GetById(instalmentModel.ContractId);

            var instalment = new ContractInstallment()
            {
                PaidAmount = instalmentModel.PaidAmount,
                DueDate = instalmentModel.DueDate,
                PaidDate = instalmentModel.PaidDate,
                LateDays = instalmentModel.LateDays,
                Fine = instalmentModel.Fine,
                UnsettleAmount = instalmentModel.UnsettleAmount,
                Contract = contract,
                CreatedByUserId = instalmentModel.CreatedByUserId,
                CreatedByUserName = instalmentModel.CreatedByUserName
            };

            base.Add(instalment);

            return true;
        }

        public List<ContractInstallment> GetInstalmentsForContract(string contractId)
        {
            return this.GetAll().Where(x => x.Contract.Id == contractId).ToList();
        }


        //private IInstalmentRepository _instalmentRepository;

        //public InstalmentService(InstalmentRepository instalmentRepository)
        //{
        //    this._instalmentRepository = instalmentRepository;
        //}

        //public bool CreateInstalment(ContractInstalmentModel instalmentModel)
        //{
        //    _instalmentRepository.CreateInstalment(instalmentModel);

        //    return true;
        //}

        //public List<ContractInstallment> GetInstalmentsForContract(string contractId)
        //{
        //    return _instalmentRepository.GetInstalmentsForContract(contractId);
        //}
    }
}