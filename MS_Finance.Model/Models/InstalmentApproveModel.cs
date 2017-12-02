using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS_Finance.Model.Models
{
    public class InstalmentApproveModel
    {
        public string ContractId { get; set; }

        public string InstalmentId { get; set; }

        public string ContractNo { get; set; }  

        public string Customer { get; set; }

        public decimal PaidAmount { get; set; }

        public DateTime? PaidDate { get; set; }

        public string CustomerContactNo { get; set; }
    }
}
