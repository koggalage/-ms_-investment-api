using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS_Finance.Model.Models
{

    public class RevenueRecordModel
    {
        public List<RevenueRecord> RevenueRecords { get; set; }

        public decimal TotalWithFine { get; set; }

        public decimal Fine { get; set; }

        public decimal TotalInstalmentAmount { get; set; }
    }

    public class RevenueRecord
    {
        public string Customer { get; set; }

        public string ContractId { get; set; }

        public string ContractNo { get; set; }

        public decimal InstalmentAmount { get; set; }

        public decimal  AmountWithFine { get; set; }

        public decimal Fine { get; set; }

        public DateTime? PaidDate { get; set; }
    }
}
