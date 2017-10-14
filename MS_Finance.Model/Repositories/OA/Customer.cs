using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS_Finance.Model.Repositories.OA
{
    public class Customer
    {
        public Customer()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string NIC { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Address { get; set; }

        public string MobileNumber { get; set; }

        public string Occupation { get; set; }
    }
}
