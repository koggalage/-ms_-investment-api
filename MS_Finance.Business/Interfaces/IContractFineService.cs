using MS_Finance.Model.Repositories.OA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS_Finance.Business.Interfaces
{
    public interface IContractFineService
    {
        IQueryable<ContractFine> GetAll();

        ContractFine GetById(string id);

        void Create(ContractFine fine);

        void Update(ContractFine fine);

        void Attach(ContractFine fine);

        ContractFine GetContractFineForContract(string contractId);
    }
}
