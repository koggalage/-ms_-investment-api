using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS_Finance.Model.Repositories.OA
{
    public class ContractRate
    {
        public ContractRate()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        public int Type { get; set; }

        public string Description { get; set; }

        public decimal Value { get; set; }

        public string CreatedByUserId { get; set; }

        public string CreatedByUserName { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ValidFrom { get; set; }

        public DateTime? ValidUntil { get; set; }
    }
}
