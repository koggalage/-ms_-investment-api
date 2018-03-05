using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace MS_Finance.Model.Repositories.Interfaces
{
    public interface IRepository<T>
    {
        IQueryable<T> GetAll();

        IQueryable<T> GetAllWithIncludes(params Expression<Func<T, object>>[] properties);

        T GetSingle(Expression<Func<T, bool>> predicate);

        //T CreateTrackedInstance();

        void Add(T entity);

        void Update(T entity);

        void Delete(T entity);

        void Attach(T entity);

        void AddRange(IEnumerable<T> entities);
    }
}
