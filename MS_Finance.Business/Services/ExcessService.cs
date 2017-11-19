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
    public class ExcessService : DefaultPersistentService<Excess>, IExcessService
    {
        public ExcessService(IUnitOfWork UoW)
            :base(UoW)
        {

        }

        public IList<Excess> GetAll()
        {
            return base.GetAll().ToList();
        }

        public Excess GetById(string id)
        {
            return base.GetSingle(x => x.Id == id);
        }

        public void Create(Excess excess)
        {
            base.Add(excess);
        }

        public void Update(Excess excess)
        {
            base.Update(excess);
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Excess GetExcessForContract(string contractId)
        {
            return base.GetAll()
                        .Where(x => x.Contract.Id == contractId)
                        .FirstOrDefault();
        }
    }
}
