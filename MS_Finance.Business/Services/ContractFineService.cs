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
    public class ContractFineService : DefaultPersistentService<ContractFine>, IContractFineService
    {
        public ContractFineService(IUnitOfWork UoW)
            :base(UoW)
        {

        }

        public IList<ContractFine> GetAll()
        {
            return base.GetAll().ToList();
        }

        public ContractFine GetById(string id)
        {
            return base.GetSingle(x => x.Id == id);
        }

        public void Create(ContractFine fine)
        {
            base.Add(fine);
        }

        public void Update(ContractFine fine)
        {
            base.Update(fine);
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public ContractFine GetContractFineForContract(string contractId)
        {
            return base.GetAll()
                .Where(x => x.Contract.Id == contractId)
                .FirstOrDefault();
        }

    }
}
