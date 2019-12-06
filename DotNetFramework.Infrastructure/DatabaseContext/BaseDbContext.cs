using DotNetStandard.Infrastructure.Extensions;
using MoreLinq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetFramework.Infrastructure.DatabaseContext
{
    public class BaseDbContext<TDbContext> : DbContext, IDbContext where TDbContext : DbContext
    {
        private static string GetEnvConn(string s)
        {
            return s; // + "_" + EnvironmentSettings.GetEnvironmentType().Replace("_", "");
        }

        public BaseDbContext(string nameOrConnectionString) : base(GetEnvConn(nameOrConnectionString))
        {
            Database.SetInitializer<TDbContext>(null);
        }

        public async Task<IEnumerable<TResult>> ExecuteTableValueFunction<TResult>(string storedProcedureNameWithParameters)
        {
            var query =
                Database.SqlQuery<TResult>($"SELECT * FROM {storedProcedureNameWithParameters}");

            var m = await query.ToListAsync();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<TResult>> ExecuteStoredProcedure<TResult>(string storedProcedureName,
            params object[] parameters)
        {
            var sqlParameters = GetSqlParametersString(parameters);
            var query =
                Database.SqlQuery<TResult>($"exec {storedProcedureName} {sqlParameters}", parameters);
            return await query.ToListAsync();
        }

        public async Task<TResult> ExecuteScalarValuedFunction<TResult>(string functionName,
            params object[] parameters)
        {
            var sqlParameters = GetSqlParametersString(parameters);
            var query =
                Database.SqlQuery<TResult>($"SELECT dbo.{functionName}({sqlParameters})", parameters);
            return await query.FirstOrDefaultAsync().ConfigureAwait(false);
        }

        private static string GetSqlParametersString(IReadOnlyCollection<object> parameters)
        {
            return Enumerable
                .Range(0, parameters.Count)
                .Select(i => $"@p{i}")
                .ToCommaSeparatedString();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<DecimalPropertyConvention>();
            modelBuilder.Conventions.Add(new DecimalPropertyConvention(28, 8));
            RegisterConfigurations(modelBuilder);
        }

        private void RegisterConfigurations(DbModelBuilder modelBuilder)
        {
            var assemblies = GetType()
                .GetInterfaces()
                .SelectMany(t => t.GetProperties())
                .Select(property => property.PropertyType)
                .Where(propertyType => propertyType.IsGenericType)
                .Where(propertyType => propertyType.GetGenericTypeDefinition() == typeof(IDbSet<>) || propertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
                .SelectMany(propertyType => propertyType.GetGenericArguments())
                .Select(propertyType => propertyType.Assembly)
                .Distinct();
            assemblies.ForEach(assembly => modelBuilder.Configurations.AddFromAssembly(assembly));
        }
    }
}
