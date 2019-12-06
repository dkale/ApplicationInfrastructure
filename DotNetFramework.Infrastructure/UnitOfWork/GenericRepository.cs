using DotNetFramework.Infrastructure.DatabaseContext;
using DotNetStandard.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetFramework.Infrastructure.UnitOfWork
{
    public sealed class GenericRepository<TContext, TEntity> : IGenericRepository<TEntity>
        where TEntity : class
        where TContext : IDbContext
    {
        private readonly TContext _dbContext;

        public GenericRepository(TContext context)
        {
            _dbContext = context;
        }

        public IQueryable<TEntity> EntityDbSet
        {
            get
            {
                IQueryable<TEntity> query = _dbContext.Set<TEntity>();
                return query;
            }
        }

        public DbContextConfiguration Configuration => _dbContext.Configuration;

        public IQueryable<TEntity> FromQuery(string query)
        {
            return _dbContext.Database.SqlQuery<TEntity>(query).AsQueryable();
        }

        public DbChangeTracker ChangeTracker => _dbContext.ChangeTracker;

        public TEntity Find(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate)
        {
            return _dbContext.Set<TEntity>().FirstOrDefault(predicate);
        }

        public TEntity FindByKeys(params object[] keyValues)
        {
            return _dbContext.Set<TEntity>().Find(keyValues);
        }

        public void Add(TEntity entity)
        {
            _dbContext.Set<TEntity>().Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entity)
        {
            _dbContext.Set<TEntity>().AddRange(entity);
        }
        public void Delete(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
        }

        public void Update(TEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public void UpdateWithEntity(TEntity existingEntity, TEntity modifiedEntity, List<string> propertiesToExclude = null)
        {
            _dbContext.Entry(existingEntity).State = EntityState.Modified;

            if (!propertiesToExclude.IsNullOrEmpty())
            {
                propertiesToExclude.ForEach(propName =>
                {
                    if (existingEntity.HasProperty(propName))
                        _dbContext.Entry(existingEntity).Property(propName).IsModified = false;
                });
            }

            _dbContext.Entry(existingEntity).CurrentValues.SetValues(modifiedEntity);
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
