using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DotNetStandard.Infrastructure.GenericRepository
{
    public interface IGenericRepository
    {
        IQueryable<TEntity> Queryable<TEntity>()
            where TEntity : class;
        Task<TEntity> FindAsync<TEntity>(Expression<Func<TEntity, bool>> predicate)
            where TEntity : class;
        void AddEntity<TEntity>(TEntity entity)
            where TEntity : class;
        void AddRange<TEntity>(IEnumerable<TEntity> entityList)
            where TEntity : class;
        void DeleteEntity<TEntity>(TEntity entity)
            where TEntity : class;
        void UpdateEntity<TEntity>(TEntity entity, List<string> propertiesToExclude = null)
            where TEntity : class;
        void UpdateWithEntity<TEntity>(TEntity existingEntity, TEntity modifiedEntity, List<string> propertiesToExclude = null)
            where TEntity : class;
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
