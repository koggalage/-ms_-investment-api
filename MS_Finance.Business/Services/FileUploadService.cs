using MS_Finance.Business.Interfaces;
using MS_Finance.Model.Repositories.Interfaces;
using MS_Finance.Model.Repositories.OA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS_Finance.Business.Services
{
    public class FileUploadService : DefaultPersistentService<ContractFile>, IFileUploadService
    {
        public FileUploadService(IUnitOfWork UoW)
            :base(UoW)
        {

        }

        public IQueryable<ContractFile> GetAll()
        {
            return base.GetAll();
        }

        public ContractFile GetById(string id)
        {
            return base.GetSingle(x => x.Id == id);
        }

        public void Create(ContractFile file)
        {
            base.Add(file);
        }

        public void Update(ContractFile file)
        {
            base.Update(file);
        }
    }
}
