using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS_Finance.Model.Models
{
    public class ContractCloseModel
    {
        public string ContractId { get; set; }

        public string Customer { get; set; }

        public string ContractNo { get; set; }

        public decimal Loan { get; set; }

        public decimal TotalPaidFine { get; set; }

        public decimal TotalPaidAmount { get; set; }

        public decimal TotalPayble { get; set; }

        public int Instalments { get; set; }



        public decimal SettlementAmount { get; set; }

        public DateTime ClosedDate { get; set; }
    }
}
