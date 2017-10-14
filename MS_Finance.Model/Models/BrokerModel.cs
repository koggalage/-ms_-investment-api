using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS_Finance.Model.Models
{
    public class BrokerModel
    {
        public string Name { get; set; }

        public string NIC { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Address { get; set; }

        public string ContactNo { get; set; }

        public string Occupation { get; set; }
    }
}
