// © John Hicks. All rights reserved. Licensed under the MIT license.
// See the LICENSE file in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using Npgsql;
using System.Threading;

namespace ArgentSea.Pg
{
    /// <summary>
    /// This class is a provider-specific resouce to enable provider-neutral code to execute. It is unlikely that you would reference this in consumer code.
    /// </summary>
	public class DataProviderServiceFactory : IDataProviderServiceFactory
	{
		public bool GetIsErrorTransient(Exception exception)
        {
            if (exception is NpgsqlException)
            {
                var ex = exception as NpgsqlException;
                return ex.IsTransient; 
            }
            return false;
        }

        public DbCommand NewCommand(string storedProcedureName, DbConnection connection)
        {
            if (connection is NpgsqlConnection)
            {
                return new NpgsqlCommand(storedProcedureName, (NpgsqlConnection)connection);
            }
            throw new ArgumentException(nameof(connection));
        }

        public DbConnection NewConnection(string connectionString)
        {
            return new NpgsqlConnection(connectionString);
        }

        public void SetParameters(DbCommand cmd, string[] queryParameterNames, DbParameterCollection parameters, IDictionary<string, object> parameterValues)
        {
            int[] ordinals;

            if (queryParameterNames is null || queryParameterNames.Length == 0)
            {
                ordinals = new int[parameters.Count];
                for (int i = 0; i < parameters.Count; i++)
                {
                    ordinals[i] = i;
                    var npgSourcePrm = (NpgsqlParameter)parameters[i];
                    var targetPrm = npgSourcePrm.Clone();
                    if (!(parameterValues is null))
                    {
                        if (parameterValues.TryGetValue(targetPrm.ParameterName, out var prmValue))
                        {
                            targetPrm.Value = prmValue;
                        }
                    }
                    cmd.Parameters.Add(targetPrm);
                }
            }
            else
            {
                ordinals = new int[queryParameterNames.Length];
                for (int i = 0; i < queryParameterNames.Length; i++)
                {
                    var found = false;
                    for (int j = 0; j < parameters.Count; j++)
                    {
                        if (queryParameterNames[i] == parameters[j].ParameterName)
                        {
                            ordinals[i] = j;
                            found = true;
                            var npgSourcePrm = (NpgsqlParameter)parameters[j];
                            var targetPrm = npgSourcePrm.Clone();
                            if (!(parameterValues is null))
                            {
                                if (parameterValues.TryGetValue(queryParameterNames[i], out var prmValue))
                                {
                                    targetPrm.Value = prmValue;
                                }
                            }
                            cmd.Parameters.Add(targetPrm);
                        }
                    }
                    if (!found)
                    {
                        throw new ParameterNotFoundException($"Expected parameter { queryParameterNames[i] }, but it was not found in the parameter list.");
                    }
                }
            }
            ((NpgsqlCommand)cmd).Prepare();
            for (var i = 0; i < cmd.Parameters.Count; i++)
            {
                cmd.Parameters[i].Value = parameters[ordinals[i]].Value;
            }
        }

    }
}
