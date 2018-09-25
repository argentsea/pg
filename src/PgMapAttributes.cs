using System;
using System.Collections.Generic;
using System.Text;
using NpgsqlTypes;
using System.Data;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using ArgentSea;
using System.Reflection;


namespace ArgentSea.Pg
{
    /// <summary>
    /// This abstract class is a PostgreSQL-specific implementation of the ParameterMapAttribute class.
    /// </summary>
	public abstract class PgParameterMapAttribute : ParameterMapAttributeBase
	{
		public PgParameterMapAttribute(string parameterName, NpgsqlDbType pgType) : base(parameterName, (int)pgType)
		{
		}
		public PgParameterMapAttribute(string parameterName, NpgsqlDbType pgType, bool isRequired) : base(parameterName, (int)pgType, isRequired)
		{
		}

    }

    #region String parameters
    /// <summary>
    /// This attribute maps a model property to/from a PostgreSQL VarChar parameter or column.
    /// </summary>
    public class MapToPgVarCharAttribute : PgParameterMapAttribute
	{
		/// <summary>
		/// Map this property to the specified Unicode database column, with a variable but maximum length.
		/// </summary>
		/// <param name="parameterName">The name of the parameter or column that contains the value. The system will automatically add or remove the prefix ':' as needed.</param>
		/// <param name="length">The maximum length of the string.</param>
		public MapToPgVarCharAttribute(string parameterName, int length) : base(parameterName, NpgsqlDbType.Varchar)
		{
			this.Length = length;
		}
		public MapToPgVarCharAttribute(string parameterName, int length, bool isRequired) : base(parameterName, NpgsqlDbType.Varchar, isRequired)
		{
			this.Length = length;
		}
		public int Length { get; private set; }

		public override bool IsValidType(Type candidateType)
			=> candidateType.IsEnum || candidateType == typeof(string) || (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == typeof(Nullable<>) && Nullable.GetUnderlyingType(candidateType).IsEnum);

		protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Expression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.InParameterStringExpressionBuilder(this.ParameterName, this.Length, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgVarcharInParameter), null, expressions, expSprocParameters, expIgnoreParameters, parameterNames, expProperty, propertyType, expLogger, logger);

		protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.OutParameterBuilder(this.ParameterName, expSprocParameters, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgVarcharOutParameter), Expression.Constant(this.Length, typeof(int)), null, parameterNames, expIgnoreParameters, logger);

		protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReadOutParameterStringExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetString), expProperty, expressions, expPrms, expPrm, propertyType, expLogger, logger);

		protected override void AppendReaderExpressions(Expression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReaderStringExpressions(this.ParameterName, expProperty, columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);
	}

    /// <summary>
    /// This attribute maps a model property to/from a PostgreSQL Char parameter or column.
    /// </summary>
	public class MapToPgCharAttribute : PgParameterMapAttribute
	{
		/// <summary>
		/// Map this property to the specified Unicode fixed-size database column.
		/// </summary>
		/// <param name="parameterName">The name of the parameter or column that contains the value. The system will automatically add or remove the prefix ':' as needed.</param>
		/// <param name="length">The length of the fixed-size string.</param>
		public MapToPgCharAttribute(string parameterName, int length) : base(parameterName, NpgsqlDbType.Char)
		{
			this.Length = length;
		}
		public MapToPgCharAttribute(string parameterName, int length, bool isRequired) : base(parameterName, NpgsqlDbType.Char, isRequired)
		{
			this.Length = length;
		}
		public int Length { get; private set; }

		public override bool IsValidType(Type candidateType)
			=> candidateType.IsEnum || candidateType == typeof(string) || (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == typeof(Nullable<>) && Nullable.GetUnderlyingType(candidateType).IsEnum);

		protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Expression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.InParameterStringExpressionBuilder(this.ParameterName, this.Length, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgCharInParameter), null, expressions, expSprocParameters, expIgnoreParameters, parameterNames, expProperty, propertyType, expLogger, logger);


		protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.OutParameterBuilder(this.ParameterName, expSprocParameters, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgCharOutParameter), Expression.Constant(this.Length, typeof(int)), null, parameterNames, expIgnoreParameters, logger);

		protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReadOutParameterStringExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetString), expProperty, expressions, expPrms, expPrm, propertyType, expLogger, logger);

		protected override void AppendReaderExpressions(Expression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReaderStringExpressions(this.ParameterName, expProperty, columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);
	}

    /// <summary>
    /// This attribute maps a model property to/from a PostgreSQL Text parameter or column.
    /// </summary>
	public class MapToPgTextAttribute : PgParameterMapAttribute
	{
		/// <summary>
		/// Map this property to the specified Unicode database column, with any size length.
		/// </summary>
		/// <param name="parameterName">The name of the parameter or column that contains the value. The system will automatically add or remove the prefix ':' as needed.</param>
		public MapToPgTextAttribute(string parameterName) : base(parameterName, NpgsqlDbType.Text)
		{
			//
		}
		public MapToPgTextAttribute(string parameterName, bool isRequired) : base(parameterName, NpgsqlDbType.Text, isRequired)
		{
			//
		}
		public override bool IsValidType(Type candidateType)
			=> candidateType == typeof(string);
			//=> candidateType.IsEnum || candidateType == typeof(string) || (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == typeof(Nullable<>) && Nullable.GetUnderlyingType(candidateType).IsEnum);

		protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Expression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.InParameterSimpleBuilder(this.ParameterName, propertyType, expSprocParameters, expIgnoreParameters, expProperty, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgTextInParameter), null, null, parameterNames, expLogger, logger);
			//=> ExpressionHelpers.InParameterStringExpressionBuilder(this.ParameterName, this.Length, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgTextInParameter), null, expressions, expSprocParameters, expIgnoreParameters, parameterNames, expProperty, propertyType, expLogger, logger);

		protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.OutParameterBuilder(this.ParameterName, expSprocParameters, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgTextOutParameter), null, null, parameterNames, expIgnoreParameters, logger);

		protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReadOutParameterStringExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetString), expProperty, expressions, expPrms, expPrm, propertyType, expLogger, logger);

		protected override void AppendReaderExpressions(Expression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmPgRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReaderStringExpressions(this.ParameterName, expProperty, columnLookupExpressions, expressions, prmPgRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);
	}
    #endregion
    #region Number parameters
    /// <summary>
    /// This attribute maps a model property to/from a PostgreSQL Bigint parameter or column.
    /// </summary>
    public class MapToPgBigintAttribute : PgParameterMapAttribute
	{
		/// <summary>
		/// Map this property to the specified BigInt (64-bit) database column.
		/// </summary>
		/// <param name="parameterName">The name of the parameter or column that contains the value. The system will automatically add or remove the prefix '@' as needed.</param>
		public MapToPgBigintAttribute(string parameterName) : base(parameterName, NpgsqlDbType.Bigint)
		{
			//
		}
		public MapToPgBigintAttribute(string parameterName, bool isRequired) : base(parameterName, NpgsqlDbType.Bigint, isRequired)
		{
			//
		}

		public override bool IsValidType(Type candidateType)
			=> candidateType == typeof(long)
			|| candidateType.IsEnum
			|| (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == typeof(Nullable<>) && Nullable.GetUnderlyingType(candidateType) == typeof(long));

		protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Expression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.InParameterEnumXIntExpressionBuilder(this.ParameterName, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgBigintInParameter), typeof(long?), expressions, expSprocParameters, expIgnoreParameters, parameterNames, expProperty, propertyType, expLogger, logger);

		protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.OutParameterBuilder(this.ParameterName, expSprocParameters, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgBigintOutParameter), null, null, parameterNames, expIgnoreParameters, logger);

		protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReadOutParameterEnumXIntExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetLong), nameof(DbParameterCollectionExtensions.GetNullableLong), expProperty, expressions, expPrms, expPrm, propertyType, expLogger, logger);

		protected override void AppendReaderExpressions(Expression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReaderEnumXIntExpressions(this.ParameterName, expProperty, typeof(long), columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);
	}

    /// <summary>
    /// This attribute maps a model property to/from a PostgreSQL Integer parameter or column.
    /// </summary>
	public class MapToPgIntegerAttribute : PgParameterMapAttribute
	{
		/// <summary>
		/// Map this property to the specified Int (32-bit) database column.
		/// </summary>
		/// <param name="parameterName">The name of the parameter or column that contains the value. The system will automatically add or remove the prefix '@' as needed.</param>
		public MapToPgIntegerAttribute(string parameterName) : base(parameterName, NpgsqlDbType.Integer)
		{
			//
		}
		public MapToPgIntegerAttribute(string parameterName, bool isRequired) : base(parameterName, NpgsqlDbType.Integer, isRequired)
		{
			//
		}

		public override bool IsValidType(Type candidateType)
			=> candidateType == typeof(int)
			|| candidateType.IsEnum
			|| (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == typeof(Nullable<>) && (Nullable.GetUnderlyingType(candidateType) == typeof(int) || Nullable.GetUnderlyingType(candidateType).IsEnum));

		protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Expression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.InParameterEnumXIntExpressionBuilder(this.ParameterName, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgIntegerInParameter), typeof(int?), expressions, expSprocParameters, expIgnoreParameters, parameterNames, expProperty, propertyType, expLogger, logger);

		protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.OutParameterBuilder(this.ParameterName, expSprocParameters, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgIntegerOutParameter), null, null, parameterNames, expIgnoreParameters, logger);

		protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReadOutParameterEnumXIntExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetInteger), nameof(DbParameterCollectionExtensions.GetNullableInteger), expProperty, expressions, expPrms, expPrm, propertyType, expLogger, logger);

		protected override void AppendReaderExpressions(Expression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReaderEnumXIntExpressions(this.ParameterName, expProperty, typeof(int), columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);
	}

    /// <summary>
    /// This attribute maps a model property to/from a PostgreSQL Smallint parameter or column.
    /// </summary>
    public class MapToPgSmallintAttribute : PgParameterMapAttribute
	{
		/// <summary>
		/// Map this property to the specified SmallInt (16-bit) database column.
		/// </summary>
		/// <param name="parameterName">The name of the parameter or column that contains the value. The system will automatically add or remove the prefix '@' as needed.</param>
		public MapToPgSmallintAttribute(string parameterName) : base(parameterName, NpgsqlDbType.Smallint)
		{
			//
		}
		public MapToPgSmallintAttribute(string parameterName, bool isRequired) : base(parameterName, NpgsqlDbType.Smallint, isRequired)
		{
			//
		}

		public override bool IsValidType(Type candidateType)
			=> candidateType == typeof(short)
			|| candidateType.IsEnum
			|| (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == typeof(Nullable<>) && (Nullable.GetUnderlyingType(candidateType) == typeof(short) || Nullable.GetUnderlyingType(candidateType).IsEnum));

		protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Expression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.InParameterEnumXIntExpressionBuilder(this.ParameterName, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgSmallintInParameter), typeof(short?), expressions, expSprocParameters, expIgnoreParameters, parameterNames, expProperty, propertyType, expLogger, logger);

		protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.OutParameterBuilder(this.ParameterName, expSprocParameters, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgSmallintOutParameter), null, null, parameterNames, expIgnoreParameters, logger);

		protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReadOutParameterEnumXIntExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetShort), nameof(DbParameterCollectionExtensions.GetNullableShort), expProperty, expressions, expPrms, expPrm, propertyType, expLogger, logger);

		protected override void AppendReaderExpressions(Expression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReaderEnumXIntExpressions(this.ParameterName, expProperty, typeof(short), columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);
	}

    /// <summary>
    /// This attribute maps a model property to/from a PostgreSQL InternalChar parameter or column.
    /// </summary>
    public class MapToPgInternalCharAttribute : PgParameterMapAttribute
	{
		/// <summary>
		/// Map this property to the specified TinyInt (unsigned 8-bit) database column.
		/// </summary>
		/// <param name="parameterName">The name of the parameter or column that contains the value. The system will automatically add or remove the prefix '@' as needed.</param>
		public MapToPgInternalCharAttribute(string parameterName) : base(parameterName, NpgsqlDbType.InternalChar)
		{
			//
		}
		public MapToPgInternalCharAttribute(string parameterName, bool isRequired) : base(parameterName, NpgsqlDbType.InternalChar, isRequired)
		{
			//
		}

		public override bool IsValidType(Type candidateType)
			=> candidateType == typeof(byte)
			|| candidateType.IsEnum
			|| (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == typeof(Nullable<>) && (Nullable.GetUnderlyingType(candidateType) == typeof(byte) || Nullable.GetUnderlyingType(candidateType).IsEnum));

		protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Expression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.InParameterEnumXIntExpressionBuilder(this.ParameterName, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgInternalCharInParameter), typeof(byte?), expressions, expSprocParameters, expIgnoreParameters, parameterNames, expProperty, propertyType, expLogger, logger);

		protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.OutParameterBuilder(this.ParameterName, expSprocParameters, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgInternalCharOutParameter), null, null, parameterNames, expIgnoreParameters, logger);

		protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReadOutParameterEnumXIntExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetByte), nameof(DbParameterCollectionExtensions.GetNullableByte), expProperty, expressions, expPrms, expPrm, propertyType, expLogger, logger);

		protected override void AppendReaderExpressions(Expression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReaderEnumXIntExpressions(this.ParameterName, expProperty, typeof(byte), columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);
	}

    /// <summary>
    /// This attribute maps a model property to/from a PostgreSQL Boolean parameter or column.
    /// </summary>
	public class MapToPgBooleanAttribute : PgParameterMapAttribute
	{
		/// <summary>
		/// Map this property to the specified Bit (boolean) database column.
		/// </summary>
		/// <param name="parameterName">The name of the parameter or column that contains the value. The system will automatically add or remove the prefix '@' as needed.</param>
		public MapToPgBooleanAttribute(string parameterName) : base(parameterName, NpgsqlDbType.Boolean)
		{
			//
		}
		public MapToPgBooleanAttribute(string parameterName, bool isRequired) : base(parameterName, NpgsqlDbType.Boolean, isRequired)
		{
			//
		}

		public override bool IsValidType(Type candidateType)
			=> candidateType == typeof(bool) || (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == typeof(Nullable<>) && Nullable.GetUnderlyingType(candidateType) == typeof(bool));

		protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Expression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.InParameterSimpleBuilder(this.ParameterName, propertyType, expSprocParameters, expIgnoreParameters, expProperty, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgBooleanInParameter), null, null, parameterNames, expLogger, logger);

		protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.OutParameterBuilder(this.ParameterName, expSprocParameters, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgBooleanOutParameter), null, null, parameterNames, expIgnoreParameters, logger);

		protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReadOutParameterSimpleValueExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetBoolean), nameof(DbParameterCollectionExtensions.GetNullableBoolean), expProperty, expressions, expPrms, expPrm, propertyType, expLogger, logger);

		protected override void AppendReaderExpressions(Expression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReaderSimpleValueExpressions(this.ParameterName, expProperty, columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);


	}

    /// <summary>
    /// This attribute maps a model property to/from a PostgreSQL Numeric parameter or column.
    /// </summary>
	public class MapToPgNumericAttribute : PgParameterMapAttribute
	{
		/// <summary>
		/// Map this property to the specified decimal database column.
		/// </summary>
		/// <param name="parameterName">The name of the parameter or column that contains the value. The system will automatically add or remove the prefix '@' as needed.</param>
		/// <param name="precision">The maximum number of digits in the database value.</param>
		/// <param name="scale">The number of digits to the right of the decimal point.</param>
		public MapToPgNumericAttribute(string parameterName, byte precision, byte scale) : base(parameterName, NpgsqlDbType.Numeric)
		{
			Precision = precision;
			Scale = scale;
		}
		public MapToPgNumericAttribute(string parameterName, byte precision, byte scale, bool isRequired) : base(parameterName, NpgsqlDbType.Numeric, isRequired)
		{
			Precision = precision;
			Scale = scale;
		}

		public byte Scale { get; private set; }

		public byte Precision { get; private set; }

		public override bool IsValidType(Type candidateType)
			=> candidateType == typeof(decimal) || (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == typeof(Nullable<>) && Nullable.GetUnderlyingType(candidateType) == typeof(decimal));

		protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Expression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.InParameterSimpleBuilder(this.ParameterName, propertyType, expSprocParameters, expIgnoreParameters, expProperty, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgNumericInParameter), Expression.Constant(this.Precision, typeof(byte)), Expression.Constant(this.Scale, typeof(byte)), parameterNames, expLogger, logger);

		protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.OutParameterBuilder(this.ParameterName, expSprocParameters, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgNumericOutParameter), Expression.Constant(this.Precision, typeof(byte)), Expression.Constant(this.Scale, typeof(byte)), parameterNames, expIgnoreParameters, logger);

		protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReadOutParameterSimpleValueExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetDecimal), nameof(DbParameterCollectionExtensions.GetNullableDecimal), expProperty, expressions, expPrms, expPrm, propertyType, expLogger, logger);

		protected override void AppendReaderExpressions(Expression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReaderSimpleValueExpressions(this.ParameterName, expProperty, columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);
	}

    /// <summary>
    /// This attribute maps a model property to/from a PostgreSQL Money parameter or column.
    /// </summary>
    public class MapToPgMoneyAttribute : PgParameterMapAttribute
	{
		/// <summary>
		/// Map this property to the specified Money database column.
		/// </summary>
		/// <param name="parameterName">The name of the parameter or column that contains the value. The system will automatically add or remove the prefix '@' as needed.</param>
		public MapToPgMoneyAttribute(string parameterName) : base(parameterName, NpgsqlDbType.Money)
		{
			//
		}
		public MapToPgMoneyAttribute(string parameterName, bool isRequired) : base(parameterName, NpgsqlDbType.Money, isRequired)
		{
			//
		}

		public override bool IsValidType(Type candidateType)
			=> candidateType == typeof(decimal) || (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == typeof(Nullable<>) && Nullable.GetUnderlyingType(candidateType) == typeof(decimal));

		protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Expression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.InParameterSimpleBuilder(this.ParameterName, propertyType, expSprocParameters, expIgnoreParameters, expProperty, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgMoneyInParameter), null, null, parameterNames, expLogger, logger);

		protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.OutParameterBuilder(this.ParameterName, expSprocParameters, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgMoneyOutParameter), null, null, parameterNames, expIgnoreParameters, logger);

		protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReadOutParameterSimpleValueExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetDecimal), nameof(DbParameterCollectionExtensions.GetNullableDecimal), expProperty, expressions, expPrms, expPrm, propertyType, expLogger, logger);

		protected override void AppendReaderExpressions(Expression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReaderSimpleValueExpressions(this.ParameterName, expProperty, columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);
	}

    /// <summary>
    /// This attribute maps a model property to/from a PostgreSQL Double parameter or column.
    /// </summary>
    public class MapToPgDoubleAttribute : PgParameterMapAttribute
	{
		/// <summary>
		/// Map this property to the specified Float (64-bit floating point or .NET double) database column.
		/// </summary>
		/// <param name="parameterName">The name of the parameter or column that contains the value. The system will automatically add or remove the prefix '@' as needed.</param>
		public MapToPgDoubleAttribute(string parameterName) : base(parameterName, NpgsqlDbType.Double)
		{
			//
		}
		public MapToPgDoubleAttribute(string parameterName, bool isRequired) : base(parameterName, NpgsqlDbType.Double, isRequired)
		{
			//
		}

		public override bool IsValidType(Type candidateType)
			=> candidateType == typeof(double) || (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == typeof(Nullable<>) && Nullable.GetUnderlyingType(candidateType) == typeof(double));

		protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Expression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.InParameterSimpleBuilder(this.ParameterName, propertyType, expSprocParameters, expIgnoreParameters, expProperty, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgDoubleInParameter), null, null, parameterNames, expLogger, logger);


		protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.OutParameterBuilder(this.ParameterName, expSprocParameters, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgDoubleOutParameter), null, null, parameterNames, expIgnoreParameters, logger);

		protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReadOutParameterSimpleValueExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetDouble), nameof(DbParameterCollectionExtensions.GetNullableDouble), expProperty, expressions, expPrms, expPrm, propertyType, expLogger, logger);

		protected override void AppendReaderExpressions(Expression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReaderNullableValueTypeExpressions(this.ParameterName, expProperty, Expression.Constant(double.NaN, typeof(double)), columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);
	}

    /// <summary>
    /// This attribute maps a model property to/from a PostgreSQL Real parameter or column.
    /// </summary>
	public class MapToPgRealAttribute : PgParameterMapAttribute
	{
		/// <summary>
		/// Map this property to the specified Real (32-bit floating point or .NET float) database column.
		/// </summary>
		/// <param name="parameterName">The name of the parameter or column that contains the value. The system will automatically add or remove the prefix '@' as needed.</param>
		public MapToPgRealAttribute(string parameterName) : base(parameterName, NpgsqlDbType.Real)
		{
			//
		}
		public MapToPgRealAttribute(string parameterName, bool isRequired) : base(parameterName, NpgsqlDbType.Real, isRequired)
		{
			//
		}

		public override bool IsValidType(Type candidateType)
			=> candidateType == typeof(float) || (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == typeof(Nullable<>) && Nullable.GetUnderlyingType(candidateType) == typeof(float));

		protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Expression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.InParameterSimpleBuilder(this.ParameterName, propertyType, expSprocParameters, expIgnoreParameters, expProperty, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgRealInParameter), null, null, parameterNames, expLogger, logger);

		protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.OutParameterBuilder(this.ParameterName, expSprocParameters, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgRealOutParameter), null, null, parameterNames, expIgnoreParameters, logger);

		protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReadOutParameterSimpleValueExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetFloat), nameof(DbParameterCollectionExtensions.GetNullableFloat), expProperty, expressions, expPrms, expPrm, propertyType, expLogger, logger);

		protected override void AppendReaderExpressions(Expression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReaderNullableValueTypeExpressions(this.ParameterName, expProperty, Expression.Constant(float.NaN, typeof(float)), columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);
	}
    #endregion
    #region Temporal parameters
    /// <summary>
    /// This attribute maps a model property to/from a PostgreSQL Timestamp parameter or column.
    /// </summary>
    public class MapToPgTimestampAttribute : PgParameterMapAttribute
	{
		/// <summary>
		/// Map this property to the specified Timestamp database column (without Timezone).
		/// </summary>
		/// <param name="parameterName">The name of the parameter or column that contains the value. The system will automatically add or remove the prefix '@' as needed.</param>
		public MapToPgTimestampAttribute(string parameterName) : base(parameterName, NpgsqlDbType.Timestamp)
		{
			//
		}
		public MapToPgTimestampAttribute(string parameterName, bool isRequired) : base(parameterName, NpgsqlDbType.Timestamp, isRequired)
		{
			//
		}

		public override bool IsValidType(Type candidateType)
			=> candidateType == typeof(DateTime) 
				|| candidateType == typeof(DateTimeOffset) 
				|| (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == typeof(Nullable<>) && Nullable.GetUnderlyingType(candidateType) == typeof(DateTime))
				|| (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == typeof(Nullable<>) && Nullable.GetUnderlyingType(candidateType) == typeof(DateTimeOffset));

		protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Expression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.InParameterSimpleBuilder(this.ParameterName, propertyType, expSprocParameters, expIgnoreParameters, expProperty, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgTimestampInParameter), null, null, parameterNames, expLogger, logger);

		protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.OutParameterBuilder(this.ParameterName, expSprocParameters, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgTimestampOutParameter), null, null, parameterNames, expIgnoreParameters, logger);

		protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReadOutParameterSimpleValueExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetDateTime), nameof(DbParameterCollectionExtensions.GetNullableDateTime), expProperty, expressions, expPrms, expPrm, propertyType, expLogger, logger);

		protected override void AppendReaderExpressions(Expression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReaderSimpleValueExpressions(this.ParameterName, expProperty, columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);
	}

    /// <summary>
    /// This attribute maps a model property to/from a PostgreSQL TimestampTz parameter or column.
    /// </summary>
	public class MapToPgTimestampTzAttribute : PgParameterMapAttribute
	{
		/// <summary>
		/// Map this property to the specified Timestamp database column (with Timezone).
		/// </summary>
		/// <param name="parameterName">The name of the parameter or column that contains the value. The system will automatically add or remove the prefix '@' as needed.</param>
		public MapToPgTimestampTzAttribute(string parameterName) : base(parameterName, NpgsqlDbType.TimestampTz)
		{
			//
		}
		public MapToPgTimestampTzAttribute(string parameterName, bool isRequired) : base(parameterName, NpgsqlDbType.TimestampTz, isRequired)
		{
			//
		}

		public override bool IsValidType(Type candidateType)
			=> candidateType == typeof(DateTimeOffset)
				|| (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == typeof(Nullable<>) && Nullable.GetUnderlyingType(candidateType) == typeof(DateTimeOffset));

		protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Expression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.InParameterSimpleBuilder(this.ParameterName, propertyType, expSprocParameters, expIgnoreParameters, expProperty, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgTimestampTzInParameter), null, null, parameterNames, expLogger, logger);

		protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.OutParameterBuilder(this.ParameterName, expSprocParameters, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgTimestampTzOutParameter), null, null, parameterNames, expIgnoreParameters, logger);

		protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReadOutParameterSimpleValueExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetDateTimeOffset), nameof(DbParameterCollectionExtensions.GetNullableDateTimeOffset), expProperty, expressions, expPrms, expPrm, propertyType, expLogger, logger);

		protected override void AppendReaderExpressions(Expression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReaderSimpleValueExpressions(this.ParameterName, expProperty, columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);
	}

    /// <summary>
    /// This attribute maps a model property to/from a PostgreSQL Date parameter or column.
    /// </summary>
    public class MapToPgDateAttribute : PgParameterMapAttribute
	{
		/// <summary>
		/// Map this property to the specified Date database column.
		/// </summary>
		/// <param name="parameterName">The name of the parameter or column that contains the value. The system will automatically add or remove the prefix '@' as needed.</param>
		public MapToPgDateAttribute(string parameterName) : base(parameterName, NpgsqlDbType.Date)
		{
			//
		}
		public MapToPgDateAttribute(string parameterName, bool isRequired) : base(parameterName, NpgsqlDbType.Date, isRequired)
		{
			//
		}

		public override bool IsValidType(Type candidateType)
			=> candidateType == typeof(DateTime) 
				|| (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == typeof(Nullable<>) && Nullable.GetUnderlyingType(candidateType) == typeof(DateTime));

		protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Expression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.InParameterSimpleBuilder(this.ParameterName, propertyType, expSprocParameters, expIgnoreParameters, expProperty, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgDateInParameter), null, null, parameterNames, expLogger, logger);

		protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.OutParameterBuilder(this.ParameterName, expSprocParameters, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgDateOutParameter), null, null, parameterNames, expIgnoreParameters, logger);

		protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReadOutParameterSimpleValueExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetDateTime), nameof(DbParameterCollectionExtensions.GetNullableDateTime), expProperty, expressions, expPrms, expPrm, propertyType, expLogger, logger);

		protected override void AppendReaderExpressions(Expression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReaderSimpleValueExpressions(this.ParameterName, expProperty, columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);
	}

    /// <summary>
    /// This attribute maps a model property to/from a PostgreSQL Time parameter or column.
    /// </summary>
    public class MapToPgTimeAttribute : PgParameterMapAttribute
	{
		/// <summary>
		/// Map this property to the specified Time database column (without Timezone).
		/// </summary>
		/// <param name="parameterName">The name of the parameter or column that contains the value. The system will automatically add or remove the prefix '@' as needed.</param>
		public MapToPgTimeAttribute(string parameterName) : base(parameterName, NpgsqlDbType.Time)
		{
			//
		}
		public MapToPgTimeAttribute(string parameterName, bool isRequired) : base(parameterName, NpgsqlDbType.Time, isRequired)
		{
			//
		}

		public override bool IsValidType(Type candidateType)
			=> candidateType == typeof(TimeSpan) || (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == typeof(Nullable<>) && Nullable.GetUnderlyingType(candidateType) == typeof(TimeSpan));

		protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Expression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.InParameterSimpleBuilder(this.ParameterName, propertyType, expSprocParameters, expIgnoreParameters, expProperty, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgTimeInParameter), null, null, parameterNames, expLogger, logger);

		protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.OutParameterBuilder(this.ParameterName, expSprocParameters, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgTimeOutParameter), null, null, parameterNames, expIgnoreParameters, logger);

		protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReadOutParameterSimpleValueExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetTimeSpan), nameof(DbParameterCollectionExtensions.GetNullableTimeSpan), expProperty, expressions, expPrms, expPrm, propertyType, expLogger, logger);

		protected override void AppendReaderExpressions(Expression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReaderSimpleValueExpressions(this.ParameterName, expProperty, columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);
	}

    /// <summary>
    /// This attribute maps a model property to/from a PostgreSQL Interval parameter or column.
    /// </summary>
    public class MapToPgIntervalAttribute : PgParameterMapAttribute
	{
		/// <summary>
		/// Map this property to the specified Time database column (without Timezone).
		/// </summary>
		/// <param name="parameterName">The name of the parameter or column that contains the value. The system will automatically add or remove the prefix '@' as needed.</param>
		public MapToPgIntervalAttribute(string parameterName) : base(parameterName, NpgsqlDbType.Interval)
		{
			//
		}
		public MapToPgIntervalAttribute(string parameterName, bool isRequired) : base(parameterName, NpgsqlDbType.Interval, isRequired)
		{
			//
		}

		public override bool IsValidType(Type candidateType)
			=> candidateType == typeof(TimeSpan) 
				|| (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == typeof(Nullable<>) && Nullable.GetUnderlyingType(candidateType) == typeof(TimeSpan));

		protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Expression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.InParameterSimpleBuilder(this.ParameterName, propertyType, expSprocParameters, expIgnoreParameters, expProperty, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgTimeInParameter), null, null, parameterNames, expLogger, logger);

		protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.OutParameterBuilder(this.ParameterName, expSprocParameters, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgTimeOutParameter), null, null, parameterNames, expIgnoreParameters, logger);

		protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReadOutParameterSimpleValueExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetTimeSpan), nameof(DbParameterCollectionExtensions.GetNullableTimeSpan), expProperty, expressions, expPrms, expPrm, propertyType, expLogger, logger);

		protected override void AppendReaderExpressions(Expression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReaderSimpleValueExpressions(this.ParameterName, expProperty, columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);
	}

    /// <summary>
    /// This attribute maps a model property to/from a PostgreSQL TimeTz parameter or column.
    /// </summary>
    public class MapToPgTimeTzAttribute : PgParameterMapAttribute
	{
		/// <summary>
		/// Map this property to the specified Time database column (without Timezone).
		/// </summary>
		/// <param name="parameterName">The name of the parameter or column that contains the value. The system will automatically add or remove the prefix '@' as needed.</param>
		public MapToPgTimeTzAttribute(string parameterName) : base(parameterName, NpgsqlDbType.TimeTz)
		{
			//
		}
		public MapToPgTimeTzAttribute(string parameterName, bool isRequired) : base(parameterName, NpgsqlDbType.TimeTz, isRequired)
		{
			//
		}

		public override bool IsValidType(Type candidateType)
			=> candidateType == typeof(TimeSpan) 
			|| candidateType == typeof(DateTimeOffset)
			|| (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == typeof(Nullable<>) && Nullable.GetUnderlyingType(candidateType) == typeof(TimeSpan))
			|| (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == typeof(Nullable<>) && Nullable.GetUnderlyingType(candidateType) == typeof(DateTimeOffset));

		protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Expression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.InParameterSimpleBuilder(this.ParameterName, propertyType, expSprocParameters, expIgnoreParameters, expProperty, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgTimeTzInParameter), null, null, parameterNames, expLogger, logger);

		protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.OutParameterBuilder(this.ParameterName, expSprocParameters, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgTimeTzOutParameter), null, null, parameterNames, expIgnoreParameters, logger);

		protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, Type propertyType, ParameterExpression expLogger, ILogger logger)
		{
			if (propertyType == typeof(TimeSpan))
			{
				ExpressionHelpers.ReadOutParameterSimpleValueExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetTimeSpan), nameof(DbParameterCollectionExtensions.GetNullableTimeSpan), expProperty, expressions, expPrms, expPrm, propertyType, expLogger, logger);
			}
			else
			{
				ExpressionHelpers.ReadOutParameterSimpleValueExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetDateTimeOffset), nameof(DbParameterCollectionExtensions.GetNullableDateTimeOffset), expProperty, expressions, expPrms, expPrm, propertyType, expLogger, logger);
			}
		}

		protected override void AppendReaderExpressions(Expression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReaderSimpleValueExpressions(this.ParameterName, expProperty, columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);
	}
    #endregion
    #region Other parameters

    /// <summary>
    /// This attribute maps a model property to/from a PostgreSQL Array parameter or column.
    /// </summary>
    public class MapToPgArrayAttribute : PgParameterMapAttribute
	{
        public MapToPgArrayAttribute(string parameterName) : base(parameterName, NpgsqlDbType.Array)
		{

		}
		public MapToPgArrayAttribute(string parameterName, bool isRequired) : base(parameterName, NpgsqlDbType.Array, isRequired)
		{

		}

		public override bool IsValidType(Type candidateType)
			=> candidateType.IsArray;

		protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Expression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.InParameterSimpleBuilder(this.ParameterName, propertyType, expSprocParameters, expIgnoreParameters, expProperty, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgArrayInParameter), null, null, parameterNames, expLogger, logger);

		protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.OutParameterBuilder(this.ParameterName, expSprocParameters, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgArrayOutParameter), null, null, parameterNames, expIgnoreParameters, logger);

		protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReadOutParameterBinaryExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetBytes), expProperty, expressions, expPrms, expPrm, propertyType, expLogger, logger);

		protected override void AppendReaderExpressions(Expression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReaderSimpleValueExpressions(this.ParameterName, expProperty, columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);
	}

    /// <summary>
    /// This attribute maps a model property to/from a PostgreSQL Byteea parameter or column.
    /// </summary>
    public class MapToPgByteaAttribute : PgParameterMapAttribute
	{
		public MapToPgByteaAttribute(string parameterName, int length) : base(parameterName, NpgsqlDbType.Bytea)
		{
			this.Length = length;
		}
		public MapToPgByteaAttribute(string parameterName, int length, bool isRequired) : base(parameterName, NpgsqlDbType.Bytea, isRequired)
		{
			this.Length = length;
		}
		public int Length { get; private set; }

		public override bool IsValidType(Type candidateType)
			=> candidateType == typeof(byte[]);

		protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Expression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.InParameterSimpleBuilder(this.ParameterName, propertyType, expSprocParameters, expIgnoreParameters, expProperty, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgByteaInParameter), Expression.Constant(this.Length, typeof(int)), null, parameterNames, expLogger, logger);

		protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.OutParameterBuilder(this.ParameterName, expSprocParameters, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgByteaOutParameter), Expression.Constant(this.Length, typeof(int)), null, parameterNames, expIgnoreParameters, logger);

		protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReadOutParameterBinaryExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetBytes), expProperty, expressions, expPrms, expPrm, propertyType, expLogger, logger);

		protected override void AppendReaderExpressions(Expression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReaderSimpleValueExpressions(this.ParameterName, expProperty, columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);
	}

    /// <summary>
    /// This attribute maps a model property to/from a PostgreSQL Hstore parameter or column.
    /// </summary>
    public class MapToPgHstoreAttribute : PgParameterMapAttribute
	{
		public MapToPgHstoreAttribute(string parameterName, int length) : base(parameterName, NpgsqlDbType.Hstore)
		{
			this.Length = length;
		}
		public MapToPgHstoreAttribute(string parameterName, int length, bool isRequired) : base(parameterName, NpgsqlDbType.Hstore, isRequired)
		{
			this.Length = length;
		}
		public int Length { get; private set; }

		public override bool IsValidType(Type candidateType)
			=> candidateType == typeof(IDictionary<string, string>);

		protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Expression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.InParameterSimpleBuilder(this.ParameterName, propertyType, expSprocParameters, expIgnoreParameters, expProperty, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgHstoreInParameter), Expression.Constant(this.Length, typeof(int)), null, parameterNames, expLogger, logger);

		protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.OutParameterBuilder(this.ParameterName, expSprocParameters, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgHstoreOutParameter), Expression.Constant(this.Length, typeof(int)), null, parameterNames, expIgnoreParameters, logger);

		protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReadOutParameterBinaryExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetBytes), expProperty, expressions, expPrms, expPrm, propertyType, expLogger, logger);

		protected override void AppendReaderExpressions(Expression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReaderSimpleValueExpressions(this.ParameterName, expProperty, columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);
	}

    /// <summary>
    /// This attribute maps a model property to/from a PostgreSQL Uuid parameter or column.
    /// </summary>
	public class MapToPgUuidAttribute : PgParameterMapAttribute
	{
		/// <summary>
		/// Map this property to the specified UniqueIdentifier (Guid) database column.
		/// </summary>
		/// <param name="parameterName">The name of the parameter or column that contains the value. The system will automatically add or remove the prefix '@' as needed.</param>
		public MapToPgUuidAttribute(string parameterName) : base(parameterName, NpgsqlDbType.Uuid)
		{
			//
		}
		public MapToPgUuidAttribute(string parameterName, bool isRequired) : base(parameterName, NpgsqlDbType.Uuid, isRequired)
		{
			//
		}

		public override bool IsValidType(Type candidateType)
			=> candidateType == typeof(Guid) || (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == typeof(Nullable<>) && Nullable.GetUnderlyingType(candidateType) == typeof(Guid));

		protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Expression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.InParameterSimpleBuilder(this.ParameterName, propertyType, expSprocParameters, expIgnoreParameters, expProperty, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgUuidInParameter), null, null, parameterNames, expLogger, logger);

		protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.OutParameterBuilder(this.ParameterName, expSprocParameters, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgUuidOutParameter), null, null, parameterNames, expIgnoreParameters, logger);

		protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReadOutParameterSimpleValueExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(DbParameterCollectionExtensions.GetGuid), nameof(DbParameterCollectionExtensions.GetNullableGuid), expProperty, expressions, expPrms, expPrm, propertyType, expLogger, logger);

		protected override void AppendReaderExpressions(Expression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReaderNullableValueTypeExpressions(this.ParameterName, expProperty, Expression.Constant(Guid.Empty, typeof(Guid)), columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);
	}
	//public class MapToPgEnumAttribute : PgParameterMapAttribute
	//{
	//	/// <summary>
	//	/// Map this property to the specified Int (32-bit) database column.
	//	/// </summary>
	//	/// <param name="parameterName">The name of the parameter or column that contains the value. The system will automatically add or remove the prefix '@' as needed.</param>
	//	public MapToPgEnumAttribute(string parameterName) : base(parameterName, NpgsqlDbType.Enum)
	//	{
	//		//
	//	}
	//	public MapToPgEnumAttribute(string parameterName, bool isRequired) : base(parameterName, NpgsqlDbType.Enum, isRequired)
	//	{
	//		//
	//	}

	//	public override bool IsValidType(Type candidateType)
	//		=> candidateType.IsEnum;

	//	protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Expression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
	//		=> ExpressionHelpers.InParameterSimpleBuilder(this.ParameterName, propertyType, expSprocParameters, expIgnoreParameters, expProperty, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgEnumInParameter), null, null, parameterNames, expLogger, logger);
	//		//=> ExpressionHelpers.InParameterEnumXIntExpressionBuilder(this.ParameterName, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgEnumInParameter), typeof(int?), expressions, expSprocParameters, expIgnoreParameters, parameterNames, expProperty, propertyType, expLogger, logger);

	//	protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, ParameterExpression expLogger, ILogger logger)
	//		=> ExpressionHelpers.OutParameterBuilder(this.ParameterName, expSprocParameters, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgEnumOutParameter), null, null, parameterNames, expIgnoreParameters, logger);

	//	protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, Type propertyType, ParameterExpression expLogger, ILogger logger)
	//		=> ExpressionHelpers.ReadOutParameterBinaryExpressions(this.ParameterName, typeof(Enum), nameof(PgParameterCollectionExtensions.GetEnum), expProperty, expressions, expPrms, expPrm, propertyType, expLogger, logger);
	//		//=> ExpressionHelpers.ReadOutParameterEnumXIntExpressions(this.ParameterName, typeof(DbParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.GetEnum), nameof(DbParameterCollectionExtensions.GetNullableInteger), expProperty, expressions, expPrms, expPrm, propertyType, expLogger, logger);

	//	protected override void AppendReaderExpressions(Expression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, Type propertyType, ParameterExpression expLogger, ILogger logger)
	//		=> ExpressionHelpers.ReaderSimpleValueExpressions(this.ParameterName, expProperty, columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);
	//}
	#endregion

	/*
		// Summary:
		//     Corresponds to the PostgreSQL "array" type, a variable-length multidimensional
		//     array of another type. This value must be combined with another value from NpgsqlTypes.NpgsqlDbType
		//     via a bit OR (e.g. NpgsqlDbType.Array | NpgsqlDbType.Integer)
		//
		// Remarks:
		//     See http://www.postgresql.org/docs/current/static/arrays.html
		Array = int.MinValue,
		//
		// Summary:
		//     Corresponds to the PostgreSQL 8-byte "bigint" type.
		//
		// Remarks:
		//     See http://www.postgresql.org/docs/current/static/datatype-numeric.html
		Bigint = 1,
		//
		// Summary:
		//     Corresponds to the PostgreSQL "boolean" type.
		//
		// Remarks:
		//     See http://www.postgresql.org/docs/current/static/datatype-boolean.html
		Boolean = 2,
		//
		// Summary:
		//     Corresponds to the PostgreSQL geometric "box" type.
		//
		// Remarks:
		//     See http://www.postgresql.org/docs/current/static/datatype-geometric.html
		Box = 3,
		//
		// Summary:
		//     Corresponds to the PostgreSQL "bytea" type, holding a raw byte string.
		//
		// Remarks:
		//     See http://www.postgresql.org/docs/current/static/datatype-binary.html
		Bytea = 4,
		//
		// Summary:
		//     Corresponds to the PostgreSQL geometric "circle" type.
		//
		// Remarks:
		//     See http://www.postgresql.org/docs/current/static/datatype-geometric.html
		Circle = 5,
		//
		// Summary:
		//     Corresponds to the PostgreSQL "char(n)"type.
		//
		// Remarks:
		//     See http://www.postgresql.org/docs/current/static/datatype-character.html
		Char = 6,
		//
		// Summary:
		//     Corresponds to the PostgreSQL "date" type.
		//
		// Remarks:
		//     See http://www.postgresql.org/docs/current/static/datatype-datetime.html
		Date = 7,
		//
		// Summary:
		//     Corresponds to the PostgreSQL 8-byte floating-point "double" type.
		//
		// Remarks:
		//     See http://www.postgresql.org/docs/current/static/datatype-numeric.html
		Double = 8,
		//
		// Summary:
		//     Corresponds to the PostgreSQL 4-byte "integer" type.
		//
		// Remarks:
		//     See http://www.postgresql.org/docs/current/static/datatype-numeric.html
		Integer = 9,
		//
		// Summary:
		//     Corresponds to the PostgreSQL geometric "line" type.
		//
		// Remarks:
		//     See http://www.postgresql.org/docs/current/static/datatype-geometric.html
		Line = 10,
		//
		// Summary:
		//     Corresponds to the PostgreSQL geometric "lseg" type.
		//
		// Remarks:
		//     See http://www.postgresql.org/docs/current/static/datatype-geometric.html
		LSeg = 11,
		//
		// Summary:
		//     Corresponds to the PostgreSQL "money" type.
		//
		// Remarks:
		//     See http://www.postgresql.org/docs/current/static/datatype-money.html
		Money = 12,
		//
		// Summary:
		//     Corresponds to the PostgreSQL arbitrary-precision "numeric" type.
		//
		// Remarks:
		//     See http://www.postgresql.org/docs/current/static/datatype-numeric.html
		Numeric = 13,
		//
		// Summary:
		//     Corresponds to the PostgreSQL geometric "path" type.
		//
		// Remarks:
		//     See http://www.postgresql.org/docs/current/static/datatype-geometric.html
		Path = 14,
		//
		// Summary:
		//     Corresponds to the PostgreSQL geometric "point" type.
		//
		// Remarks:
		//     See http://www.postgresql.org/docs/current/static/datatype-geometric.html
		Point = 15,
		//
		// Summary:
		//     Corresponds to the PostgreSQL geometric "polygon" type.
		//
		// Remarks:
		//     See http://www.postgresql.org/docs/current/static/datatype-geometric.html
		Polygon = 16,
		//
		// Summary:
		//     Corresponds to the PostgreSQL floating-point "real" type.
		//
		// Remarks:
		//     See http://www.postgresql.org/docs/current/static/datatype-numeric.html
		Real = 17,
		//
		// Summary:
		//     Corresponds to the PostgreSQL 2-byte "smallint" type.
		//
		// Remarks:
		//     See http://www.postgresql.org/docs/current/static/datatype-numeric.html
		Smallint = 18,
		//
		// Summary:
		//     Corresponds to the PostgreSQL "text" type.
		//
		// Remarks:
		//     See http://www.postgresql.org/docs/current/static/datatype-character.html
		Text = 19,
		//
		// Summary:
		//     Corresponds to the PostgreSQL "time" type.
		//
		// Remarks:
		//     See http://www.postgresql.org/docs/current/static/datatype-datetime.html
		Time = 20,
		//
		// Summary:
		//     Corresponds to the PostgreSQL "timestamp" type.
		//
		// Remarks:
		//     See http://www.postgresql.org/docs/current/static/datatype-datetime.html
		Timestamp = 21,
		//
		// Summary:
		//     Corresponds to the PostgreSQL "varchar" type.
		//
		// Remarks:
		//     See http://www.postgresql.org/docs/current/static/datatype-character.html
		Varchar = 22,
		//
		// Summary:
		//     Corresponds to the PostgreSQL "refcursor" type.
		Refcursor = 23,
		//
		// Summary:
		//     Corresponds to the PostgreSQL "inet" type.
		//
		// Remarks:
		//     See http://www.postgresql.org/docs/current/static/datatype-net-types.html
		Inet = 24,
		//
		// Summary:
		//     Corresponds to the PostgreSQL "bit" type.
		//
		// Remarks:
		//     See http://www.postgresql.org/docs/current/static/datatype-bit.html
		Bit = 25,
		//
		// Summary:
		//     Corresponds to the PostgreSQL "timestamp with time zone" type.
		//
		// Remarks:
		//     See http://www.postgresql.org/docs/current/static/datatype-datetime.html
		TimestampTz = 26,
		//
		// Summary:
		//     Corresponds to the PostgreSQL "uuid" type.
		//
		// Remarks:
		//     See http://www.postgresql.org/docs/current/static/datatype-uuid.html
		Uuid = 27,
		//
		// Summary:
		//     Corresponds to the PostgreSQL "xml" type.
		//
		// Remarks:
		//     See http://www.postgresql.org/docs/current/static/datatype-xml.html
		Xml = 28,
		//
		// Summary:
		//     Corresponds to the PostgreSQL internal "oidvector" type.
		//
		// Remarks:
		//     See http://www.postgresql.org/docs/current/static/datatype-oid.html
		Oidvector = 29,
		//
		// Summary:
		//     Corresponds to the PostgreSQL "interval" type.
		//
		// Remarks:
		//     See http://www.postgresql.org/docs/current/static/datatype-datetime.html
		Interval = 30,
		//
		// Summary:
		//     Corresponds to the PostgreSQL "time with time zone" type.
		//
		// Remarks:
		//     See http://www.postgresql.org/docs/current/static/datatype-datetime.html
		TimeTz = 31,
		//
		// Summary:
		//     Corresponds to the PostgreSQL internal "name" type.
		//
		// Remarks:
		//     See http://www.postgresql.org/docs/current/static/datatype-character.html
		Name = 32,
		//
		// Summary:
		//     Corresponds to the obsolete PostgreSQL "abstime" type.
		//
		// Remarks:
		//     See http://www.postgresql.org/docs/current/static/datatype-datetime.html
		Abstime = 33,
		//
		// Summary:
		//     Corresponds to the PostgreSQL "macaddr" type, a field storing a 6-byte physical
		//     address.
		//
		// Remarks:
		//     See http://www.postgresql.org/docs/current/static/datatype-net-types.html
		MacAddr = 34,
		//
		// Summary:
		//     Corresponds to the PostgreSQL "json" type, a field storing JSON in text format.
		//
		// Remarks:
		//     See http://www.postgresql.org/docs/current/static/datatype-json.html
		Json = 35,
		//
		// Summary:
		//     Corresponds to the PostgreSQL "jsonb" type, a field storing JSON in an optimized
		//     binary format.
		//
		// Remarks:
		//     Supported since PostgreSQL 9.4. See http://www.postgresql.org/docs/current/static/datatype-json.html
		Jsonb = 36,
		//
		// Summary:
		//     Corresponds to the PostgreSQL "hstore" type, a dictionary of string key-value
		//     pairs.
		//
		// Remarks:
		//     See http://www.postgresql.org/docs/current/static/hstore.html
		Hstore = 37,
		//
		// Summary:
		//     Corresponds to the PostgreSQL "char" type.
		//
		// Remarks:
		//     This is an internal field and should normally not be used for regular applications.
		//     See http://www.postgresql.org/docs/current/static/datatype-text.html
		InternalChar = 38,
		//
		// Summary:
		//     Corresponds to the PostgreSQL "varbit" type, a field storing a variable-length
		//     string of bits.
		//
		// Remarks:
		//     See http://www.postgresql.org/docs/current/static/datatype-boolean.html
		Varbit = 39,
		//
		// Summary:
		//     A special value that can be used to send parameter values to the database without
		//     specifying their type, allowing the database to cast them to another value based
		//     on context. The value will be converted to a string and send as text.
		//
		// Remarks:
		//     This value shouldn't ordinarily be used, and makes sense only when sending a
		//     data type unsupported by Npgsql.
		Unknown = 40,
		//
		// Summary:
		//     Corresponds to the PostgreSQL "oid" type.
		//
		// Remarks:
		//     See http://www.postgresql.org/docs/current/static/datatype-oid.html
		Oid = 41,
		//
		// Summary:
		//     Corresponds to the PostgreSQL "xid" type, an internal transaction identifier.
		//
		// Remarks:
		//     See http://www.postgresql.org/docs/current/static/datatype-oid.html
		Xid = 42,
		//
		// Summary:
		//     Corresponds to the PostgreSQL "cid" type, an internal command identifier.
		//
		// Remarks:
		//     See http://www.postgresql.org/docs/current/static/datatype-oid.html
		Cid = 43,
		//
		// Summary:
		//     Corresponds to the PostgreSQL "cidr" type, a field storing an IPv4 or IPv6 network.
		//
		// Remarks:
		//     See http://www.postgresql.org/docs/current/static/datatype-net-types.html
		Cidr = 44,
		//
		// Summary:
		//     Corresponds to the PostgreSQL "tsvector" type.
		//
		// Remarks:
		//     See http://www.postgresql.org/docs/current/static/datatype-textsearch.html
		TsVector = 45,
		//
		// Summary:
		//     Corresponds to the PostgreSQL "tsquery" type.
		//
		// Remarks:
		//     See http://www.postgresql.org/docs/current/static/datatype-textsearch.html
		TsQuery = 46,
		//
		// Summary:
		//     Corresponds to the PostgreSQL "enum" type.
		//
		// Remarks:
		//     See http://www.postgresql.org/docs/current/static/datatype-enum.html
		Enum = 47,
		//
		// Summary:
		//     Corresponds to the PostgreSQL "composite" type.
		//
		// Remarks:
		//     See http://www.postgresql.org/docs/current/static/rowtypes.html
		Composite = 48,
		//
		// Summary:
		//     Corresponds to the PostgreSQL "regtype" type, a numeric (OID) ID of a type in
		//     the pg_type table.
		Regtype = 49,
		//
		// Summary:
		//     The geometry type for postgresql spatial extension postgis.
		Geometry = 50,
		//
		// Summary:
		//     Corresponds to the PostgreSQL "citext" type for the citext module.
		//
		// Remarks:
		//     See http://www.postgresql.org/docs/current/static/citext.html
		Citext = 51,
		//
		// Summary:
		//     Corresponds to the PostgreSQL internal "int2vector" type.
		Int2Vector = 52,
		//
		// Summary:
		//     Corresponds to the PostgreSQL "tid" type, a tuple id identifying the physical
		//     location of a row within its table.
		Tid = 53,
		//
		// Summary:
		//     Corresponds to the PostgreSQL "macaddr8" type, a field storing a 6-byte or 8-byte
		//     physical address.
		//
		// Remarks:
		//     See http://www.postgresql.org/docs/current/static/datatype-net-types.html
		MacAddr8 = 54,
		//
		// Summary:
		//     Corresponds to the PostgreSQL "array" type, a variable-length multidimensional
		//     array of another type. This value must be combined with another value from NpgsqlTypes.NpgsqlDbType
		//     via a bit OR (e.g. NpgsqlDbType.Array | NpgsqlDbType.Integer)
		//
		// Remarks:
		//     Supported since PostgreSQL 9.2. See http://www.postgresql.org/docs/9.2/static/rangetypes.html
		Range = 1073741824
*/



}
