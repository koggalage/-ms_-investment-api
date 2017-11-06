using MS_Finance.Business.Interfaces;
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
    public class GuarantorService : IGuarantorService
    {
        private IGuarantorRepository _guarantorRepository;

        public GuarantorService(GuarantorRepository guarantorRepository)
        {
            this._guarantorRepository = guarantorRepository;
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
                CreatedDate = DateTime.Now
            };

            _guarantorRepository.CreateGuarantor(guarantor);

            return true;
        }
    }
}