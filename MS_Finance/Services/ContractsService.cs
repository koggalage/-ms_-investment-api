using MS_Finance.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MS_Finance.Services
{
    public class ContractsService
    {
        private ContractsRepository _contractsRepository;

        public ContractsService()
        {
            _contractsRepository = new ContractsRepository();
        }
    }
}