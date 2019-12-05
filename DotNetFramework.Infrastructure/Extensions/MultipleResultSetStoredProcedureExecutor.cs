using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;

namespace DotNetFramework.Infrastructure.Extensions
{
    public class MultipleResultSetStoredProcedureExecutor
    {
        private readonly DbContext _dbContext;
        private readonly string _storedProcedure;
        private List<Func<IObjectContextAdapter, DbDataReader, IEnumerable>> _resultSets;
        private readonly Dictionary<string, object> _inputParameters;
        private readonly Dictionary<string, object> _outputParameters;

        public MultipleResultSetStoredProcedureExecutor(DbContext db, string storedProcedure, Dictionary<string, object> inputParameters, Dictionary<string, object> outputParameters)
        {
            _dbContext = db;
            _storedProcedure = storedProcedure;
            _inputParameters = inputParameters;
            _outputParameters = outputParameters;
            _resultSets = new List<Func<IObjectContextAdapter, DbDataReader, IEnumerable>>();
        }

        public MultipleResultSetStoredProcedureExecutor With<TResult>()
        {
            _resultSets.Add((adapter, reader) => adapter
                .ObjectContext
                .Translate<TResult>(reader)
                .ToList());

            return this;
        }

        public List<IEnumerable> Execute()
        {
            var results = new List<IEnumerable>();

            using (var connection = (SqlConnection)_dbContext.Database.Connection)
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = _storedProcedure;

                command.AppendInputParameters(_inputParameters);
                command.AppendOutputParameters(_outputParameters);

                using (var reader = command.ExecuteReader())
                {
                    var adapter = ((IObjectContextAdapter)_dbContext);
                    foreach (var resultSet in _resultSets)
                    {
                        results.Add(resultSet(adapter, reader));
                        reader.NextResult();
                    }
                }

                return results;
            }
        }
    }
}
