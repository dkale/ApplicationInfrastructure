using Dapper;
using DotNetStandard.Infrastructure.DbPolly;
using System.Threading.Tasks;

namespace DotNetStandard.ServiceLayer.IDbConnectionFactoryWithPolly
{
    public class DatabaseAccessService
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public DatabaseAccessService(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task BusinessMethod()
        {
            using (var dbConnection = _dbConnectionFactory.CreateConnection("Employees"))
            {
                await dbConnection.QueryAsync<string>("SELECT * FROM Employees.dbo.EmployeeDetails");
            }
        }
    }
}
