using MS_Finance.Business.Interfaces;
using MS_Finance.Business.Models.EnumsAndConstants;
using MS_Finance.Model.Models;
using MS_Finance.Model.Repositories.Interfaces;
using MS_Finance.Model.Repositories.OA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS_Finance.Business.Services
{
    public class ContractRateService : DefaultPersistentService<ContractRate>, IContractRateService
    {
        public ContractRateService(IUnitOfWork UoW)
            : base(UoW)
        {

        }

        public IQueryable<ContractRate> GetAll()
        {
            return base.GetAll();
        }

        public ContractRate GetById(string id)
        {
            return base.GetSingle(x => x.Id == id);
        }

        public void Create(ContractRate fine)
        {
            base.Add(fine);
        }

        public void Update(ContractRate fine)
        {
            base.Update(fine);
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public void ObsoletePreviousAndAddNewContractRate(ContractRateModel rateModel)
        {
            var previousRate = this.GetAll().OrderByDescending(x => x.CreatedOn).FirstOrDefault(x => x.Type == rateModel.Type);

            if (previousRate != null)
            {
                previousRate.ValidUntil = DateTime.Now;
                rateModel.ValidFrom = DateTime.Now.AddDays(1);
                this.Update(previousRate);
            }
            else
            {
                rateModel.ValidFrom = DateTime.Now;
            }

            var coantractRate = new ContractRate()
            {
                Type                = rateModel.Type,
                ValidFrom           = rateModel.ValidFrom,
                ValidUntil          = rateModel.ValidUntil,
                Value               = rateModel.Value,
                Description         = ((ContractRateType)rateModel.Type).GetDescription(),
                CreatedOn           = DateTime.Now,
                CreatedByUserId     = rateModel.CreatedByUserId,
                CreatedByUserName   = rateModel.CreatedByUserName
            };

            this.Add(coantractRate);
        }


        public List<ContractRateModel> GetRates(int type)
        {
            var rates = this.GetAll().Where(x =>
                (type == (int)RateCatagory.Interest && (x.Type == (int)ContractRateType.InterestForShortTerm || x.Type == (int)ContractRateType.InterestForLongTerm)) ||
                (type == (int)RateCatagory.Fine && (x.Type == (int)ContractRateType.FineForShortTerm || x.Type == (int)ContractRateType.FineForLongTerm)) ||
                (type == (int)RateCatagory.DocumentCharge && (x.Type == (int)ContractRateType.DocumentCharges ))
                ).OrderByDescending(c => c.ValidFrom).ToList();
            var rateModelList = new List<ContractRateModel>();


            rates.ForEach(x =>
                rateModelList.Add(new ContractRateModel() { 
                    Description = x.Description,
                    Type        = x.Type,
                    ValidFrom   = x.ValidFrom,
                    ValidUntil  = x.ValidUntil,
                    Value       = x.Value
                })
            );

            return rateModelList;
        }

    }
}
