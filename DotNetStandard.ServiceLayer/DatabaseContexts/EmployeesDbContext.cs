using Microsoft.EntityFrameworkCore;

namespace DotNetStandard.ServiceLayer.DatabaseContexts
{
    public sealed class EmployeesDbContext : DbContext
    {
        public const string SchemaName = "Emp";
        public const string MigrationHistoryTableName = "MigrationHistory";

        public EmployeesDbContext(DbContextOptions<EmployeesDbContext> options)
            : base(options)
        {
        }
    }
}
