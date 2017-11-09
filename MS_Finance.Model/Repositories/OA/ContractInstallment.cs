using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS_Finance.Model.Repositories.OA
{
    public class ContractInstallment
    {
        public ContractInstallment()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        public decimal PaidAmount { get; set; }

        public decimal UnsettleAmount { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime PaidDate { get; set; }

        public int LateDays { get; set; }

        public decimal Fine { get; set; }

        public virtual Contract Contract { get; set; }

        public string CreatedByUserId { get; set; }

        public string CreatedByUserName { get; set; }
    }
}
