using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using Npgsql;
using System.Threading;

namespace ArgentSea.Pg
{
    public class DataProviderServices: IDataProviderServices
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

        //public string NormalizeFieldName(string fieldName)
        //{
        //    return fieldName.ToLower();
        //}

        //public string NormalizeParameterName(string parameterName)
        //{
        //    return parameterName.ToLower();
        //}

        public void SetParameters(DbCommand cmd, DbParameterCollection parameters)
        {
            foreach (var sourcePrm in parameters)
            {
                var npgSourcePrm = (NpgsqlParameter)sourcePrm;
                var targetPrm = npgSourcePrm.Clone();
                cmd.Parameters.Add(targetPrm);
            }
        }

    }
}
