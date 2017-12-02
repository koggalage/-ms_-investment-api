using MS_Finance.Model.Repositories.OA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS_Finance.Business.Interfaces
{
    public interface IExcessService
    {
        IQueryable<Excess> GetAll();

        Excess GetById(string id);

        void Create(Excess excess);

        void Update(Excess excess);

        void Attach(Excess excess);

        Excess GetExcessForContract(string contractId);
    }
}
