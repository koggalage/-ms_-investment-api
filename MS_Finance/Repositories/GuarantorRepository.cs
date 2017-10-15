﻿using MS_Finance.Model.Repositories.OA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MS_Finance.Repositories
{
    public class GuarantorRepository
    {
        AuthContext _context;

        public GuarantorRepository()
        {
            _context = new AuthContext();
        }

        public void CreateGuarantor(Guarantor guarantor) 
        {
            _context.Guarantors.Add(guarantor);
            _context.SaveChanges();
        }

        public Guarantor GetSingle(string id)
        {
            return _context.Guarantors.Where(x => x.Id == id).FirstOrDefault();
        }
    }
}