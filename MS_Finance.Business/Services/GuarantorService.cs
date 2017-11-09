using MS_Finance.Business.Interfaces;
using MS_Finance.Business.Services;
using MS_Finance.Model.Models;
using MS_Finance.Model.Repositories.Interfaces;
using MS_Finance.Model.Repositories.OA;
using MS_Finance.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MS_Finance.Services
{
    public class GuarantorService : DefaultPersistentService<Guarantor>, IGuarantorService
    {

        public GuarantorService(IUnitOfWork UoW)
            : base(UoW)
        {

        }


        public IList<Guarantor> GetAll()
        {
            return base
                .GetAll()
                .ToList();
        }

        public Guarantor GetById(string id)
        {
            return base.GetSingle(x => x.Id == id);
        }

        public void Create(Guarantor guarantor)
        {
            base.Add(guarantor);
        }

        public void Update(Guarantor guarantor)
        {
            base.Update(guarantor);
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public bool CreateGuarantor(GuarantorModel guarantorModel)
        {
            var guarantor = new Guarantor()
            {
                Name = guarantorModel.Name,
                Address = guarantorModel.Address,
                ContactNo = guarantorModel.ContactNo,
                NIC = guarantorModel.NIC,
                Occupation = guarantorModel.Occupation,
                CreatedDate = DateTime.Now,
                CreatedByUserId = guarantorModel.CreatedByUserId,
                CreatedByUserName = guarantorModel.CreatedByUserName
            };

            base.Add(guarantor);

            return true;
        }
    }
}