using MS_Finance.Model.Models;
using MS_Finance.Model.Repositories.OA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS_Finance.Business.Interfaces
{
    public interface IInstalmentService
    {

        //IList<ContractInstallment> GetAll();

        //ContractInstallment GetById(Guid id);

        //void Create(ContractInstallment installment);

        //void Update(ContractInstallment installment);

        //void Delete(Guid id);

        bool CreateInstalment(ContractInstalmentModel instalmentModel);

        List<ContractInstallment> GetInstalmentsForContract(string contractId);
    }
}
