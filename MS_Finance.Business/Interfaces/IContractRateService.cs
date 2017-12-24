using MS_Finance.Model.Models;
using MS_Finance.Model.Repositories.OA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS_Finance.Business.Interfaces
{
    public interface IContractRateService
    {
        IQueryable<ContractRate> GetAll();

        ContractRate GetById(string id);

        void Create(ContractRate fine);

        void Update(ContractRate fine);

        void ObsoletePreviousAndAddNewContractRate(ContractRateModel rateModel);

        List<ContractRateModel> GetRates(int type);
    }
}
