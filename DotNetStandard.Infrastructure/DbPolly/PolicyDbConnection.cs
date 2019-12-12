using Polly;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetStandard.Infrastructure.DbPolly
{
    public class PolicyDbConnection : DbConnection
    {
        private readonly DbConnection _dbConnection;
        private readonly IAsyncPolicy _asyncPolicy;
        private readonly ISyncPolicy _syncPolicy;

        public PolicyDbConnection(DbConnection dbConnection, IAsyncPolicy asyncPolicy, ISyncPolicy syncPolicy)
        {
            _dbConnection = dbConnection;
            _asyncPolicy = asyncPolicy;
            _syncPolicy = syncPolicy;
        }

        public override string ConnectionString
        {
            get => _dbConnection.ConnectionString;
            set => _dbConnection.ConnectionString = value;
        }

        public override string DataSource => _dbConnection.DataSource;

        public override string Database => _dbConnection.Database;

        public override ConnectionState State => _dbConnection.State;

        public override string ServerVersion => _dbConnection.ServerVersion;

        public override void ChangeDatabase(string databaseName)
        {
            _dbConnection.ChangeDatabase(databaseName);
        }

        public override void Close()
        {
            _dbConnection.Close();
        }

        public override void Open()
        {
            _syncPolicy.Execute(() => _dbConnection.Open());
        }

        public override Task OpenAsync(CancellationToken cancellationToken)
        {
            return _asyncPolicy.ExecuteAsync(ct => _dbConnection.OpenAsync(ct), cancellationToken);
        }

        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            return _dbConnection.BeginTransaction(isolationLevel);
        }

        protected override DbCommand CreateDbCommand()
        {
            return new PolicyDbCommand(_dbConnection.CreateCommand(), _asyncPolicy, _syncPolicy);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbConnection.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
