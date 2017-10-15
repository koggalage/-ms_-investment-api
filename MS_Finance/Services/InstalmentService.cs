using MS_Finance.Model.Models;
using MS_Finance.Model.Repositories.OA;
using MS_Finance.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MS_Finance.Services
{
    public class InstalmentService
    {
        private InstalmentRepository _instalmentRepository;

        public InstalmentService()
        {
            _instalmentRepository = new InstalmentRepository();
        }

        public bool CreateInstalment(ContractInstalmentModel instalmentModel)
        {
            var instalment = new ContractInstallment()
            {
                PaidAmount = instalmentModel.PaidAmount,
                //Contract =  instalmentModel.Address,
                DueDate = instalmentModel.DueDate,
                PaidDate = instalmentModel.PaidDate,
                LateDays = instalmentModel.LateDays,
                Fine = instalmentModel.Fine,
                UnsettleAmount = instalmentModel.UnsettleAmount
            };

            _instalmentRepository.CreateInstalment(instalment);

            return true;
        }

        public List<ContractInstallment> GetInstalmentsForContract(string contractId)
        {
            return _instalmentRepository.GetInstalmentsForContract(contractId);
        }
    }
}