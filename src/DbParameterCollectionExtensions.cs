using System;
using System.Collections.Generic;
using System.Text;
using Npgsql;
using NpgsqlTypes;

namespace ArgentSea.Pg
{
    public static class DbParameterCollectionExtensions
    {
        public static NpgsqlParameterCollection MapModelOutput(this NpgsqlParameterCollection prms, string name, long value)
        {
            //Todo...
            return prms;
        }

        public static NpgsqlParameterCollection AddInputParameter(this NpgsqlParameterCollection prms, string name, long value)
        {
            var prm = prms.Add(name, NpgsqlDbType.Bigint);
            prm.Direction = System.Data.ParameterDirection.Input;
            prm.IsNullable = false;
            prm.Value = value;
            return prms;
        }
        public static NpgsqlParameterCollection AddInputParameter(this NpgsqlParameterCollection prms, string name, long? value)
        {
            var prm = prms.Add(name, NpgsqlDbType.Bigint);
            prm.Direction = System.Data.ParameterDirection.Input;
            prm.IsNullable = true;
            if (value.HasValue)
            {
                prm.Value = value;
            }
            else
            {
                prm.Value = System.DBNull.Value;
            }
            return prms;
        }
        public static NpgsqlParameterCollection AddInputParameter(this NpgsqlParameterCollection prms, string name, int value)
        {
            var prm = prms.Add(name, NpgsqlDbType.Integer);
            prm.Direction = System.Data.ParameterDirection.Input;
            prm.IsNullable = false;
            prm.Value = value;
            return prms;
        }
        public static NpgsqlParameterCollection AddInputParameter(this NpgsqlParameterCollection prms, string name, int? value)
        {
            var prm = prms.Add(name, NpgsqlDbType.Integer);
            prm.Direction = System.Data.ParameterDirection.Input;
            prm.IsNullable = true;
            if (value.HasValue)
            {
                prm.Value = value;
            }
            else
            {
                prm.Value = System.DBNull.Value;
            }
            return prms;

            ////numbers
            //NpgsqlDbType.Bigint;
            //NpgsqlDbType.Double;
            //NpgsqlDbType.Integer;
            //NpgsqlDbType.Money;
            //NpgsqlDbType.Real;
            //NpgsqlDbType.Smallint;

            ////dates
            //NpgsqlDbType.Abstime;
            //NpgsqlDbType.Date;
            //NpgsqlDbType.Interval;
            //NpgsqlDbType.Time;
            //NpgsqlDbType.Timestamp;
            //NpgsqlDbType.TimestampTZ;
            //NpgsqlDbType.TimeTZ;

            ////other
            //NpgsqlDbType.Array;
            //NpgsqlDbType.Json;
            //NpgsqlDbType.Jsonb;

            //NpgsqlDbType.Bit;
            //NpgsqlDbType.Boolean;

            ////spatial
            //NpgsqlDbType.Box;
            //NpgsqlDbType.Circle;
            //NpgsqlDbType.Geometry;
            //NpgsqlDbType.Line;
            //NpgsqlDbType.LSeg;
            //NpgsqlDbType.Path;
            //NpgsqlDbType.Point;
            //NpgsqlDbType.Polygon;

            //NpgsqlDbType.Bytea;
            //NpgsqlDbType.Char;
            //NpgsqlDbType.Cid;
            //NpgsqlDbType.Cidr;
            //NpgsqlDbType.Citext;
            //NpgsqlDbType.Composite;
            //NpgsqlDbType.Enum;
            //NpgsqlDbType.Hstore;
            //NpgsqlDbType.Inet;
            //NpgsqlDbType.Int2Vector;
            //NpgsqlDbType.InternalChar; //char
            //NpgsqlDbType.MacAddr;
            //NpgsqlDbType.Name;
            //NpgsqlDbType.Numeric;
            //NpgsqlDbType.Oid;
            //NpgsqlDbType.Oidvector;
            //NpgsqlDbType.Range;
            //NpgsqlDbType.Refcursor;
            //NpgsqlDbType.Regtype;
            //NpgsqlDbType.Text;
            //NpgsqlDbType.Tid;
            //NpgsqlDbType.TsQuery;
            //NpgsqlDbType.TsVector;
            //NpgsqlDbType.Unknown;
            //NpgsqlDbType.Uuid;
            //NpgsqlDbType.Varbit;
            //NpgsqlDbType.Varchar;
            //NpgsqlDbType.Xid;
            //NpgsqlDbType.Xml;


        }

    }
    public class Test
    {
        public void Test2()
        {
            var cmd = new NpgsqlCommand();
            var prms = cmd.Parameters
                .AddInputParameter("@ShardId", 0)
                .AddInputParameter("@ShardId2", 1)
                .MapModelOutput("", 0);
        }
    }
}
