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
    public class FinePaymentService : DefaultPersistentService<FinePayment>, IFinePaymentService
    {
        public FinePaymentService(IUnitOfWork UoW)
            :base(UoW)
        {

        }

        public IQueryable<FinePayment> GetAll()
        {
            return base.GetAll();
        }

        public FinePayment GetById(string id)
        {
            return base.GetSingle(x => x.Id == id);
        }

        public void Create(FinePayment payment)
        {
            base.Add(payment);
        }

        public void Update(FinePayment payment)
        {
            base.Update(payment);
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
