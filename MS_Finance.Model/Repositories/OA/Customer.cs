using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        [Index("IX_X_CustomerNIC", 1, IsUnique = true)]
        [MaxLength(255)]
        public string NIC { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Address { get; set; }

        public string MobileNumber { get; set; }

        public string Occupation { get; set; }

        public string CreatedByUserId { get; set; }

        public string CreatedByUserName { get; set; }

        public string Description { get; set; }

        public virtual List<Contract> Contracts { get; set; }
    }
}
