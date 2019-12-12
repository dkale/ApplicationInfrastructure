using System.Data;

namespace DotNetStandard.Infrastructure.DbPolly
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection(string connectionStringName);
    }
}
