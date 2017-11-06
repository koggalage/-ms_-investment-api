using MS_Finance.Business.Interfaces;
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
    public class InstalmentService : IInstalmentService
    {
        private IInstalmentRepository _instalmentRepository;

        public InstalmentService(InstalmentRepository instalmentRepository)
        {
            this._instalmentRepository = instalmentRepository;
        }

        public bool CreateInstalment(ContractInstalmentModel instalmentModel)
        {
            _instalmentRepository.CreateInstalment(instalmentModel);

            return true;
        }

        public List<ContractInstallment> GetInstalmentsForContract(string contractId)
        {
            return _instalmentRepository.GetInstalmentsForContract(contractId);
        }
    }
}