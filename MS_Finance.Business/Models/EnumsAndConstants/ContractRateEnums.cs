using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS_Finance.Business.Models.EnumsAndConstants
{
    public enum ContractRateType
    {
        [Description("Interest For Less Than Six Months")]
        InterestForShortTerm = 1,

        [Description("Interest For Greater Than Six Months")]
        InterestForLongTerm = 2,

        [Description("Fine For Greater Than Seven Days")]
        FineForShortTerm = 3,

        [Description("Fine For Greater Thirty Days")]
        FineForLongTerm = 4,

        [Description("Document Charges")]
        DocumentCharges = 5
    }

    public enum RateCatagory
    {
        Interest = 1,
        Fine = 2,
        DocumentCharge = 3
    }
}
