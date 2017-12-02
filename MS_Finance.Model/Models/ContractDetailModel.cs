using MS_Finance.Model.Repositories.OA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS_Finance.Model.Models
{
    public class ContractDetailModel
    {
        public List<ContractInstallment> Instalments { get; set; }

        public DateTime LicenceExpireDate { get; set; }

        public decimal TotalPaidAmount { get; set; }

        public decimal TotalFinePaid { get; set; }


        public string CustomerName { get; set; }

        public string CustomerNIC { get; set; }

        public string CustomerAddress { get; set; }

        public string CustomerContactNo { get; set; }

        public string CustomerOccupation { get; set; }



        public string GuarantorName { get; set; }

        public string GuarantorNIC { get; set; }

        public string GuarantorAddress { get; set; }

        public string GuarantorContactNo { get; set; }


        public string BrokerName { get; set; }

        public string BrokerNIC { get; set; }

        public string BrokerAddress { get; set; }

        public string BrokerContactNo { get; set; }

        public string BrokerOccupation { get; set; }

    }
}
