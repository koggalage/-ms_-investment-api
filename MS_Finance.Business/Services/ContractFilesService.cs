using MS_Finance.Business.Interfaces;
using MS_Finance.Model.Repositories.Interfaces;
using MS_Finance.Model.Repositories.OA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS_Finance.Business.Services
{
    public class ContractFilesService : DefaultPersistentService<ContractFile>, IContractFilesService
    {
        public ContractFilesService(IUnitOfWork UoW)
            :base(UoW)
        {

        }

        public IQueryable<ContractFile> GetAll()
        {
            return base.GetAll();
        }
    }
}
