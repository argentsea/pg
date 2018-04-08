using System;
using System.Collections.Generic;
using System.Text;
using Npgsql;
using NpgsqlTypes;
using System.Data.Common;
using System.Data;
using Microsoft.Extensions.Logging;
using ArgentSea;

namespace ArgentSea.Pg
{
	public static class PgParameterCollectionExtensions
	{

		private static string NormalizeSqlParameterName(string parameterName)
		{
			if (!string.IsNullOrEmpty(parameterName) && !parameterName.StartsWith(":"))
			{
				return ":" + parameterName;
			}
			return parameterName;
		}

		#region String types
		//VARCHAR
		/// <summary>
		/// Creates parameter for providing a string or a DBNull value to a stored procedure.
		/// </summary>
		/// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
		/// <param name="parameterName">The name of the parameter. If the name doesn’t start with “@”, it will be automatically pre-pended.</param>
		/// <param name="value">An empty string will be saved as a zero-length string; a null string will be saved as a database null value.</param>
		/// <param name="maxLength">This should match the size of the parameter, not the size of the input string (and certainly not the number of bytes).
		/// For nvarchar(max) parameters, specify -1. 
		/// Setting the value correctly will help avoid plan cache pollution (when not using stored procedures) and minimize memory buffer allocations.</param>
		/// <returns>The DbParameterCollection to which the parameter was appended.</returns>
		public static DbParameterCollection AddPgVarcharInParameter(this DbParameterCollection prms, string parameterName, string value, int maxLength)
		{
			var prm = new NpgsqlParameter(NormalizeSqlParameterName(parameterName), NpgsqlDbType.Varchar, maxLength)
			{
				Value = value != null ? value : (dynamic)System.DBNull.Value,
				Direction = ParameterDirection.Input
			};
			prms.Add(prm);
			return prms;
		}
		/// <summary>
		/// Creates parameter for obtaining a string from a stored procedure.
		/// </summary>
		/// <param name="prms">The existing parameter collection to which this output parameter should be added.</param>
		/// <param name="parameterName">The name of the parameter. If the name doesn’t start with “:”, it will be automatically pre-pended.</param>
		/// <param name="length">Specifies the number of characters in the string. If the original string value is smaller than this length, the returned value will be padded with spaces.</param>
		/// <returns>The DbParameterCollection to which the parameter was appended.</returns>
		public static DbParameterCollection AddPgVarcharOutParameter(this DbParameterCollection prms, string parameterName, int maxLength)
		{
			var prm = new NpgsqlParameter(NormalizeSqlParameterName(parameterName), NpgsqlDbType.Varchar, maxLength)
			{
				Direction = ParameterDirection.Output
			};
			prms.Add(prm);
			return prms;
		}
		//CHAR
		/// <summary>
		/// Creates parameter for providing a fixed-length string or a DBNull value to a stored procedure.
		/// </summary>
		/// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
		/// <param name="parameterName">The name of the parameter. If the name doesn’t start with “:”, it will be automatically pre-pended.</param>
		/// <param name="value">An empty string will be saved as a zero-length string; a null string will be saved as a database null value.</param>
		/// <param name="length">Specifies the number of characters in the string. If the original string value is smaller than this length, the returned value will be padded with spaces.</param>
		/// <returns>The DbParameterCollection to which the parameter was appended.</returns>
		public static DbParameterCollection AddPgCharInParameter(this DbParameterCollection prms, string parameterName, string value, int length)
		{
			var prm = new NpgsqlParameter(NormalizeSqlParameterName(parameterName), NpgsqlDbType.Char, length)
			{
				Value = value != null ? value : (dynamic)System.DBNull.Value,
				Direction = ParameterDirection.Input
			};
			prms.Add(prm);
			return prms;
		}
		/// <summary>
		/// Creates parameter for obtaining a fixed-length string from a stored procedure.
		/// </summary>
		/// <param name="prms">The existing parameter collection to which this output parameter should be added.</param>
		/// <param name="parameterName">The name of the parameter. If the name doesn’t start with “:”, it will be automatically pre-pended.</param>
		/// <param name="length">Specifies the number of characters in the string. If the original string value is smaller than this length, the returned value will be padded with spaces.</param>
		/// <returns>The DbParameterCollection to which the parameter was appended.</returns>
		public static DbParameterCollection AddPgCharOutParameter(this DbParameterCollection prms, string parameterName, int length)
		{
			var prm = new NpgsqlParameter(NormalizeSqlParameterName(parameterName), NpgsqlDbType.Char, length)
			{
				Direction = ParameterDirection.Output
			};
			prms.Add(prm);
			return prms;
		}
		//TEXT
		/// <summary>
		/// Creates parameter for providing a string or a DBNull value to a stored procedure, which is converted to the target ANSI code page (if possible).
		/// </summary>
		/// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
		/// <param name="parameterName">The name of the parameter. If the name doesn’t start with “:”, it will be automatically pre-pended.</param>
		/// <param name="value">An empty string will be saved as a zero-length string; a null string will be saved as a database null value.</param>
		/// <returns>The DbParameterCollection to which the parameter was appended.</returns>
		public static DbParameterCollection AddPgTextInParameter(this DbParameterCollection prms, string parameterName, string value)
		{
			var prm = new NpgsqlParameter(NormalizeSqlParameterName(parameterName), NpgsqlDbType.Text)
			{
				Value = value != null ? value : (dynamic)System.DBNull.Value,
				Direction = ParameterDirection.Input
			};
			prms.Add(prm);
			return prms;
		}
		/// <summary>
		/// Creates parameter for obtaining a string from a stored procedure, which has been converted from the source ANSI code page.
		/// </summary>
		/// <param name="prms">The existing parameter collection to which this output parameter should be added.</param>
		/// <param name="parameterName">The name of the parameter. If the name doesn’t start with “:”, it will be automatically pre-pended.</param>
		/// <returns>The DbParameterCollection to which the parameter was appended.</returns>
		public static DbParameterCollection AddPgTextOutParameter(this DbParameterCollection prms, string parameterName)
		{
			var prm = new NpgsqlParameter(NormalizeSqlParameterName(parameterName), NpgsqlDbType.Text)
			{
				Direction = ParameterDirection.Output,
			};
			prms.Add(prm);
			return prms;
		}
		#endregion
		#region Numeric types
		//LONG
		/// <summary>
		/// Creates a parameter for providing a 64-bit signed integer (long) to a stored procedure.
		/// </summary>
		/// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
		/// <param name="parameterName">The name of the parameter. If the name doesn’t start with “:”, it will be automatically pre-pended.</param>
		/// <param name="value">A 64-bit signed integer value.</param>
		/// <returns>The DbParameterCollection to which the parameter was appended.</returns>
		public static DbParameterCollection AddPgBigintInParameter(this DbParameterCollection prms, string parameterName, long value)
		{
			var prm = new NpgsqlParameter(NormalizeSqlParameterName(parameterName), NpgsqlDbType.Bigint)
			{
				Value = value,
				Direction = ParameterDirection.Input
			};
			prms.Add(prm);
			return prms;
		}
		/// <summary>
		/// Creates a parameter for providing a 64-bit signed integer (long) or a DbNull value to a stored procedure.
		/// </summary>
		/// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
		/// <param name="parameterName">The name of the parameter. If the name doesn’t start with “:”, it will be automatically pre-pended.</param>
		/// <param name="value">A 64-bit signed integer value or null.</param>
		/// <returns>The DbParameterCollection to which the parameter was appended.</returns>
		public static DbParameterCollection AddPgBigintInParameter(this DbParameterCollection prms, string parameterName, long? value)
		{
			var prm = new NpgsqlParameter(NormalizeSqlParameterName(parameterName), NpgsqlDbType.Bigint)
			{
				Value = value.HasValue ? (dynamic)value.Value : System.DBNull.Value,
				Direction = ParameterDirection.Input
			};
			prms.Add(prm);
			return prms;
		}
		/// <summary>
		/// Creates an output parameter for retrieving a 64-bit signed integer (long) from a stored procedure.
		/// </summary>
		/// <param name="prms">The existing parameter collection to which this output parameter should be added.</param>
		/// <param name="parameterName">The name of the parameter. If the name doesn’t start with “:”, it will be automatically pre-pended.</param>
		/// <returns>The DbParameterCollection to which the parameter was appended.</returns>
		public static DbParameterCollection AddPgBigintOutParameter(this DbParameterCollection prms, string parameterName)
		{
			var prm = new NpgsqlParameter(NormalizeSqlParameterName(parameterName), NpgsqlDbType.Bigint)
			{
				Direction = ParameterDirection.Output
			};
			prms.Add(prm);
			return prms;
		}
		//INT
		/// <summary>
		/// Creates a parameter for providing a 32-bit signed integer (int) to a stored procedure.
		/// </summary>
		/// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
		/// <param name="parameterName">The name of the parameter. If the name doesn’t start with “:”, it will be automatically pre-pended.</param>
		/// <param name="value">A 32-bit signed integer value.</param>
		/// <returns>The DbParameterCollection to which the parameter was appended.</returns>
		public static DbParameterCollection AddPgIntegerInParameter(this DbParameterCollection prms, string parameterName, int value)
		{
			var prm = new NpgsqlParameter(NormalizeSqlParameterName(parameterName), NpgsqlDbType.Integer)
			{
				Value = value,
				Direction = ParameterDirection.Input
			};
			prms.Add(prm);
			return prms;
		}
		/// <summary>
		/// Creates a parameter for providing a 32-bit signed integer (int) or a DbNull value to a stored procedure.
		/// </summary>
		/// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
		/// <param name="parameterName">The name of the parameter. If the name doesn’t start with “:”, it will be automatically pre-pended.</param>
		/// <param name="value">A 32-bit signed integer value or null.</param>
		/// <returns>The DbParameterCollection to which the parameter was appended.</returns>
		public static DbParameterCollection AddPgIntegerInParameter(this DbParameterCollection prms, string parameterName, int? value)
		{
			var prm = new NpgsqlParameter(NormalizeSqlParameterName(parameterName), NpgsqlDbType.Integer)
			{
				Value = value.HasValue ? (dynamic)value.Value : System.DBNull.Value,
				Direction = ParameterDirection.Input
			};
			prms.Add(prm);
			return prms;
		}
		/// <summary>
		/// Creates an output parameter for retrieving a 32-bit signed integer (int) from a stored procedure.
		/// </summary>
		/// <param name="prms">The existing parameter collection to which this output parameter should be added.</param>
		/// <param name="parameterName">The name of the parameter. If the name doesn’t start with “:”, it will be automatically pre-pended.</param>
		/// <returns>The DbParameterCollection to which the parameter was appended.</returns>
		public static DbParameterCollection AddPgIntegerOutParameter(this DbParameterCollection prms, string parameterName)
		{
			var prm = new NpgsqlParameter(NormalizeSqlParameterName(parameterName), NpgsqlDbType.Integer)
			{
				Direction = ParameterDirection.Output
			};
			prms.Add(prm);
			return prms;
		}

