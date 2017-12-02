using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS_Finance.Model.Repositories.OA
{
    public class ContractSettlement
    {
        public ContractSettlement()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        public decimal Amount { get; set; }

        public string CreatedByUserId { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedByUserName { get; set; }

        public virtual Contract Contract { get; set; }
    }
}
