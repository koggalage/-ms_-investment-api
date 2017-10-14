using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS_Finance.Model.Repositories.OA
{
    public class Guarantor
    {

        public Guarantor()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Address { get; set; }

        public string ContactNo { get; set; }

        public string Occupation { get; set; }
    }
}
