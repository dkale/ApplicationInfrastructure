using DotNetFramework.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DotNetFramework.Infrastructure.TestClasses
{
    public class TestRepository<TEntity> : IGenericRepository<TEntity>
         where TEntity : class
    {
        private readonly TestDbSet<TEntity> _data;

        public TestRepository()
        {
            _data = new TestDbSet<TEntity>(); //new ObservableCollection<TEntity>();
            EntityDbSet = _data.AsQueryable();
        }

        public IQueryable<TEntity> EntityDbSet { get; }

        public DbContextConfiguration Configuration => throw new NotImplementedException();

        public DbChangeTracker ChangeTracker => throw new NotImplementedException();

        public TEntity Find(Expression<Func<TEntity, bool>> predicate)
        {
            return EntityDbSet.FirstOrDefault(predicate);
        }

        public TEntity FindByKeys(params object[] keyValues)
        {
            throw new NotImplementedException();
        }

        public void Add(TEntity entity)
        {
            _data.Add(entity);
        }

        public void Delete(TEntity entity)
        {
            _data.Remove(entity);
        }

        public void Update(TEntity entity)
        {

        }

        public void UpdateWithEntity(TEntity existingEntity, TEntity modifiedEntity,
            List<string> propertiesToExclude = null)
        {
            //_dbContext.Entry(existingEntity).State = EntityState.Modified;

            //if (propertiesToExclude != null && propertiesToExclude.Any())
            //{
            //    propertiesToExclude.ForEach(propName =>
            //    {
            //        if (existingEntity.HasProperty(propName))
            //            _dbContext.Entry(existingEntity).Property(propName).IsModified = false;
            //    });
            //}

            //_dbContext.Entry(existingEntity).CurrentValues.SetValues(modifiedEntity);
        }

        public void SaveChanges()
        {

        }

        public async Task SaveChangesAsync()
        {
            await Task.CompletedTask;
        }

        public IQueryable<TEntity> FromQuery(string query)
        {
            throw new NotImplementedException();
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            _data.AddRange(entities);
        }
    }
}
