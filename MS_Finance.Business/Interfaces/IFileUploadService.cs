using MS_Finance.Model.Repositories.OA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS_Finance.Business.Interfaces
{
    public interface IFileUploadService
    {
        IQueryable<ContractFile> GetAll();

        ContractFile GetById(string id);

        void Create(ContractFile file);

        void Update(ContractFile file);
    }
}
