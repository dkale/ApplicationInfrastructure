using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace DotNetFramework.Infrastructure.Extensions
{
    public static class SqlCommandExtensions
    {
        public static void AppendInputParameters(this SqlCommand command, Dictionary<string, object> inputParameters)
        {
            if (inputParameters == null)
                return;

            foreach (var pair in inputParameters)
            {
                var dbParameter = command.CreateParameter();

                // Throw exception if parameter name is not a single word
                if (!new Regex(@"^\b[a-zA-Z0-9_]+\b$").IsMatch(pair.Key))
                    throw new Exception("Invalid parameter name : " + pair.Key);

                if (pair.Value is DataTable table)
                {
                    dbParameter.ParameterName = pair.Key;
                    dbParameter.Value = pair.Value;
                    dbParameter.SqlDbType = SqlDbType.Structured;
                    if (!string.IsNullOrEmpty(table.TableName))
                    {
                        dbParameter.TypeName = table.TableName;
                    }
                }
                else
                {
                    dbParameter.ParameterName = pair.Key;
                    dbParameter.Value = pair.Value ?? DBNull.Value;
                }

                command.Parameters.Add(dbParameter);
            }
        }

        public static void AppendOutputParameters(this SqlCommand command, Dictionary<string, object> outputParameters)
        {
            if (outputParameters == null)
                return;

            foreach (var pair in outputParameters)
            {
                var dbParameter = command.CreateParameter();

                // Throw exception if parameter name is not a single word
                if (!new Regex(@"^\b[a-zA-Z0-9_]+\b$").IsMatch(pair.Key))
                    throw new Exception("Invalid output parameter name : " + pair.Key);
                dbParameter.ParameterName = pair.Key;
                dbParameter.Direction = ParameterDirection.Output;
                dbParameter.Size = 200;
                command.Parameters.Add(dbParameter);
            }
        }
    }
}
