using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS_Finance.Model.Repositories.OA
{
    public class Contract
    {

        public Contract()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        public virtual Customer Customer { get; set; }

        public string ContractNo { get; set; }

        public decimal Amount { get; set; }

        public int NoOfInstallments { get; set; }

        public decimal Insallment { get; set; }

        public int Type { get; set; }

        public string VehicleNo { get; set; }

        public virtual Guarantor Guarantor { get; set; }

        public virtual Broker Broker { get; set; }

        public bool IsOpen { get; set; }
    }
}
