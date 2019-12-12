using DotNetStandard.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetStandard.Infrastructure.GenericRepository
{
    // https://codingblast.com/entity-framework-core-generic-repository/
    public class GenericRepository<TContext> : IGenericRepository
        where TContext : DbContext
    {
        private readonly TContext _dbContext;

        public GenericRepository(TContext context)
        {
            _dbContext = context;
        }

        public virtual IQueryable<TEntity> Queryable<TEntity>()
            where TEntity : class
            => _dbContext.Set<TEntity>();

        public Task<TEntity> FindAsync<TEntity>(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate)
            where TEntity : class
            => _dbContext.Set<TEntity>().FirstOrDefaultAsync(predicate);

        public virtual void AddEntity<TEntity>(TEntity entity)
            where TEntity : class
            => _dbContext.Set<TEntity>().Add(entity);

        public virtual void AddRange<TEntity>(IEnumerable<TEntity> entityList)
            where TEntity : class
            => _dbContext.Set<TEntity>().AddRange(entityList);

        public virtual void DeleteEntity<TEntity>(TEntity entity)
            where TEntity : class
            => _dbContext.Set<TEntity>().Remove(entity);

        public virtual void UpdateEntity<TEntity>(TEntity entity, List<string> propertiesToExclude = null)
            where TEntity : class
        {
            _dbContext.Entry(entity).State = EntityState.Modified;

            if (!propertiesToExclude.IsNullOrEmpty())
            {
                propertiesToExclude.ForEach(propName => _dbContext.Entry(entity).Property(propName).IsModified = entity.HasProperty(propName));
            }
        }

        public virtual void UpdateWithEntity<TEntity>(TEntity existingEntity, TEntity modifiedEntity, List<string> propertiesToExclude = null)
            where TEntity : class
        {
            _dbContext.Entry(existingEntity).CurrentValues.SetValues(modifiedEntity);

            UpdateEntity(existingEntity, propertiesToExclude);
        }

        public virtual int SaveChanges()
            => _dbContext.SaveChanges();

        public virtual async Task<int> SaveChangesAsync()
            => await _dbContext.SaveChangesAsync();
    }
}
