﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MS_Finance.Repositories
{
    public class ContractsRepository
    {
        AuthContext _context;

        public ContractsRepository()
        {
            _context = new AuthContext();
        }


    }
}