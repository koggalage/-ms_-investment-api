using MS_Finance.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS_Finance.Business.Interfaces
{
    public interface IGuarantorService
    {
        bool CreateGuarantor(GuarantorModel guarantorModel);
    }
}
