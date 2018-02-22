using MS_Finance.Model.Models;
using MS_Finance.Model.Repositories.OA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MS_Finance.Business.Interfaces
{
    public interface ILocationsService
    {
        IQueryable<Location> GetAll();

        IQueryable<Location> GetAllWithIncludes(params Expression<Func<Location, object>>[] properties);

        Location GetById(string id);

        void Create(Location contract);

        void Update(Location contract);

        bool CreateNewLocation(ContractLocationModel model);

        List<ContractLocationModel> GetAllLocations();
    }
}
