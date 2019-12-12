using Polly;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetStandard.Infrastructure.DbPolly
{
    public class PolicyDbCommand : DbCommand
    {
        private readonly DbCommand _dbCommand;
        private readonly IAsyncPolicy _asyncPolicy;
        private readonly ISyncPolicy _syncPolicy;

        public PolicyDbCommand(DbCommand dbCommand, IAsyncPolicy asyncPolicy, ISyncPolicy syncPolicy)
        {
            _dbCommand = dbCommand;
            _asyncPolicy = asyncPolicy;
            _syncPolicy = syncPolicy;
        }

        public override string CommandText
        {
            get => _dbCommand.CommandText;
            set => _dbCommand.CommandText = value;
        }

        public override int CommandTimeout
        {
            get => _dbCommand.CommandTimeout;
            set => _dbCommand.CommandTimeout = value;
        }

        public override CommandType CommandType
        {
            get => _dbCommand.CommandType;
            set => _dbCommand.CommandType = value;
        }

        public override UpdateRowSource UpdatedRowSource
        {
            get => _dbCommand.UpdatedRowSource;
            set => _dbCommand.UpdatedRowSource = value;
        }

        public override bool DesignTimeVisible
        {
            get => _dbCommand.DesignTimeVisible;
            set => _dbCommand.DesignTimeVisible = value;
        }

        protected override DbConnection DbConnection
        {
            get => _dbCommand.Connection;
            set => _dbCommand.Connection = value;
        }

        protected override DbParameterCollection DbParameterCollection => _dbCommand.Parameters;

        protected override DbTransaction DbTransaction
        {
            get => _dbCommand.Transaction;
            set => _dbCommand.Transaction = value;
        }

        public override void Cancel()
        {
            _dbCommand.Cancel();
        }

        public override int ExecuteNonQuery()
        {
            return _syncPolicy.Execute(() => _dbCommand.ExecuteNonQuery());
        }

        public override Task<int> ExecuteNonQueryAsync(CancellationToken cancellationToken)
        {
            return _asyncPolicy.ExecuteAsync(ct => _dbCommand.ExecuteNonQueryAsync(ct), cancellationToken);
        }

        public override object ExecuteScalar()
        {
            return _syncPolicy.Execute(() => _dbCommand.ExecuteScalar());
        }

        public override Task<object> ExecuteScalarAsync(CancellationToken cancellationToken)
        {
            return _asyncPolicy.ExecuteAsync(ct => _dbCommand.ExecuteScalarAsync(ct), cancellationToken);
        }

        public override void Prepare()
        {
            _syncPolicy.Execute(() => _dbCommand.Prepare());
        }

        protected override DbParameter CreateDbParameter()
        {
            return _dbCommand.CreateParameter();
        }

        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            DbDataReader dbDataReader = _syncPolicy.Execute(() => _dbCommand.ExecuteReader(behavior));
            return new PolicyDbDataReader(dbDataReader, _asyncPolicy, _syncPolicy);
        }

        protected override async Task<DbDataReader> ExecuteDbDataReaderAsync(CommandBehavior behavior, CancellationToken cancellationToken)
        {
            DbDataReader dbDataReader = await _asyncPolicy.ExecuteAsync(ct => _dbCommand.ExecuteReaderAsync(behavior, ct), cancellationToken).ConfigureAwait(false);
            return new PolicyDbDataReader(dbDataReader, _asyncPolicy, _syncPolicy);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbCommand?.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
