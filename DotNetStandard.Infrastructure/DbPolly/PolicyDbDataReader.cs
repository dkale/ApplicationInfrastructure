using Polly;
using System;
using System.Collections;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetStandard.Infrastructure.DbPolly
{
    public class PolicyDbDataReader : DbDataReader
    {
        private readonly DbDataReader _dbDataReader;
        private readonly IAsyncPolicy _asyncPolicy;
        private readonly ISyncPolicy _syncPolicy;

        public PolicyDbDataReader(DbDataReader dbDataReader, IAsyncPolicy asyncPolicy, ISyncPolicy syncPolicy)
        {
            _dbDataReader = dbDataReader;
            _asyncPolicy = asyncPolicy;
            _syncPolicy = syncPolicy;
        }

        public override int FieldCount => _dbDataReader.FieldCount;

        public override int RecordsAffected => _dbDataReader.RecordsAffected;

        public override bool HasRows => _dbDataReader.HasRows;

        public override bool IsClosed => _dbDataReader.IsClosed;

        public override int Depth => _dbDataReader.Depth;

        public override object this[int ordinal] => _dbDataReader[ordinal];

        public override object this[string name] => _dbDataReader[name];

        public override bool GetBoolean(int ordinal)
        {
            return _dbDataReader.GetBoolean(ordinal);
        }

        public override byte GetByte(int ordinal)
        {
            return _dbDataReader.GetByte(ordinal);
        }

        public override long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length)
        {
            return _dbDataReader.GetBytes(ordinal, dataOffset, buffer, bufferOffset, length);
        }

        public override char GetChar(int ordinal)
        {
            return _dbDataReader.GetChar(ordinal);
        }

        public override long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length)
        {
            return _dbDataReader.GetChars(ordinal, dataOffset, buffer, bufferOffset, length);
        }

        public override string GetDataTypeName(int ordinal)
        {
            return _dbDataReader.GetDataTypeName(ordinal);
        }

        public override DateTime GetDateTime(int ordinal)
        {
            return _dbDataReader.GetDateTime(ordinal);
        }

        public override decimal GetDecimal(int ordinal)
        {
            return _dbDataReader.GetDecimal(ordinal);
        }

        public override double GetDouble(int ordinal)
        {
            return _dbDataReader.GetDouble(ordinal);
        }

        public override Type GetFieldType(int ordinal)
        {
            return _dbDataReader.GetFieldType(ordinal);
        }

        public override float GetFloat(int ordinal)
        {
            return _dbDataReader.GetFloat(ordinal);
        }

        public override Guid GetGuid(int ordinal)
        {
            return _dbDataReader.GetGuid(ordinal);
        }

        public override short GetInt16(int ordinal)
        {
            return _dbDataReader.GetInt16(ordinal);
        }

        public override int GetInt32(int ordinal)
        {
            return _dbDataReader.GetInt32(ordinal);
        }

        public override long GetInt64(int ordinal)
        {
            return _dbDataReader.GetInt64(ordinal);
        }

        public override string GetName(int ordinal)
        {
            return _dbDataReader.GetName(ordinal);
        }

        public override int GetOrdinal(string name)
        {
            return _dbDataReader.GetOrdinal(name);
        }

        public override string GetString(int ordinal)
        {
            return _dbDataReader.GetString(ordinal);
        }

        public override object GetValue(int ordinal)
        {
            return _dbDataReader.GetValue(ordinal);
        }

        public override int GetValues(object[] values)
        {
            return _dbDataReader.GetValues(values);
        }

        public override bool IsDBNull(int ordinal)
        {
            return _dbDataReader.IsDBNull(ordinal);
        }

        public override bool NextResult()
        {
            return _dbDataReader.NextResult();
        }

        public override bool Read()
        {
            return _syncPolicy.Execute(() => _dbDataReader.Read());
        }

        public override Task<bool> ReadAsync(CancellationToken cancellationToken)
        {
            return _asyncPolicy.ExecuteAsync(ct => base.ReadAsync(ct), cancellationToken);
        }

        public override IEnumerator GetEnumerator()
        {
            return _dbDataReader.GetEnumerator();
        }

        public override void Close()
        {
            _dbDataReader.Close();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbDataReader.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
