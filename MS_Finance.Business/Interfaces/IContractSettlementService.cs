using MS_Finance.Model.Repositories.OA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS_Finance.Business.Interfaces
{
    public interface IContractSettlementService
    {
        IQueryable<ContractSettlement> GetAll();

        ContractSettlement GetById(string id);

        void Create(ContractSettlement settlement);

        void Update(ContractSettlement settlement);
    }
}
