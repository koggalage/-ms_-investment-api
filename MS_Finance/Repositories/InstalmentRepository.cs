﻿using MS_Finance.Model.Models;
using MS_Finance.Model.Repositories.OA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MS_Finance.Repositories
{
    public class InstalmentRepository
    {
        AuthContext _context;

        public InstalmentRepository()
        {
            _context = new AuthContext();
        }

        public void CreateInstalment(ContractInstalmentModel instalmentModel) 
        {
            var contract = _context.Contracts.Where(x => x.Id == instalmentModel.ContractId).FirstOrDefault();

            var instalment = new ContractInstallment()
            {
                PaidAmount = instalmentModel.PaidAmount,
                DueDate = instalmentModel.DueDate,
                PaidDate = instalmentModel.PaidDate,
                LateDays = instalmentModel.LateDays,
                Fine = instalmentModel.Fine,
                UnsettleAmount = instalmentModel.UnsettleAmount
            };

            _context.ContractInstallments.Add(instalment);

            if (contract != null)
            {
                _context.Contracts.Attach(contract);
            }

            instalment.Contract = contract;
            _context.SaveChanges();
        }

        public List<ContractInstallment> GetInstalmentsForContract(string contractId) 
        {
            return _context.ContractInstallments.Where(x => x.Id == contractId).ToList();
        }
    }
}