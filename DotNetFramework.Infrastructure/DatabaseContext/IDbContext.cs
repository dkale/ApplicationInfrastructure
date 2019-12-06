using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;

namespace DotNetFramework.Infrastructure.DatabaseContext
{
    public interface IDbContext : IDisposable
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        DbEntityEntry<T> Entry<T>(T entity) where T : class;
        int SaveChanges();
        Task<int> SaveChangesAsync();
        DbContextConfiguration Configuration { get; }
        DbChangeTracker ChangeTracker { get; }
        Database Database { get; }
    }
}
