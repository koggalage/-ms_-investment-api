using MS_Finance.Model.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Linq.Expressions;

namespace MS_Finance.Model.Repositories.OA
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        protected MSDataContext Context;
        protected readonly DbSet<T> _DbSet;

        public BaseRepository(MSDataContext context)
        {
            this.Context = context;
            _DbSet = context.Set<T>();
        }

        public IQueryable<T> GetAll()
        {
            return _DbSet.AsQueryable<T>();
        }

        public virtual IQueryable<T> GetAllWithIncludes(Expression<Func<T, object>>[] properties)
        {
            var query = _DbSet as IQueryable<T>;

            query = properties
                       .Aggregate(query, (current, property) => current.Include(property));

            return query.AsNoTracking();
        }

        public T GetSingle(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return _DbSet.Where(predicate).FirstOrDefault();
        }

        public void Add(T entity)
        {
            _DbSet.Add(entity);
        }

        public void Update(T entity)
        {
            Context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
        }

        public void Delete(T entity)
        {
            _DbSet.Remove(entity);
        }

        public void Attach(T entity)
        {
            Context.Entry(entity).State = EntityState.Unchanged;
            _DbSet.Attach(entity);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            _DbSet.AddRange(entities);
        }

    }
}
