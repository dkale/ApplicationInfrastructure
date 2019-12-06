using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DotNetFramework.Infrastructure.UnitOfWork
{
    public interface IGenericRepository<TEntity> : IRepository where TEntity : class
    {
        IQueryable<TEntity> EntityDbSet { get; }
        IQueryable<TEntity> FromQuery(string query);
        DbContextConfiguration Configuration { get; }
        DbChangeTracker ChangeTracker { get; }
        TEntity Find(Expression<Func<TEntity, bool>> predicate);
        TEntity FindByKeys(params object[] keyValues);
        void UpdateWithEntity(TEntity existingEntity, TEntity modifiedEntity, List<string> propertiesToExclude = null);
        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
        void Delete(TEntity entity);
        void Update(TEntity entity);
        void SaveChanges();
        Task SaveChangesAsync();
    }

    public interface IRepository
    {
    }
}
