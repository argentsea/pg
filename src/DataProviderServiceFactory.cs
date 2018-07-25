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

        public void SetParameters(DbCommand cmd, DbParameterCollection parameters)
        {
            foreach (var sourcePrm in parameters)
            {
                var npgSourcePrm = (NpgsqlParameter)sourcePrm;
                var targetPrm = npgSourcePrm.Clone();
                cmd.Parameters.Add(targetPrm);
            }
            ((NpgsqlCommand)cmd).Prepare();
            for (var i = 0; i < parameters.Count; i++)
            {
                cmd.Parameters[i].Value = parameters[i].Value;
            }
        }

    }
}
