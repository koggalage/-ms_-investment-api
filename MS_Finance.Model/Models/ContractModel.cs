using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS_Finance.Model.Models
{
    public class ContractModel
    {
        public string Id { get; set; }

        public string ContractNo { get; set; }

        public string  CustomerId { get; set; }

        public decimal Amount { get; set; }

        public int NoOfInstallments { get; set; }

        public decimal Insallment { get; set; }

        public int Type { get; set; }

        public string VehicleNo { get; set; }

        public string GuarantorId { get; set; }

        public string BrokerId { get; set; }

        public string CustomerName { get; set; }

        public string CreatedByUserId { get; set; }

        public string CreatedByUserName { get; set; }
    }
}
