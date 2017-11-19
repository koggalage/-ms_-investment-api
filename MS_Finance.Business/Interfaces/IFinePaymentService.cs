using MS_Finance.Model.Repositories.OA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS_Finance.Business.Interfaces
{
    public interface IFinePaymentService
    {
        IQueryable<FinePayment> GetAll();

        FinePayment GetById(string id);

        void Create(FinePayment payment);

        void Update(FinePayment payment);

        void Attach(FinePayment payment);
    }
}
