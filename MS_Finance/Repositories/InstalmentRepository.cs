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

        public void CreateInstalment(ContractInstallment instalment) 
        {
            _context.ContractInstallments.Add(instalment);
            _context.SaveChanges();
        }
    }
}