// © John Hicks. All rights reserved. Licensed under the MIT license.
// See the LICENSE file in the repository root for more information.

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

        internal abstract string ColumnDefinition { get; }

        public override string SqlTypeName => ((NpgsqlDbType)base.SqlType).ToString();
    }

    #region String parameters
    /// <summary>
    /// This attribute maps a model property to/from a PostgreSQL Varchar parameter or column.
    /// </summary>
    public class MapToPgVarcharAttribute : PgParameterMapAttribute
	{
		/// <summary>
		/// Map this property to the specified Unicode database column, with a variable but maximum length.
		/// </summary>
		/// <param name="parameterName">The name of the parameter or column that contains the value. The system will automatically add or remove the prefix ':' as needed.</param>
		/// <param name="length">The maximum length of the string.</param>
		public MapToPgVarcharAttribute(string parameterName, int length) : base(parameterName, NpgsqlDbType.Varchar)
		{
			this.Length = length;
		}
		public MapToPgVarcharAttribute(string parameterName, int length, bool isRequired) : base(parameterName, NpgsqlDbType.Varchar, isRequired)
		{
			this.Length = length;
		}
		public int Length { get; private set; }

		public override bool IsValidType(Type candidateType)
			=> candidateType.IsEnum || candidateType == typeof(string) || (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == typeof(Nullable<>) && Nullable.GetUnderlyingType(candidateType).IsEnum);

		protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Expression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.InParameterStringExpressionBuilder(this.ParameterName, this.Length, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgVarcharInputParameter), null, expressions, expSprocParameters, expIgnoreParameters, parameterNames, expProperty, propertyType, expLogger, logger);

		protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.OutParameterBuilder(this.ParameterName, expSprocParameters, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgVarcharOutputParameter), Expression.Constant(this.Length, typeof(int)), null, parameterNames, expIgnoreParameters, logger);

		protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReadOutParameterStringExpressions(this.ParameterName, typeof(DbParameterExtensions), nameof(DbParameterExtensions.GetString), expProperty, expressions, expPrms, expPrm, propertyType, expLogger, logger);

		protected override void AppendReaderExpressions(Expression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReaderStringExpressions(this.ParameterName, expProperty, columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);

        public override string ParameterName { get => PgParameterCollectionExtensions.NormalizePgParameterName(base.Name); }

        public override string ColumnName { get => PgParameterCollectionExtensions.NormalizePgColumnName(base.Name); }

        internal override string ColumnDefinition => $"\"{this.ColumnName}\" varchar({this.Length.ToString()})";
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
			=> ExpressionHelpers.InParameterStringExpressionBuilder(this.ParameterName, this.Length, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgCharInputParameter), null, expressions, expSprocParameters, expIgnoreParameters, parameterNames, expProperty, propertyType, expLogger, logger);


		protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.OutParameterBuilder(this.ParameterName, expSprocParameters, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgCharOutputParameter), Expression.Constant(this.Length, typeof(int)), null, parameterNames, expIgnoreParameters, logger);

		protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReadOutParameterStringExpressions(this.ParameterName, typeof(DbParameterExtensions), nameof(DbParameterExtensions.GetString), expProperty, expressions, expPrms, expPrm, propertyType, expLogger, logger);

		protected override void AppendReaderExpressions(Expression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReaderStringExpressions(this.ParameterName, expProperty, columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);

        public override string ParameterName { get => PgParameterCollectionExtensions.NormalizePgParameterName(base.Name); }

        public override string ColumnName { get => PgParameterCollectionExtensions.NormalizePgColumnName(base.Name); }

        internal override string ColumnDefinition => $"\"{this.ColumnName}\" char({this.Length.ToString()})";
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
			=> ExpressionHelpers.InParameterSimpleBuilder(this.ParameterName, propertyType, expSprocParameters, expIgnoreParameters, expProperty, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgTextInputParameter), null, null, parameterNames, expLogger, logger);
			//=> ExpressionHelpers.InParameterStringExpressionBuilder(this.ParameterName, this.Length, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgTextInParameter), null, expressions, expSprocParameters, expIgnoreParameters, parameterNames, expProperty, propertyType, expLogger, logger);

		protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.OutParameterBuilder(this.ParameterName, expSprocParameters, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgTextOutputParameter), null, null, parameterNames, expIgnoreParameters, logger);

		protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReadOutParameterStringExpressions(this.ParameterName, typeof(DbParameterExtensions), nameof(DbParameterExtensions.GetString), expProperty, expressions, expPrms, expPrm, propertyType, expLogger, logger);

		protected override void AppendReaderExpressions(Expression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmPgRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReaderStringExpressions(this.ParameterName, expProperty, columnLookupExpressions, expressions, prmPgRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);

        public override string ParameterName { get => PgParameterCollectionExtensions.NormalizePgParameterName(base.Name); }

        public override string ColumnName { get => PgParameterCollectionExtensions.NormalizePgColumnName(base.Name); }

        internal override string ColumnDefinition => $"\"{this.ColumnName}\" text";
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
			=> ExpressionHelpers.InParameterEnumXIntExpressionBuilder(this.ParameterName, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgBigintInputParameter), typeof(long?), expressions, expSprocParameters, expIgnoreParameters, parameterNames, expProperty, propertyType, expLogger, logger);

		protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.OutParameterBuilder(this.ParameterName, expSprocParameters, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgBigintOutputParameter), null, null, parameterNames, expIgnoreParameters, logger);

		protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReadOutParameterEnumXIntExpressions(this.ParameterName, typeof(DbParameterExtensions), nameof(DbParameterExtensions.GetLong), nameof(DbParameterExtensions.GetNullableLong), expProperty, expressions, expPrms, expPrm, propertyType, expLogger, logger);

		protected override void AppendReaderExpressions(Expression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReaderEnumXIntExpressions(this.ParameterName, expProperty, typeof(long), columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);

        public override string ParameterName { get => PgParameterCollectionExtensions.NormalizePgParameterName(base.Name); }

        public override string ColumnName { get => PgParameterCollectionExtensions.NormalizePgColumnName(base.Name); }

        internal override string ColumnDefinition => $"\"{this.ColumnName}\" bigint";
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
			=> ExpressionHelpers.InParameterEnumXIntExpressionBuilder(this.ParameterName, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgIntegerInputParameter), typeof(int?), expressions, expSprocParameters, expIgnoreParameters, parameterNames, expProperty, propertyType, expLogger, logger);

		protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.OutParameterBuilder(this.ParameterName, expSprocParameters, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgIntegerOutputParameter), null, null, parameterNames, expIgnoreParameters, logger);

		protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReadOutParameterEnumXIntExpressions(this.ParameterName, typeof(DbParameterExtensions), nameof(DbParameterExtensions.GetInteger), nameof(DbParameterExtensions.GetNullableInteger), expProperty, expressions, expPrms, expPrm, propertyType, expLogger, logger);

		protected override void AppendReaderExpressions(Expression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReaderEnumXIntExpressions(this.ParameterName, expProperty, typeof(int), columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);

        public override string ParameterName { get => PgParameterCollectionExtensions.NormalizePgParameterName(base.Name); }

        public override string ColumnName { get => PgParameterCollectionExtensions.NormalizePgColumnName(base.Name); }

        internal override string ColumnDefinition => $"\"{this.ColumnName}\" integer";
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
			=> ExpressionHelpers.InParameterEnumXIntExpressionBuilder(this.ParameterName, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgSmallintInputParameter), typeof(short?), expressions, expSprocParameters, expIgnoreParameters, parameterNames, expProperty, propertyType, expLogger, logger);

		protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.OutParameterBuilder(this.ParameterName, expSprocParameters, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgSmallintOutputParameter), null, null, parameterNames, expIgnoreParameters, logger);

		protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReadOutParameterEnumXIntExpressions(this.ParameterName, typeof(DbParameterExtensions), nameof(DbParameterExtensions.GetShort), nameof(DbParameterExtensions.GetNullableShort), expProperty, expressions, expPrms, expPrm, propertyType, expLogger, logger);

		protected override void AppendReaderExpressions(Expression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReaderEnumXIntExpressions(this.ParameterName, expProperty, typeof(short), columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);

        public override string ParameterName { get => PgParameterCollectionExtensions.NormalizePgParameterName(base.Name); }

        public override string ColumnName { get => PgParameterCollectionExtensions.NormalizePgColumnName(base.Name); }

        internal override string ColumnDefinition => $"\"{this.ColumnName}\" smallint";
    }

    /// <summary>
    /// This attribute maps a model property to/from a PostgreSQL (internal) "char" parameter or column. This data type is not intendeted for general use.
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
			=> ExpressionHelpers.InParameterEnumXIntExpressionBuilder(this.ParameterName, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgInternalCharInputParameter), typeof(byte?), expressions, expSprocParameters, expIgnoreParameters, parameterNames, expProperty, propertyType, expLogger, logger);

		protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.OutParameterBuilder(this.ParameterName, expSprocParameters, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgInternalCharOutputParameter), null, null, parameterNames, expIgnoreParameters, logger);

		protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReadOutParameterEnumXIntExpressions(this.ParameterName, typeof(DbParameterExtensions), nameof(DbParameterExtensions.GetByte), nameof(DbParameterExtensions.GetNullableByte), expProperty, expressions, expPrms, expPrm, propertyType, expLogger, logger);

		protected override void AppendReaderExpressions(Expression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReaderEnumXIntExpressions(this.ParameterName, expProperty, typeof(byte), columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);

        public override string ParameterName { get => PgParameterCollectionExtensions.NormalizePgParameterName(base.Name); }

        public override string ColumnName { get => PgParameterCollectionExtensions.NormalizePgColumnName(base.Name); }

        internal override string ColumnDefinition => $"\"{this.ColumnName}\" internal char";
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
			=> ExpressionHelpers.InParameterSimpleBuilder(this.ParameterName, propertyType, expSprocParameters, expIgnoreParameters, expProperty, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgBooleanInputParameter), null, null, parameterNames, expLogger, logger);

		protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.OutParameterBuilder(this.ParameterName, expSprocParameters, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgBooleanOutputParameter), null, null, parameterNames, expIgnoreParameters, logger);

		protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReadOutParameterSimpleValueExpressions(this.ParameterName, typeof(DbParameterExtensions), nameof(DbParameterExtensions.GetBoolean), nameof(DbParameterExtensions.GetNullableBoolean), expProperty, expressions, expPrms, expPrm, propertyType, expLogger, logger);

		protected override void AppendReaderExpressions(Expression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReaderSimpleValueExpressions(this.ParameterName, expProperty, columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);

        public override string ParameterName { get => PgParameterCollectionExtensions.NormalizePgParameterName(base.Name); }

        public override string ColumnName { get => PgParameterCollectionExtensions.NormalizePgColumnName(base.Name); }
        internal override string ColumnDefinition => $"\"{this.ColumnName}\" boolean";
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
			=> ExpressionHelpers.InParameterSimpleBuilder(this.ParameterName, propertyType, expSprocParameters, expIgnoreParameters, expProperty, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgNumericInputParameter), Expression.Constant(this.Precision, typeof(byte)), Expression.Constant(this.Scale, typeof(byte)), parameterNames, expLogger, logger);

		protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.OutParameterBuilder(this.ParameterName, expSprocParameters, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgNumericOutputParameter), Expression.Constant(this.Precision, typeof(byte)), Expression.Constant(this.Scale, typeof(byte)), parameterNames, expIgnoreParameters, logger);

		protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReadOutParameterSimpleValueExpressions(this.ParameterName, typeof(DbParameterExtensions), nameof(DbParameterExtensions.GetDecimal), nameof(DbParameterExtensions.GetNullableDecimal), expProperty, expressions, expPrms, expPrm, propertyType, expLogger, logger);

		protected override void AppendReaderExpressions(Expression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReaderSimpleValueExpressions(this.ParameterName, expProperty, columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);

        public override string ParameterName { get => PgParameterCollectionExtensions.NormalizePgParameterName(base.Name); }

        public override string ColumnName { get => PgParameterCollectionExtensions.NormalizePgColumnName(base.Name); }
        internal override string ColumnDefinition => $"\"{this.ColumnName}\" numeric({this.Precision.ToString()}, {this.Scale.ToString()})";
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
			=> ExpressionHelpers.InParameterSimpleBuilder(this.ParameterName, propertyType, expSprocParameters, expIgnoreParameters, expProperty, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgMoneyInputParameter), null, null, parameterNames, expLogger, logger);

		protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.OutParameterBuilder(this.ParameterName, expSprocParameters, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgMoneyOutputParameter), null, null, parameterNames, expIgnoreParameters, logger);

		protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReadOutParameterSimpleValueExpressions(this.ParameterName, typeof(DbParameterExtensions), nameof(DbParameterExtensions.GetDecimal), nameof(DbParameterExtensions.GetNullableDecimal), expProperty, expressions, expPrms, expPrm, propertyType, expLogger, logger);

		protected override void AppendReaderExpressions(Expression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReaderSimpleValueExpressions(this.ParameterName, expProperty, columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);

        public override string ParameterName { get => PgParameterCollectionExtensions.NormalizePgParameterName(base.Name); }

        public override string ColumnName { get => PgParameterCollectionExtensions.NormalizePgColumnName(base.Name); }
        internal override string ColumnDefinition => $"\"{this.ColumnName}\" money";
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
			=> ExpressionHelpers.InParameterSimpleBuilder(this.ParameterName, propertyType, expSprocParameters, expIgnoreParameters, expProperty, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgDoubleInputParameter), null, null, parameterNames, expLogger, logger);


		protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.OutParameterBuilder(this.ParameterName, expSprocParameters, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgDoubleOutputParameter), null, null, parameterNames, expIgnoreParameters, logger);

		protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReadOutParameterSimpleValueExpressions(this.ParameterName, typeof(DbParameterExtensions), nameof(DbParameterExtensions.GetDouble), nameof(DbParameterExtensions.GetNullableDouble), expProperty, expressions, expPrms, expPrm, propertyType, expLogger, logger);

		protected override void AppendReaderExpressions(Expression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReaderNullableValueTypeExpressions(this.ParameterName, expProperty, Expression.Constant(double.NaN, typeof(double)), columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);

        public override string ParameterName { get => PgParameterCollectionExtensions.NormalizePgParameterName(base.Name); }

        public override string ColumnName { get => PgParameterCollectionExtensions.NormalizePgColumnName(base.Name); }
        internal override string ColumnDefinition => $"\"{this.ColumnName}\" double precision";
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
			=> ExpressionHelpers.InParameterSimpleBuilder(this.ParameterName, propertyType, expSprocParameters, expIgnoreParameters, expProperty, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgRealInputParameter), null, null, parameterNames, expLogger, logger);

		protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.OutParameterBuilder(this.ParameterName, expSprocParameters, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgRealOutputParameter), null, null, parameterNames, expIgnoreParameters, logger);

		protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReadOutParameterSimpleValueExpressions(this.ParameterName, typeof(DbParameterExtensions), nameof(DbParameterExtensions.GetFloat), nameof(DbParameterExtensions.GetNullableFloat), expProperty, expressions, expPrms, expPrm, propertyType, expLogger, logger);

		protected override void AppendReaderExpressions(Expression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReaderNullableValueTypeExpressions(this.ParameterName, expProperty, Expression.Constant(float.NaN, typeof(float)), columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);

        public override string ParameterName { get => PgParameterCollectionExtensions.NormalizePgParameterName(base.Name); }

        public override string ColumnName { get => PgParameterCollectionExtensions.NormalizePgColumnName(base.Name); }

        internal override string ColumnDefinition => $"\"{this.ColumnName}\" real";
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
			=> ExpressionHelpers.InParameterSimpleBuilder(this.ParameterName, propertyType, expSprocParameters, expIgnoreParameters, expProperty, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgTimestampInputParameter), null, null, parameterNames, expLogger, logger);

		protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.OutParameterBuilder(this.ParameterName, expSprocParameters, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgTimestampOutputParameter), null, null, parameterNames, expIgnoreParameters, logger);

		protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReadOutParameterSimpleValueExpressions(this.ParameterName, typeof(DbParameterExtensions), nameof(DbParameterExtensions.GetDateTime), nameof(DbParameterExtensions.GetNullableDateTime), expProperty, expressions, expPrms, expPrm, propertyType, expLogger, logger);

		protected override void AppendReaderExpressions(Expression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReaderSimpleValueExpressions(this.ParameterName, expProperty, columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);

        public override string ParameterName { get => PgParameterCollectionExtensions.NormalizePgParameterName(base.Name); }

        public override string ColumnName { get => PgParameterCollectionExtensions.NormalizePgColumnName(base.Name); }

        internal override string ColumnDefinition => $"\"{this.ColumnName}\" timestamp";
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
			=> ExpressionHelpers.InParameterSimpleBuilder(this.ParameterName, propertyType, expSprocParameters, expIgnoreParameters, expProperty, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgTimestampTzInputParameter), null, null, parameterNames, expLogger, logger);

		protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.OutParameterBuilder(this.ParameterName, expSprocParameters, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgTimestampTzOutputParameter), null, null, parameterNames, expIgnoreParameters, logger);

		protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReadOutParameterSimpleValueExpressions(this.ParameterName, typeof(DbParameterExtensions), nameof(DbParameterExtensions.GetDateTimeOffset), nameof(DbParameterExtensions.GetNullableDateTimeOffset), expProperty, expressions, expPrms, expPrm, propertyType, expLogger, logger);

		protected override void AppendReaderExpressions(Expression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReaderSimpleValueExpressions(this.ParameterName, expProperty, columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);

        public override string ParameterName { get => PgParameterCollectionExtensions.NormalizePgParameterName(base.Name); }

        public override string ColumnName { get => PgParameterCollectionExtensions.NormalizePgColumnName(base.Name); }

        internal override string ColumnDefinition => $"\"{this.ColumnName}\" timestamp with time zone";
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
			=> ExpressionHelpers.InParameterSimpleBuilder(this.ParameterName, propertyType, expSprocParameters, expIgnoreParameters, expProperty, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgDateInputParameter), null, null, parameterNames, expLogger, logger);

		protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.OutParameterBuilder(this.ParameterName, expSprocParameters, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgDateOutputParameter), null, null, parameterNames, expIgnoreParameters, logger);

		protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReadOutParameterSimpleValueExpressions(this.ParameterName, typeof(DbParameterExtensions), nameof(DbParameterExtensions.GetDateTime), nameof(DbParameterExtensions.GetNullableDateTime), expProperty, expressions, expPrms, expPrm, propertyType, expLogger, logger);

		protected override void AppendReaderExpressions(Expression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReaderSimpleValueExpressions(this.ParameterName, expProperty, columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);

        public override string ParameterName { get => PgParameterCollectionExtensions.NormalizePgParameterName(base.Name); }

        public override string ColumnName { get => PgParameterCollectionExtensions.NormalizePgColumnName(base.Name); }

        internal override string ColumnDefinition => $"\"{this.ColumnName}\" date";
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
			=> ExpressionHelpers.InParameterSimpleBuilder(this.ParameterName, propertyType, expSprocParameters, expIgnoreParameters, expProperty, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgTimeInputParameter), null, null, parameterNames, expLogger, logger);

		protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.OutParameterBuilder(this.ParameterName, expSprocParameters, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgTimeOutputParameter), null, null, parameterNames, expIgnoreParameters, logger);

		protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReadOutParameterSimpleValueExpressions(this.ParameterName, typeof(DbParameterExtensions), nameof(DbParameterExtensions.GetTimeSpan), nameof(DbParameterExtensions.GetNullableTimeSpan), expProperty, expressions, expPrms, expPrm, propertyType, expLogger, logger);

		protected override void AppendReaderExpressions(Expression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReaderSimpleValueExpressions(this.ParameterName, expProperty, columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);

        public override string ParameterName { get => PgParameterCollectionExtensions.NormalizePgParameterName(base.Name); }

        public override string ColumnName { get => PgParameterCollectionExtensions.NormalizePgColumnName(base.Name); }

        internal override string ColumnDefinition => $"\"{this.ColumnName}\" time";
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
			=> ExpressionHelpers.InParameterSimpleBuilder(this.ParameterName, propertyType, expSprocParameters, expIgnoreParameters, expProperty, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgIntervalInputParameter), null, null, parameterNames, expLogger, logger);

		protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.OutParameterBuilder(this.ParameterName, expSprocParameters, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgIntervalOutputParameter), null, null, parameterNames, expIgnoreParameters, logger);

		protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReadOutParameterSimpleValueExpressions(this.ParameterName, typeof(DbParameterExtensions), nameof(DbParameterExtensions.GetTimeSpan), nameof(DbParameterExtensions.GetNullableTimeSpan), expProperty, expressions, expPrms, expPrm, propertyType, expLogger, logger);

		protected override void AppendReaderExpressions(Expression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReaderSimpleValueExpressions(this.ParameterName, expProperty, columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);

        public override string ParameterName { get => PgParameterCollectionExtensions.NormalizePgParameterName(base.Name); }

        public override string ColumnName { get => PgParameterCollectionExtensions.NormalizePgColumnName(base.Name); }

        internal override string ColumnDefinition => $"\"{this.ColumnName}\" interval";
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
			=> ExpressionHelpers.InParameterSimpleBuilder(this.ParameterName, propertyType, expSprocParameters, expIgnoreParameters, expProperty, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgTimeTzInputParameter), null, null, parameterNames, expLogger, logger);

		protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.OutParameterBuilder(this.ParameterName, expSprocParameters, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgTimeTzOutputParameter), null, null, parameterNames, expIgnoreParameters, logger);

		protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, Type propertyType, ParameterExpression expLogger, ILogger logger)
		{
			if (propertyType == typeof(TimeSpan))
			{
				ExpressionHelpers.ReadOutParameterSimpleValueExpressions(this.ParameterName, typeof(DbParameterExtensions), nameof(DbParameterExtensions.GetTimeSpan), nameof(DbParameterExtensions.GetNullableTimeSpan), expProperty, expressions, expPrms, expPrm, propertyType, expLogger, logger);
			}
			else
			{
				ExpressionHelpers.ReadOutParameterSimpleValueExpressions(this.ParameterName, typeof(DbParameterExtensions), nameof(DbParameterExtensions.GetDateTimeOffset), nameof(DbParameterExtensions.GetNullableDateTimeOffset), expProperty, expressions, expPrms, expPrm, propertyType, expLogger, logger);
			}
		}

		protected override void AppendReaderExpressions(Expression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReaderSimpleValueExpressions(this.ParameterName, expProperty, columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);

        public override string ParameterName { get => PgParameterCollectionExtensions.NormalizePgParameterName(base.Name); }

        public override string ColumnName { get => PgParameterCollectionExtensions.NormalizePgColumnName(base.Name); }
        internal override string ColumnDefinition => $"\"{this.ColumnName}\" time with time zone";

    }
    #endregion
    #region Other parameters

    /// <summary>
    /// This attribute maps a model property to/from a PostgreSQL Array parameter or column.
    /// </summary>
    public class MapToPgArrayAttribute : PgParameterMapAttribute
	{
        private NpgsqlDbType _arrayType;
        public MapToPgArrayAttribute(string parameterName, NpgsqlDbType arrayType) : base(parameterName, NpgsqlDbType.Array | arrayType)
		{
            _arrayType = arrayType;

        }
		public MapToPgArrayAttribute(string parameterName, NpgsqlDbType arrayType, bool isRequired) : base(parameterName, NpgsqlDbType.Array | arrayType, isRequired)
		{
            _arrayType = arrayType;
        }

        public override bool IsValidType(Type candidateType)
			=> candidateType.IsArray;

		protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Expression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.InParameterSimpleBuilder(this.ParameterName, propertyType, expSprocParameters, expIgnoreParameters, expProperty, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgArrayInputParameter), Expression.Constant(_arrayType, typeof(NpgsqlDbType)), null, parameterNames, expLogger, logger);

		protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.OutParameterBuilder(this.ParameterName, expSprocParameters, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgArrayOutputParameter), Expression.Constant(_arrayType, typeof(NpgsqlDbType)), null, parameterNames, expIgnoreParameters, logger);

		protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReadOutParameterArrayExpressions(this.ParameterName, typeof(PgParameterExtensions), nameof(PgParameterExtensions.GetArray), expProperty, expressions, expPrms, expPrm, propertyType, expLogger, logger);

		protected override void AppendReaderExpressions(Expression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReaderSimpleValueExpressions(this.ParameterName, expProperty, columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);

        public override string ParameterName { get => PgParameterCollectionExtensions.NormalizePgParameterName(base.Name); }

        public override string ColumnName { get => PgParameterCollectionExtensions.NormalizePgColumnName(base.Name); }

        internal override string ColumnDefinition => $"\"{this.ColumnName}\" { _arrayType.ToString().ToLowerInvariant() }[]";
        //
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
			=> ExpressionHelpers.InParameterSimpleBuilder(this.ParameterName, propertyType, expSprocParameters, expIgnoreParameters, expProperty, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgByteaInputParameter), Expression.Constant(this.Length, typeof(int)), null, parameterNames, expLogger, logger);

		protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.OutParameterBuilder(this.ParameterName, expSprocParameters, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgByteaOutputParameter), Expression.Constant(this.Length, typeof(int)), null, parameterNames, expIgnoreParameters, logger);

		protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReadOutParameterBinaryExpressions(this.ParameterName, typeof(DbParameterExtensions), nameof(DbParameterExtensions.GetBytes), expProperty, expressions, expPrms, expPrm, propertyType, expLogger, logger);

		protected override void AppendReaderExpressions(Expression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReaderSimpleValueExpressions(this.ParameterName, expProperty, columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);

        public override string ParameterName { get => PgParameterCollectionExtensions.NormalizePgParameterName(base.Name); }

        public override string ColumnName { get => PgParameterCollectionExtensions.NormalizePgColumnName(base.Name); }

        internal override string ColumnDefinition => $"\"{this.ColumnName}\" bytea";
    }

    /// <summary>
    /// This attribute maps a model property to/from a PostgreSQL Hstore parameter or column.
    /// </summary>
    public class MapToPgHstoreAttribute : PgParameterMapAttribute
	{
		public MapToPgHstoreAttribute(string parameterName) : base(parameterName, NpgsqlDbType.Hstore)
		{
            //
		}
		public MapToPgHstoreAttribute(string parameterName, bool isRequired) : base(parameterName, NpgsqlDbType.Hstore, isRequired)
		{
            //
		}

		public override bool IsValidType(Type candidateType)
			=> candidateType == typeof(IDictionary<string, string>);

		protected override void AppendInParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, Expression expProperty, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.InParameterSimpleBuilder(this.ParameterName, propertyType, expSprocParameters, expIgnoreParameters, expProperty, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgHstoreInputParameter), null, null, parameterNames, expLogger, logger);

		protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.OutParameterBuilder(this.ParameterName, expSprocParameters, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgHstoreOutputParameter), null, null, parameterNames, expIgnoreParameters, logger);

		protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReadOutParameterBinaryExpressions(this.ParameterName, typeof(PgParameterExtensions), nameof(PgParameterExtensions.GetHStore), expProperty, expressions, expPrms, expPrm, propertyType, expLogger, logger);

		protected override void AppendReaderExpressions(Expression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReaderEnumXIntExpressions(this.ParameterName, expProperty, typeof(Dictionary<string, string>), columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);

        public override string ParameterName { get => PgParameterCollectionExtensions.NormalizePgParameterName(base.Name); }

        public override string ColumnName { get => PgParameterCollectionExtensions.NormalizePgColumnName(base.Name); }

        internal override string ColumnDefinition => $"\"{this.ColumnName}\" hstore";
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
			=> ExpressionHelpers.InParameterSimpleBuilder(this.ParameterName, propertyType, expSprocParameters, expIgnoreParameters, expProperty, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgUuidInputParameter), null, null, parameterNames, expLogger, logger);

		protected override void AppendSetOutParameterExpressions(IList<Expression> expressions, ParameterExpression expSprocParameters, ParameterExpression expIgnoreParameters, HashSet<string> parameterNames, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.OutParameterBuilder(this.ParameterName, expSprocParameters, expressions, typeof(PgParameterCollectionExtensions), nameof(PgParameterCollectionExtensions.AddPgUuidOutputParameter), null, null, parameterNames, expIgnoreParameters, logger);

		protected override void AppendReadOutParameterExpressions(Expression expProperty, IList<Expression> expressions, ParameterExpression expPrms, ParameterExpression expPrm, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReadOutParameterSimpleValueExpressions(this.ParameterName, typeof(DbParameterExtensions), nameof(DbParameterExtensions.GetGuid), nameof(DbParameterExtensions.GetNullableGuid), expProperty, expressions, expPrms, expPrm, propertyType, expLogger, logger);

		protected override void AppendReaderExpressions(Expression expProperty, IList<MethodCallExpression> columnLookupExpressions, IList<Expression> expressions, ParameterExpression prmSqlRdr, ParameterExpression expOrdinals, ParameterExpression expOrdinal, ref int propIndex, Type propertyType, ParameterExpression expLogger, ILogger logger)
			=> ExpressionHelpers.ReaderNullableValueTypeExpressions(this.ParameterName, expProperty, Expression.Constant(Guid.Empty, typeof(Guid)), columnLookupExpressions, expressions, prmSqlRdr, expOrdinals, expOrdinal, ref propIndex, propertyType, expLogger, logger);

        public override string ParameterName { get => PgParameterCollectionExtensions.NormalizePgParameterName(base.Name); }

        public override string ColumnName { get => PgParameterCollectionExtensions.NormalizePgColumnName(base.Name); }
        internal override string ColumnDefinition => $"\"{this.ColumnName}\" uuid";

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
    //
    //  public override string ParameterName { get => PgParameterCollectionExtensions.NormalizeSqlParameterName(base.Name); }
    //
    //  public override string ColumnName { get => PgParameterCollectionExtensions.NormalizeSqlColumnName(base.Name); }
    //}
    #endregion
}
