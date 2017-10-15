﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS_Finance.Model.Models
{
    public class ContractInstalmentModel
    {

        public decimal PaidAmount { get; set; }

        public decimal UnsettleAmount { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime PaidDate { get; set; }

        public int LateDays { get; set; }

        public decimal Fine { get; set; }

        public int ContractId { get; set; }
    }
}