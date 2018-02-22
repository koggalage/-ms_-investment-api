using MS_Finance.Business.Interfaces;
using MS_Finance.Model.Models;
using MS_Finance.Model.Repositories.Interfaces;
using MS_Finance.Model.Repositories.OA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MS_Finance.Business.Services
{
    public class LocationsService : DefaultPersistentService<Location>, ILocationsService
    {
        public LocationsService(IUnitOfWork UoW)
            : base(UoW)
        {

        }

        public IQueryable<Location> GetAll()
        {
            return base
                .GetAll();
        }

        public IQueryable<Location> GetAllWithIncludes(params Expression<Func<Location, object>>[] properties)
        {
            return base.GetAllWithIncludes(properties);
        }

        public Location GetById(string id)
        {
            return base.GetSingle(x => x.Id == id);
        }

        public void Create(Location contract)
        {
            base.Add(contract);
        }

        public void Update(Location contract)
        {
            base.Update(contract);
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public bool CreateNewLocation(ContractLocationModel model)
        {
            try
            {
                this.Add(new Location()
                {
                    Name = model.Location,
                    Code = model.Code
                });
            }
            catch (Exception ex)
            {
                
                throw;
            }

            return true;
        }

        public List<ContractLocationModel> GetAllLocations()
        {
            return this.GetAll()
                .Select(x => new ContractLocationModel() { Location = x.Name, Code = x.Code})
                .ToList();
        }
    }
}
