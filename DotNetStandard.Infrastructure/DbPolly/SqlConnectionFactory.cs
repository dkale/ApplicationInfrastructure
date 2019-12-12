using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DotNetStandard.Infrastructure.DbPolly
{
    public class SqlConnectionFactory : IDbConnectionFactory
    {
        private readonly IConfiguration _configuration;

        public SqlConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Creates and opens a new DbConnection for a given <paramref name="connectionStringName"/>. A caller is responsible of closing the connection.
        /// </summary>
        /// <param name="connectionStringName">The name of connection string</param>
        /// <returns>An open connection.</returns>
        public IDbConnection CreateConnection(string connectionStringName)
        {
            string connectionString = _configuration.GetConnectionString(connectionStringName);
            var sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            return sqlConnection;
        }
    }
}
