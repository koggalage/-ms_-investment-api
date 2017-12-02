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
    public class ContractSettlementService : DefaultPersistentService<ContractSettlement>, IContractSettlementService
    {
        public ContractSettlementService(IUnitOfWork UoW)
            : base(UoW)
        {
            
        }


        public IQueryable<ContractSettlement> GetAll()
        {
            return base.GetAll();
        }

        public ContractSettlement GetById(string id)
        {
            return base.GetSingle(x => x.Id == id);
        }

        public void Create(ContractSettlement settlement)
        {
            base.Add(settlement);
        }

        public void Update(ContractSettlement settlement)
        {
            base.Update(settlement);
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
