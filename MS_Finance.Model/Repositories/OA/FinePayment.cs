using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS_Finance.Model.Repositories.OA
{
    public class FinePayment
    {

        public FinePayment()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        public decimal Amount { get; set; }

        public virtual Contract Contract { get; set; }

    }

}