		//SHORT
		/// <summary>
		/// Creates a parameter for providing a 16-bit signed integer (short) to a stored procedure.
		/// </summary>
		/// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
		/// <param name="parameterName">The name of the parameter. If the name doesn’t start with “:”, it will be automatically pre-pended.</param>
		/// <param name="value">A 16-bit signed integer value.</param>
		/// <returns>The DbParameterCollection to which the parameter was appended.</returns>
		public static DbParameterCollection AddPgSmallintInParameter(this DbParameterCollection prms, string parameterName, short value)
		{
			var prm = new NpgsqlParameter(NormalizeSqlParameterName(parameterName), NpgsqlDbType.Smallint)
			{
				Value = value,
				Direction = ParameterDirection.Input
			};
			prms.Add(prm);
			return prms;
		}
		/// <summary>
		/// Creates a parameter for providing a 16-bit signed integer (short) or a DbNull value to a stored procedure.
		/// </summary>
		/// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
		/// <param name="parameterName">The name of the parameter. If the name doesn’t start with “:”, it will be automatically pre-pended.</param>
		/// <param name="value">A 16-bit signed integer value or null.</param>
		/// <returns>The DbParameterCollection to which the parameter was appended.</returns>
		public static DbParameterCollection AddPgSmallintInParameter(this DbParameterCollection prms, string parameterName, short? value)
		{
			var prm = new NpgsqlParameter(NormalizeSqlParameterName(parameterName), NpgsqlDbType.Smallint)
			{
				Value = value.HasValue ? (dynamic)value.Value : System.DBNull.Value,
				Direction = ParameterDirection.Input
			};
			prms.Add(prm);
			return prms;
		}
		/// <summary>
		/// Creates an output parameter for retrieving a 32-bit signed integer (short) from a stored procedure.
		/// </summary>
		/// <param name="prms">The existing parameter collection to which this output parameter should be added.</param>
		/// <param name="parameterName">The name of the parameter. If the name doesn’t start with “:”, it will be automatically pre-pended.</param>
		/// <returns>The DbParameterCollection to which the parameter was appended.</returns>
		public static DbParameterCollection AddPgSmallintOutParameter(this DbParameterCollection prms, string parameterName)
		{
			var prm = new NpgsqlParameter(NormalizeSqlParameterName(parameterName), NpgsqlDbType.Smallint)
			{
				Direction = ParameterDirection.Output
			};
			prms.Add(prm);
			return prms;
		}
		//BYTE
		/// <summary>
		/// Creates a parameter for providing a 8-bit unsigned integer (byte) to a stored procedure.
		/// </summary>
		/// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
		/// <param name="parameterName">The name of the parameter. If the name doesn’t start with “:”, it will be automatically pre-pended.</param>
		/// <param name="value">An unsigned 8-bit integer value.</param>
		/// <returns>The DbParameterCollection to which the parameter was appended.</returns>
		public static DbParameterCollection AddPgInternalCharInParameter(this DbParameterCollection prms, string parameterName, byte value)
		{
			var prm = new NpgsqlParameter(NormalizeSqlParameterName(parameterName), NpgsqlDbType.InternalChar)
			{
				Value = value,
				Direction = ParameterDirection.Input
			};
			prms.Add(prm);
			return prms;
		}
		/// <summary>
		/// Creates a parameter for providing a 8-bit unsigned integer (byte) or a DbNull value to a stored procedure.
		/// </summary>
		/// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
		/// <param name="parameterName">The name of the parameter. If the name doesn’t start with “:”, it will be automatically pre-pended.</param>
		/// <param name="value">An unsigned 8-bit integer value or null.</param>
		/// <returns>The DbParameterCollection to which the parameter was appended.</returns>
		public static DbParameterCollection AddPgInternalCharInParameter(this DbParameterCollection prms, string parameterName, byte? value)
		{
			var prm = new NpgsqlParameter(NormalizeSqlParameterName(parameterName), NpgsqlDbType.InternalChar)
			{
				Value = value.HasValue ? (dynamic)value.Value : System.DBNull.Value,
				Direction = ParameterDirection.Input
			};
			prms.Add(prm);
			return prms;
		}
		/// <summary>
		/// Creates an output parameter for retrieving a 32-bit signed integer (byte) from a stored procedure.
		/// </summary>
		/// <param name="prms">The existing parameter collection to which this output parameter should be added.</param>
		/// <param name="parameterName">The name of the parameter. If the name doesn’t start with “:”, it will be automatically pre-pended.</param>
		/// <returns>The DbParameterCollection to which the parameter was appended.</returns>
		public static DbParameterCollection AddPgInternalCharOutParameter(this DbParameterCollection prms, string parameterName)
		{
			var prm = new NpgsqlParameter(NormalizeSqlParameterName(parameterName), NpgsqlDbType.InternalChar)
			{
				Direction = ParameterDirection.Output
			};
			prms.Add(prm);
			return prms;
		}
		//BOOL
		/// <summary>
		/// Creates a parameter for providing a boolean value (bool) to a stored procedure.
		/// </summary>
		/// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
		/// <param name="parameterName">The name of the parameter. If the name doesn’t start with “:”, it will be automatically pre-pended.</param>
		/// <param name="value">A boolean value.</param>
		/// <returns>The DbParameterCollection to which the parameter was appended.</returns>
		public static DbParameterCollection AddPgBooleanInParameter(this DbParameterCollection prms, string parameterName, bool value)
		{
			var prm = new NpgsqlParameter(NormalizeSqlParameterName(parameterName), NpgsqlDbType.Boolean)
			{
				Value = value,
				Direction = ParameterDirection.Input
			};
			prms.Add(prm);
			return prms;
		}
		/// <summary>
		/// Creates a parameter for providing a boolean value (bool) or a DbNull value to a stored procedure.
		/// </summary>
		/// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
		/// <param name="parameterName">The name of the parameter. If the name doesn’t start with “:”, it will be automatically pre-pended.</param>
		/// <param name="value">A boolean value or null.</param>
		/// <returns>The DbParameterCollection to which the parameter was appended.</returns>
		public static DbParameterCollection AddPgBooleanInParameter(this DbParameterCollection prms, string parameterName, bool? value)
		{
			var prm = new NpgsqlParameter(NormalizeSqlParameterName(parameterName), NpgsqlDbType.Boolean)
			{
				Value = value.HasValue ? (dynamic)value.Value : System.DBNull.Value,
				Direction = ParameterDirection.Input
			};
			prms.Add(prm);
			return prms;
		}
		/// <summary>
		/// Creates an output parameter for retrieving a boolean value (bool) from a stored procedure.
		/// </summary>
		/// <param name="prms">The existing parameter collection to which this output parameter should be added.</param>
		/// <param name="parameterName">The name of the parameter. If the name doesn’t start with “:”, it will be automatically pre-pended.</param>
		/// <returns>The DbParameterCollection to which the parameter was appended.</returns>
		public static DbParameterCollection AddPgBooleanOutParameter(this DbParameterCollection prms, string parameterName)
		{
			var prm = new NpgsqlParameter(NormalizeSqlParameterName(parameterName), NpgsqlDbType.Boolean)
			{
				Direction = ParameterDirection.Output
			};
			prms.Add(prm);
			return prms;
		}
		//NUMERIC
		/// <summary>
		/// Creates a parameter for providing a decmial value to a stored procedure.
		/// </summary>
		/// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
		/// <param name="parameterName">The name of the parameter. If the name doesn’t start with “:”, it will be automatically pre-pended.</param>
		/// <param name="value">A decmial value .</param>
		/// <returns>The DbParameterCollection to which the parameter was appended.</returns>
		public static DbParameterCollection AddPgNumericInParameter(this DbParameterCollection prms, string parameterName, decimal value, byte precision, byte scale)
		{
			var prm = new NpgsqlParameter(NormalizeSqlParameterName(parameterName), NpgsqlDbType.Numeric)
			{
				Value = value,
				Direction = ParameterDirection.Input,
				Precision = precision,
				Scale = scale
			};
			prms.Add(prm);
			return prms;
		}
		/// <summary>
		/// Creates a parameter for providing a decmial value or a DbNull value to a stored procedure.
		/// </summary>
		/// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
		/// <param name="parameterName">The name of the parameter. If the name doesn’t start with “:”, it will be automatically pre-pended.</param>
		/// <param name="value">A decmial value or null.</param>
		/// <returns>The DbParameterCollection to which the parameter was appended.</returns>
		public static DbParameterCollection AddPgNumericInParameter(this DbParameterCollection prms, string parameterName, decimal? value, byte precision, byte scale)
		{
			var prm = new NpgsqlParameter(NormalizeSqlParameterName(parameterName), NpgsqlDbType.Numeric)
			{
				Value = value.HasValue ? (dynamic)value.Value : System.DBNull.Value,
				Direction = ParameterDirection.Input,
				Precision = precision,
				Scale = scale
			};
			prms.Add(prm);
			return prms;
		}
		/// <summary>
		/// Creates an output parameter for retrieving a decmial value from a stored procedure.
		/// </summary>
		/// <param name="prms">The existing parameter collection to which this output parameter should be added.</param>
		/// <param name="parameterName">The name of the parameter. If the name doesn’t start with “:”, it will be automatically pre-pended.</param>
		/// <param name="precision">Specifies the maximum number of digits used to store the number (inclusive of both sides of the decimal point).</param>
		/// <param name="scale">Specifies the number of digits used in the fractional portion of the number (i.e. digits to the right of the decimal point).</param>
		/// <returns>The DbParameterCollection to which the parameter was appended.</returns>
		public static DbParameterCollection AddPgNumericOutParameter(this DbParameterCollection prms, string parameterName, byte precision, byte scale)
		{
			var prm = new NpgsqlParameter(NormalizeSqlParameterName(parameterName), NpgsqlDbType.Numeric)
			{
				Direction = ParameterDirection.Output,
				Precision = precision,
				Scale = scale
			};
			prms.Add(prm);
			return prms;
		}
		//MONEY
		/// <summary>
		/// Creates a parameter for providing a decmial value to a stored procedure.
		/// </summary>
		/// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
		/// <param name="parameterName">The name of the parameter. If the name doesn’t start with “:”, it will be automatically pre-pended.</param>
		/// <param name="value">A decmial value .</param>
		/// <returns>The DbParameterCollection to which the parameter was appended.</returns>
		public static DbParameterCollection AddPgMoneyInParameter(this DbParameterCollection prms, string parameterName, decimal value)
		{
			var prm = new NpgsqlParameter(NormalizeSqlParameterName(parameterName), NpgsqlDbType.Money)
			{
				Value = value,
				Direction = ParameterDirection.Input
			};
			prms.Add(prm);
			return prms;
		}
		/// <summary>
		/// Creates a parameter for providing a decmial value or a DbNull value to a stored procedure.
		/// </summary>
		/// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
		/// <param name="parameterName">The name of the parameter. If the name doesn’t start with “:”, it will be automatically pre-pended.</param>
		/// <param name="value">A decmial value or null.</param>
		/// <returns>The DbParameterCollection to which the parameter was appended.</returns>
		public static DbParameterCollection AddPgMoneyInParameter(this DbParameterCollection prms, string parameterName, decimal? value)
		{
			var prm = new NpgsqlParameter(NormalizeSqlParameterName(parameterName), NpgsqlDbType.Money)
			{
				Value = value.HasValue ? (dynamic)value.Value : System.DBNull.Value,
				Direction = ParameterDirection.Input
			};
			prms.Add(prm);
			return prms;
		}
		/// <summary>
		/// Creates an output parameter for retrieving a decmial value from a stored procedure.
		/// </summary>
		/// <param name="prms">The existing parameter collection to which this output parameter should be added.</param>
		/// <param name="parameterName">The name of the parameter. If the name doesn’t start with “:”, it will be automatically pre-pended.</param>
		/// <param name="precision">Specifies the maximum number of digits used to store the number (inclusive of both sides of the decimal point).</param>
		/// <param name="scale">Specifies the number of digits used in the fractional portion of the number (i.e. digits to the right of the decimal point).</param>
		/// <returns>The DbParameterCollection to which the parameter was appended.</returns>
		public static DbParameterCollection AddPgMoneyOutParameter(this DbParameterCollection prms, string parameterName)
		{
			var prm = new NpgsqlParameter(NormalizeSqlParameterName(parameterName), NpgsqlDbType.Money)
			{
				Direction = ParameterDirection.Output
			};
			prms.Add(prm);
			return prms;
		}
		//DOUBLE
		/// <summary>
		/// Creates a parameter for providing a 64-bit floating-point value (double) to a stored procedure. NaN will be converted to DbNull.
		/// </summary>
		/// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
		/// <param name="parameterName">The name of the parameter. If the name doesn’t start with “:”, it will be automatically pre-pended.</param>
		/// <param name="value">A 64-bit floating-point value.</param>
		/// <returns>The DbParameterCollection to which the parameter was appended.</returns>
		public static DbParameterCollection AddPgDoubleInParameter(this DbParameterCollection prms, string parameterName, double value)
		{
			var prm = new NpgsqlParameter(NormalizeSqlParameterName(parameterName), NpgsqlDbType.Double)
			{
				Value = double.IsNaN(value) ? (dynamic)System.DBNull.Value : value,
				Direction = ParameterDirection.Input
			};
			prms.Add(prm);
			return prms;
		}
		/// <summary>
		/// Creates a parameter for providing a 64-bit floating-point value (double) or a DbNull value to a stored procedure.
		/// </summary>
		/// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
		/// <param name="parameterName">The name of the parameter. If the name doesn’t start with “:”, it will be automatically pre-pended.</param>
		/// <param name="value">A 64-bit floating-point value or null.</param>
		/// <returns>The DbParameterCollection to which the parameter was appended.</returns>
		public static DbParameterCollection AddPgDoubleInParameter(this DbParameterCollection prms, string parameterName, double? value)
		{
			var prm = new NpgsqlParameter(NormalizeSqlParameterName(parameterName), NpgsqlDbType.Double)
			{
				Value = value.HasValue ? (dynamic)value.Value : System.DBNull.Value,
				Direction = ParameterDirection.Input
			};
			prms.Add(prm);
			return prms;
		}
		/// <summary>
		/// Creates an output parameter for retrieving a 64-bit floating-point value (double) from a stored procedure.
		/// </summary>
		/// <param name="prms">The existing parameter collection to which this output parameter should be added.</param>
		/// <param name="parameterName">The name of the parameter. If the name doesn’t start with “:”, it will be automatically pre-pended.</param>
		/// <returns>The DbParameterCollection to which the parameter was appended.</returns>
		public static DbParameterCollection AddPgDoubleOutParameter(this DbParameterCollection prms, string parameterName)
		{
			var prm = new NpgsqlParameter(NormalizeSqlParameterName(parameterName), NpgsqlDbType.Double)
			{
				Direction = ParameterDirection.Output
			};
			prms.Add(prm);
			return prms;
		}
		//SINGLE
		/// <summary>
		/// Creates a parameter for providing a 32-bit floating-point value (float) to a stored procedure. NaN will be converted to DbNull.
		/// </summary>
		/// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
		/// <param name="parameterName">The name of the parameter. If the name doesn’t start with “:”, it will be automatically pre-pended.</param>
		/// <param name="value">A 32-bit floating point value (float).</param>
		/// <returns>The DbParameterCollection to which the parameter was appended.</returns>
		public static DbParameterCollection AddPgRealInParameter(this DbParameterCollection prms, string parameterName, float value)
		{
			var prm = new NpgsqlParameter(NormalizeSqlParameterName(parameterName), NpgsqlDbType.Real)
			{
				Value = float.IsNaN(value) ? (dynamic)System.DBNull.Value : value,
				Direction = ParameterDirection.Input
			};
			prms.Add(prm);
			return prms;
		}
		/// <summary>
		/// Creates a parameter for providing a 32-bit floating-point value (float) or DbNull value to a stored procedure.
		/// </summary>
		/// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
		/// <param name="parameterName">The name of the parameter. If the name doesn’t start with “:”, it will be automatically pre-pended.</param>
		/// <param name="value">A 32-bit floating point value (float) or null.</param>
		/// <returns>The DbParameterCollection to which the parameter was appended.</returns>
		public static DbParameterCollection AddPgRealInParameter(this DbParameterCollection prms, string parameterName, float? value)
		{
			var prm = new NpgsqlParameter(NormalizeSqlParameterName(parameterName), NpgsqlDbType.Real)
			{
				Value = value.HasValue ? (dynamic)value.Value : System.DBNull.Value,
				Direction = ParameterDirection.Input
			};
			prms.Add(prm);
			return prms;
		}
		/// <summary>
		/// Creates an output parameter for retrieving a 32-bit floating-point value (float) from a stored procedure.
		/// </summary>
		/// <param name="prms">The existing parameter collection to which this output parameter should be added.</param>
		/// <param name="parameterName">The name of the parameter. If the name doesn’t start with “:”, it will be automatically pre-pended.</param>
		/// <returns>The DbParameterCollection to which the parameter was appended.</returns>
		public static DbParameterCollection AddPgRealOutParameter(this DbParameterCollection prms, string parameterName)
		{
			var prm = new NpgsqlParameter(NormalizeSqlParameterName(parameterName), NpgsqlDbType.Real)
			{
				Direction = ParameterDirection.Output
			};
			prms.Add(prm);
			return prms;
		}
		#endregion
		#region Temporal types
		//TIMESTAMP
		/// <summary>
		/// Creates a parameter for providing a date and time value to a stored procedure.
		/// </summary>
		/// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
		/// <param name="parameterName">The name of the parameter. If the name doesn’t start with “:”, it will be automatically pre-pended.</param>
		/// <param name="value">A date and time value .</param>
		/// <returns>The DbParameterCollection to which the parameter was appended.</returns>
		public static DbParameterCollection AddPgTimestampInParameter(this DbParameterCollection prms, string parameterName, DateTime value)
		{
			var prm = new NpgsqlParameter(NormalizeSqlParameterName(parameterName), NpgsqlDbType.Timestamp)
			{
				Value = value,
				Direction = ParameterDirection.Input
			};
			prms.Add(prm);
			return prms;
		}
		/// <summary>
		/// Creates a parameter for providing a date and time value or a DbNull value to a stored procedure.
		/// </summary>
		/// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
		/// <param name="parameterName">The name of the parameter. If the name doesn’t start with “:”, it will be automatically pre-pended.</param>
		/// <param name="value">A date and time value or null.</param>
		/// <returns>The DbParameterCollection to which the parameter was appended.</returns>
		public static DbParameterCollection AddPgTimestampInParameter(this DbParameterCollection prms, string parameterName, DateTime? value)
		{
			var prm = new NpgsqlParameter(NormalizeSqlParameterName(parameterName), NpgsqlDbType.Timestamp)
			{
				Value = value.HasValue ? (dynamic)value.Value : System.DBNull.Value,
				Direction = ParameterDirection.Input
			};
			prms.Add(prm);
			return prms;
		}
		/// <summary>
		/// Creates an output parameter for retrieving a date and time value from a stored procedure.
		/// </summary>
		/// <param name="prms">The existing parameter collection to which this output parameter should be added.</param>
		/// <param name="parameterName">The name of the parameter. If the name doesn’t start with “:”, it will be automatically pre-pended.</param>
		/// <returns>The DbParameterCollection to which the parameter was appended.</returns>
		public static DbParameterCollection AddPgTimestampOutParameter(this DbParameterCollection prms, string parameterName)
		{
			var prm = new NpgsqlParameter(NormalizeSqlParameterName(parameterName), NpgsqlDbType.Timestamp)
			{
				Direction = ParameterDirection.Output
			};
			prms.Add(prm);
			return prms;
		}
		//TIMESTAMPTZ
		/// <summary>
		/// Creates a parameter for providing a DateTimeOffset value to a stored procedure.
		/// </summary>
		/// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
		/// <param name="parameterName">The name of the parameter. If the name doesn’t start with “:”, it will be automatically pre-pended.</param>
		/// <param name="value">A DateTimeOffset value.</param>
		/// <returns>The DbParameterCollection to which the parameter was appended.</returns>
		public static DbParameterCollection AddPgTimestampTZInParameter(this DbParameterCollection prms, string parameterName, DateTimeOffset value)
		{
			var prm = new NpgsqlParameter(NormalizeSqlParameterName(parameterName), NpgsqlDbType.TimestampTZ)
			{
				Value = value,
				Direction = ParameterDirection.Input
			};
			prms.Add(prm);
			return prms;
		}
		/// <summary>
		/// Creates a parameter for providing a DateTimeOffset or a DbNull value to a stored procedure.
		/// </summary>
		/// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
		/// <param name="parameterName">The name of the parameter. If the name doesn’t start with “:”, it will be automatically pre-pended.</param>
		/// <param name="value">A DateTimeOffset value or null.</param>
		/// <returns>The DbParameterCollection to which the parameter was appended.</returns>
		public static DbParameterCollection AddPgTimestampTZInParameter(this DbParameterCollection prms, string parameterName, DateTimeOffset? value)
		{
			var prm = new NpgsqlParameter(NormalizeSqlParameterName(parameterName), NpgsqlDbType.TimestampTZ)
			{
				Value = value.HasValue ? (dynamic)value.Value : System.DBNull.Value,
				Direction = ParameterDirection.Input
			};
			prms.Add(prm);
			return prms;
		}
		/// <summary>
		/// Creates an output parameter for retrieving a DateTimeOffset from a stored procedure.
		/// </summary>
		/// <param name="prms">The existing parameter collection to which this output parameter should be added.</param>
		/// <param name="parameterName">The name of the parameter. If the name doesn’t start with “:”, it will be automatically pre-pended.</param>
		/// <returns>The DbParameterCollection to which the parameter was appended.</returns>
		public static DbParameterCollection AddPgTimestampTZOutParameter(this DbParameterCollection prms, string parameterName)
		{
			var prm = new NpgsqlParameter(NormalizeSqlParameterName(parameterName), NpgsqlDbType.TimestampTZ)
			{
				Direction = ParameterDirection.Output
			};
			prms.Add(prm);
			return prms;
		}
		//DATE
		/// <summary>
		/// Creates a parameter for providing a date (sans time) to a stored procedure.
		/// </summary>
		/// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
		/// <param name="parameterName">The name of the parameter. If the name doesn’t start with “:”, it will be automatically pre-pended.</param>
		/// <param name="value">A DateTime value.</param>
		/// <returns>The DbParameterCollection to which the parameter was appended.</returns>
		public static DbParameterCollection AddPgDateInParameter(this DbParameterCollection prms, string parameterName, DateTime value)
		{
			var prm = new NpgsqlParameter(NormalizeSqlParameterName(parameterName), NpgsqlDbType.Date)
			{
				Value = value,
				Direction = ParameterDirection.Input
			};
			prms.Add(prm);
			return prms;
		}
		/// <summary>
		/// Creates a parameter for providing a date (sans time) or DbNull to a stored procedure.
		/// </summary>
		/// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
		/// <param name="parameterName">The name of the parameter. If the name doesn’t start with “:”, it will be automatically pre-pended.</param>
		/// <param name="value">A DateTime value or null.</param>
		/// <returns>The DbParameterCollection to which the parameter was appended.</returns>
		public static DbParameterCollection AddPgDateInParameter(this DbParameterCollection prms, string parameterName, DateTime? value)
		{
			var prm = new NpgsqlParameter(NormalizeSqlParameterName(parameterName), NpgsqlDbType.Date)
			{
				Value = value.HasValue ? (dynamic)value.Value : System.DBNull.Value,
				Direction = ParameterDirection.Input
			};
			prms.Add(prm);
			return prms;
		}
		/// <summary>
		/// Creates an output parameter for retrieving a date (sans time) from a stored procedure.
		/// </summary>
		/// <param name="prms">The existing parameter collection to which this output parameter should be added.</param>
		/// <param name="parameterName">The name of the parameter. If the name doesn’t start with “:”, it will be automatically pre-pended.</param>
		/// <returns>The DbParameterCollection to which the parameter was appended.</returns>
		public static DbParameterCollection AddPgDateOutParameter(this DbParameterCollection prms, string parameterName)
		{
			var prm = new NpgsqlParameter(NormalizeSqlParameterName(parameterName), NpgsqlDbType.Date)
			{
				Direction = ParameterDirection.Output
			};
			prms.Add(prm);
			return prms;
		}
		//TIME
		/// <summary>
		/// Creates a parameter for providing a time value to a stored procedure.
		/// </summary>
		/// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
		/// <param name="parameterName">The name of the parameter. If the name doesn’t start with “:”, it will be automatically pre-pended.</param>
		/// <param name="value">A time value .</param>
		/// <returns>The DbParameterCollection to which the parameter was appended.</returns>
		public static DbParameterCollection AddPgTimeInParameter(this DbParameterCollection prms, string parameterName, TimeSpan value)
		{
			var prm = new NpgsqlParameter(NormalizeSqlParameterName(parameterName), NpgsqlDbType.Time)
			{
				Value = value,
				Direction = ParameterDirection.Input
			};
			prms.Add(prm);
			return prms;
		}
		/// <summary>
		/// Creates a parameter for providing a time value or a DbNull value to a stored procedure.
		/// </summary>
		/// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
		/// <param name="parameterName">The name of the parameter. If the name doesn’t start with “:”, it will be automatically pre-pended.</param>
		/// <param name="value">A time value or null.</param>
		/// <returns>The DbParameterCollection to which the parameter was appended.</returns>
		public static DbParameterCollection AddPgTimeInParameter(this DbParameterCollection prms, string parameterName, TimeSpan? value)
		{
			var prm = new NpgsqlParameter(NormalizeSqlParameterName(parameterName), NpgsqlDbType.Time)
			{
				Value = value.HasValue ? (dynamic)value.Value : System.DBNull.Value,
				Direction = ParameterDirection.Input
			};
			prms.Add(prm);
			return prms;
		}
		/// <summary>
		/// Creates an output parameter for retrieving a time value from a stored procedure.
		/// </summary>
		/// <param name="prms">The existing parameter collection to which this output parameter should be added.</param>
		/// <param name="parameterName">The name of the parameter. If the name doesn’t start with “:”, it will be automatically pre-pended.</param>
		/// <returns>The DbParameterCollection to which the parameter was appended.</returns>
		public static DbParameterCollection AddPgTimeOutParameter(this DbParameterCollection prms, string parameterName)
		{
			var prm = new NpgsqlParameter(NormalizeSqlParameterName(parameterName), NpgsqlDbType.Time)
			{
				Direction = ParameterDirection.Output
			};
			prms.Add(prm);
			return prms;
		}
		//INTERVAL
		/// <summary>
		/// Creates a parameter for providing a time value to a stored procedure.
		/// </summary>
		/// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
		/// <param name="parameterName">The name of the parameter. If the name doesn’t start with “:”, it will be automatically pre-pended.</param>
		/// <param name="value">A time value .</param>
		/// <returns>The DbParameterCollection to which the parameter was appended.</returns>
		public static DbParameterCollection AddPgIntervalInParameter(this DbParameterCollection prms, string parameterName, TimeSpan value)
		{
			var prm = new NpgsqlParameter(NormalizeSqlParameterName(parameterName), NpgsqlDbType.Interval)
			{
				Value = value,
				Direction = ParameterDirection.Input
			};
			prms.Add(prm);
			return prms;
		}
		/// <summary>
		/// Creates a parameter for providing a time value or a DbNull value to a stored procedure.
		/// </summary>
		/// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
		/// <param name="parameterName">The name of the parameter. If the name doesn’t start with “:”, it will be automatically pre-pended.</param>
		/// <param name="value">A time value or null.</param>
		/// <returns>The DbParameterCollection to which the parameter was appended.</returns>
		public static DbParameterCollection AddPgIntervalInParameter(this DbParameterCollection prms, string parameterName, TimeSpan? value)
		{
			var prm = new NpgsqlParameter(NormalizeSqlParameterName(parameterName), NpgsqlDbType.Interval)
			{
				Value = value.HasValue ? (dynamic)value.Value : System.DBNull.Value,
				Direction = ParameterDirection.Input
			};
			prms.Add(prm);
			return prms;
		}
		/// <summary>
		/// Creates an output parameter for retrieving a time value from a stored procedure.
		/// </summary>
		/// <param name="prms">The existing parameter collection to which this output parameter should be added.</param>
		/// <param name="parameterName">The name of the parameter. If the name doesn’t start with “:”, it will be automatically pre-pended.</param>
		/// <returns>The DbParameterCollection to which the parameter was appended.</returns>
		public static DbParameterCollection AddPgIntervalOutParameter(this DbParameterCollection prms, string parameterName)
		{
			var prm = new NpgsqlParameter(NormalizeSqlParameterName(parameterName), NpgsqlDbType.Interval)
			{
				Direction = ParameterDirection.Output
			};
			prms.Add(prm);
			return prms;
		}
		//TIMETZ
		/// <summary>
		/// Creates a parameter for providing a time value to a stored procedure.
		/// </summary>
		/// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
		/// <param name="parameterName">The name of the parameter. If the name doesn’t start with “:”, it will be automatically pre-pended.</param>
		/// <param name="value">A time value .</param>
		/// <returns>The DbParameterCollection to which the parameter was appended.</returns>
		public static DbParameterCollection AddPgTimeTZInParameter(this DbParameterCollection prms, string parameterName, TimeSpan value)
		{
			var prm = new NpgsqlParameter(NormalizeSqlParameterName(parameterName), NpgsqlDbType.TimeTZ)
			{
				Value = value,
				Direction = ParameterDirection.Input
			};
			prms.Add(prm);
			return prms;
		}
		/// <summary>
		/// Creates a parameter for providing a time value to a stored procedure.
		/// </summary>
		/// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
		/// <param name="parameterName">The name of the parameter. If the name doesn’t start with “:”, it will be automatically pre-pended.</param>
		/// <param name="value">A time value .</param>
		/// <returns>The DbParameterCollection to which the parameter was appended.</returns>
		public static DbParameterCollection AddPgTimeTZInParameter(this DbParameterCollection prms, string parameterName, DateTimeOffset value)
		{
			var prm = new NpgsqlParameter(NormalizeSqlParameterName(parameterName), NpgsqlDbType.TimeTZ)
			{
				Value = value,
				Direction = ParameterDirection.Input
			};
			prms.Add(prm);
			return prms;
		}
		/// <summary>
		/// Creates a parameter for providing a time value or a DbNull value to a stored procedure.
		/// </summary>
		/// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
		/// <param name="parameterName">The name of the parameter. If the name doesn’t start with “:”, it will be automatically pre-pended.</param>
		/// <param name="value">A time value or null.</param>
		/// <returns>The DbParameterCollection to which the parameter was appended.</returns>
		public static DbParameterCollection AddPgTimeTZInParameter(this DbParameterCollection prms, string parameterName, TimeSpan? value)
		{
			var prm = new NpgsqlParameter(NormalizeSqlParameterName(parameterName), NpgsqlDbType.TimeTZ)
			{
				Value = value.HasValue ? (dynamic)value.Value : System.DBNull.Value,
				Direction = ParameterDirection.Input
			};
			prms.Add(prm);
			return prms;
		}
		/// <summary>
		/// Creates a parameter for providing a time value or a DbNull value to a stored procedure.
		/// </summary>
		/// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
		/// <param name="parameterName">The name of the parameter. If the name doesn’t start with “:”, it will be automatically pre-pended.</param>
		/// <param name="value">A time value or null.</param>
		/// <returns>The DbParameterCollection to which the parameter was appended.</returns>
		public static DbParameterCollection AddPgTimeTZInParameter(this DbParameterCollection prms, string parameterName, DateTimeOffset? value)
		{
			var prm = new NpgsqlParameter(NormalizeSqlParameterName(parameterName), NpgsqlDbType.TimeTZ)
			{
				Value = value.HasValue ? (dynamic)value.Value : System.DBNull.Value,
				Direction = ParameterDirection.Input
			};
			prms.Add(prm);
			return prms;
		}
		/// <summary>
		/// Creates an output parameter for retrieving a time value from a stored procedure.
		/// </summary>
		/// <param name="prms">The existing parameter collection to which this output parameter should be added.</param>
		/// <param name="parameterName">The name of the parameter. If the name doesn’t start with “:”, it will be automatically pre-pended.</param>
		/// <returns>The DbParameterCollection to which the parameter was appended.</returns>
		public static DbParameterCollection AddPgTimeTZOutParameter(this DbParameterCollection prms, string parameterName)
		{
			var prm = new NpgsqlParameter(NormalizeSqlParameterName(parameterName), NpgsqlDbType.TimeTZ)
			{
				Direction = ParameterDirection.Output
			};
			prms.Add(prm);
			return prms;
		}
		#endregion
		#region Other types
		//ARRAY
		/// <summary>
		/// Creates a parameter for providing a variable-sized byte array to a stored procedure. A null reference will save DBNull.
		/// </summary>
		/// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
		/// <param name="parameterName">The name of the parameter. If the name doesn’t start with “:”, it will be automatically pre-pended.</param>
		/// <param name="value">An array, or null.</param>
		/// <param name="length">The maximum allowable number of bytes in the database column. Use -1 for varbinary(max).</param>
		/// <returns>The DbParameterCollection to which the parameter was appended.</returns>
		public static DbParameterCollection AddPgArrayInParameter<T>(this DbParameterCollection prms, string parameterName, Array value)
		{
			var prm = new NpgsqlParameter(NormalizeSqlParameterName(parameterName), NpgsqlDbType.Array)
			{
				Value = value == null ? (dynamic)System.DBNull.Value : value,
				Direction = ParameterDirection.Input
			};
			prms.Add(prm);
			return prms;
		}
		/// <summary>
		/// Creates a parameter for obtaining a variable-sized byte array from a stored procedure.
		/// </summary>
		/// <param name="prms">The existing parameter collection to which this output parameter should be added.</param>
		/// <param name="parameterName">The name of the parameter. If the name doesn’t start with “:”, it will be automatically pre-pended.</param>
		/// <param name="length">The maximum allowable number of bytes in the database column. Use -1 for varbinary(max).</param>
		/// <returns>The DbParameterCollection to which the parameter was appended.</returns>
		public static DbParameterCollection AddPgArrayOutParameter(this DbParameterCollection prms, string parameterName)
		{
			var prm = new NpgsqlParameter(NormalizeSqlParameterName(parameterName), NpgsqlDbType.Array)
			{
				Direction = ParameterDirection.Output
			};
			prms.Add(prm);
			return prms;
		}
		//BYTEA
		/// <summary>
		/// Creates a parameter for providing a variable-sized byte array to a stored procedure. A null reference will save DBNull.
		/// </summary>
		/// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
		/// <param name="parameterName">The name of the parameter. If the name doesn’t start with “:”, it will be automatically pre-pended.</param>
		/// <param name="value">An array of bytes, or null.</param>
		/// <param name="length">The maximum allowable number of bytes in the database column. Use -1 for varbinary(max).</param>
		/// <returns>The DbParameterCollection to which the parameter was appended.</returns>
		public static DbParameterCollection AddPgByteaInParameter(this DbParameterCollection prms, string parameterName, byte[] value, int length)
		{
			var prm = new NpgsqlParameter(NormalizeSqlParameterName(parameterName), NpgsqlDbType.Bytea, length)
			{
				Value = value == null ? (dynamic)System.DBNull.Value : value,
				Direction = ParameterDirection.Input
			};
			prms.Add(prm);
			return prms;
		}
		/// <summary>
		/// Creates a parameter for obtaining a variable-sized byte array from a stored procedure.
		/// </summary>
		/// <param name="prms">The existing parameter collection to which this output parameter should be added.</param>
		/// <param name="parameterName">The name of the parameter. If the name doesn’t start with “:”, it will be automatically pre-pended.</param>
		/// <param name="length">The maximum allowable number of bytes in the database column. Use -1 for varbinary(max).</param>
		/// <returns>The DbParameterCollection to which the parameter was appended.</returns>
		public static DbParameterCollection AddPgByteaOutParameter(this DbParameterCollection prms, string parameterName, int length)
		{
			var prm = new NpgsqlParameter(NormalizeSqlParameterName(parameterName), NpgsqlDbType.Bytea, length)
			{
				Direction = ParameterDirection.Output
			};
			prms.Add(prm);
			return prms;
		}
		//HSTORE
		/// <summary>
		/// Creates a parameter for providing a variable-sized byte array to a stored procedure. A null reference will save DBNull.
		/// </summary>
		/// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
		/// <param name="parameterName">The name of the parameter. If the name doesn’t start with “:”, it will be automatically pre-pended.</param>
		/// <param name="value">An array of bytes, or null.</param>
		/// <param name="length">The maximum allowable number of bytes in the database column. Use -1 for varbinary(max).</param>
		/// <returns>The DbParameterCollection to which the parameter was appended.</returns>
		public static DbParameterCollection AddPgHstoreInParameter(this DbParameterCollection prms, string parameterName, IDictionary<string, string> value, int length)
		{
			var prm = new NpgsqlParameter(NormalizeSqlParameterName(parameterName), NpgsqlDbType.Hstore, length)
			{
				Value = value == null ? (dynamic)System.DBNull.Value : value,
				Direction = ParameterDirection.Input
			};
			prms.Add(prm);
			return prms;
		}
		/// <summary>
		/// Creates a parameter for obtaining a variable-sized byte array from a stored procedure.
		/// </summary>
		/// <param name="prms">The existing parameter collection to which this output parameter should be added.</param>
		/// <param name="parameterName">The name of the parameter. If the name doesn’t start with “:”, it will be automatically pre-pended.</param>
		/// <param name="length">The maximum allowable number of bytes in the database column. Use -1 for varbinary(max).</param>
		/// <returns>The DbParameterCollection to which the parameter was appended.</returns>
		public static DbParameterCollection AddPgHstoreOutParameter(this DbParameterCollection prms, string parameterName, int length)
		{
			var prm = new NpgsqlParameter(NormalizeSqlParameterName(parameterName), NpgsqlDbType.Bytea, length)
			{
				Direction = ParameterDirection.Output
			};
			prms.Add(prm);
			return prms;
		}
		//UUID
		/// <summary>
		/// Creates a parameter for providing a Guid or DBNull (via Guid.Empty) to a stored procedure.
		/// </summary>
		/// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
		/// <param name="parameterName">The name of the parameter. If the name doesn’t start with “:”, it will be automatically pre-pended.</param>
		/// <param name="value">A Guid value. Will convert Guild.Empty to DBNull.</param>
		/// <returns>The DbParameterCollection to which the parameter was appended.</returns>
		public static DbParameterCollection AddPgUuidInParameter(this DbParameterCollection prms, string parameterName, Guid value)
		{
			var prm = new NpgsqlParameter(NormalizeSqlParameterName(parameterName), NpgsqlDbType.Uuid)
			{
				Value = Guid.Empty.Equals(value) ? (dynamic)System.DBNull.Value : value,
				Direction = ParameterDirection.Input
			};
			prms.Add(prm);
			return prms;
		}
		/// <summary>
		/// Creates a parameter for providing a Guid or DBNull (via null value) to a stored procedure.
		/// </summary>
		/// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
		/// <param name="parameterName">The name of the parameter. If the name doesn’t start with “:”, it will be automatically pre-pended.</param>
		/// <returns>The DbParameterCollection to which the parameter was appended.</returns>
		public static DbParameterCollection AddPgUuidInParameter(this DbParameterCollection prms, string parameterName, Guid? value)
		{
			var prm = new NpgsqlParameter(NormalizeSqlParameterName(parameterName), NpgsqlDbType.Uuid)
			{
				Value = value.HasValue ? (dynamic)value.Value : System.DBNull.Value,
				Direction = ParameterDirection.Input
			};
			prms.Add(prm);
			return prms;
		}
		/// <summary>
		/// Creates an output parameter for retrieving a Guid from a stored procedure.
		/// </summary>
		/// <param name="prms">The existing parameter collection to which this output parameter should be added.</param>
		/// <param name="parameterName">The name of the parameter. If the name doesn’t start with “:”, it will be automatically pre-pended.</param>
		/// <returns>The DbParameterCollection to which the parameter was appended.</returns>
		public static DbParameterCollection AddPgUuidOutParameter(this DbParameterCollection prms, string parameterName)
		{
			var prm = new NpgsqlParameter(NormalizeSqlParameterName(parameterName), NpgsqlDbType.Uuid)
			{
				Direction = ParameterDirection.Output
			};
			prms.Add(prm);
			return prms;
		}
		//ENUM
		/// <summary>
		/// Creates a parameter for providing a fixed-sized byte array to a stored procedure. A null reference will save DBNull.
		/// </summary>
		/// <param name="prms">The existing parameter collection to which this parameter should be added.</param>
		/// <param name="parameterName">The name of the parameter. If the name doesn’t start with “:”, it will be automatically pre-pended.</param>
		/// <param name="value">An array of bytes, or null.</param>
		/// <param name="length">The fixed number of bytes in the database column.</param>
		/// <returns>The DbParameterCollection to which the parameter was appended.</returns>
		public static DbParameterCollection AddPgEnumInParameter(this DbParameterCollection prms, string parameterName, Enum value)
		{
			var prm = new NpgsqlParameter(NormalizeSqlParameterName(parameterName), NpgsqlDbType.Enum)
			{
				Value = value == null ? (dynamic)System.DBNull.Value : value,
				Direction = ParameterDirection.Input
			};
			prms.Add(prm);
			return prms;
		}
		/// <summary>
		/// Creates a parameter for obtaining a fixed-sized byte array from a stored procedure.
		/// </summary>
		/// <param name="prms">The existing parameter collection to which this output parameter should be added.</param>
		/// <param name="parameterName">The name of the parameter. If the name doesn’t start with “:”, it will be automatically pre-pended.</param>
		/// <param name="length">The fixed number of bytes in the database column.</param>
		/// <returns>The DbParameterCollection to which the parameter was appended.</returns>
		public static DbParameterCollection AddPgEnumOutParameter(this DbParameterCollection prms, string parameterName)
		{
			var prm = new NpgsqlParameter(NormalizeSqlParameterName(parameterName), NpgsqlDbType.Enum)
			{
				Direction = ParameterDirection.Output
			};
			prms.Add(prm);
			return prms;
		}
		#endregion
		public static Enum GetEnum(this DbParameter prm) 
			=> (Enum)prm.Value;

	}
}
