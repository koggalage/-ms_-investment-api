using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS_Finance.Model.Models
{
    public class ContractRateModel
    {
        public int Type { get; set; }

        public string Description { get; set; }

        public decimal Value { get; set; }

        public DateTime ValidFrom { get; set; }

        public DateTime? ValidUntil { get; set; }

        public string CreatedByUserId { get; set; }

        public string CreatedByUserName { get; set; }
    }
}
