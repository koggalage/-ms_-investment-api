using MS_Finance.Business.Interfaces;
using MS_Finance.Model.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace MS_Finance.Business.Services
{
    public abstract class DefaultPersistentService
    {
        protected IUnitOfWork UoW;

        public DefaultPersistentService(IUnitOfWork UoW)
        {
            this.UoW = UoW;
        }

    }

    public abstract class DefaultPersistentService<T> : DefaultPersistentService, IDefaultPersistentService<T> where T : new()
    {

        public DefaultPersistentService(IUnitOfWork UoW)
            :base(UoW)
        {
            this.UoW = UoW;
        }

        public virtual IQueryable<T> GetAll()
        {
            return UoW.GetRepository<T>().GetAll();
        }

        public virtual T GetSingle(Expression<Func<T, bool>> predicate)
        {
            return UoW.GetRepository<T>().GetSingle(predicate);
        }

        public virtual void Add(T entity)
        {
            UoW.GetRepository<T>().Add(entity);
            UoW.Commit();
        }

        public virtual void Update(T entity)
        {
            UoW.GetRepository<T>().Update(entity);
            UoW.Commit();
        }

        public virtual void Delete(T entity)
        {
            UoW.GetRepository<T>().Delete(entity);
        }

        public virtual void Attach(T entity)
        {
            UoW.GetRepository<T>().Attach(entity);
        }

    }

}
