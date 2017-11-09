using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MS_Finance.Business.Interfaces
{
    public interface IDefaultPersistentService<T>
    {
        IQueryable<T> GetAll();

        T GetSingle(Expression<Func<T, bool>> predicate);

        //T CreateTrackedInstance();

        void Add(T entity);

        void Update(T entity);

        void Delete(T entity);

        void Attach(T entity);
    }
}
