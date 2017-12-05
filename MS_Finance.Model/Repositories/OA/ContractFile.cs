using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS_Finance.Model.Repositories.OA
{
    public class ContractFile
    {
        public ContractFile()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        public virtual Contract Contract { get; set; }

        public string FilePath { get; set; }

        public string CreatedByUserId { get; set; }

        public string CreatedByUserName { get; set; }

        public DateTime CreatedOn { get; set; }

    }
}
