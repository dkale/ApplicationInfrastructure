using Polly;
using System;
using System.Data;
using System.Data.Common;

namespace DotNetStandard.Infrastructure.DbPolly
{
    public class PolicyDbConnectionFactory : IDbConnectionFactory
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly IAsyncPolicy _asyncPolicy;
        private readonly ISyncPolicy _syncPolicy;

        public PolicyDbConnectionFactory(IDbConnectionFactory dbConnectionFactory, IAsyncPolicy asyncPolicy, ISyncPolicy syncPolicy)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _asyncPolicy = asyncPolicy;
            _syncPolicy = syncPolicy;
        }

        public IDbConnection CreateConnection(string connectionStringName)
        {
            return _dbConnectionFactory.CreateConnection(connectionStringName) is DbConnection dbConnection
                ? new PolicyDbConnection(dbConnection, _asyncPolicy, _syncPolicy)
                : throw new NotSupportedException();
        }
    }
}
