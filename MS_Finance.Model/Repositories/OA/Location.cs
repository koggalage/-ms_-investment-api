using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS_Finance.Model.Repositories.OA
{
    public class Location
    {
        public Location()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        [Index("IX_X_Location", 1, IsUnique = true)]
        [MaxLength(255)]
        public string Code { get; set; }

        public string CreatedByUserId { get; set; }

        public string CreatedByUserName { get; set; }
    }
}
