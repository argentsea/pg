// © John Hicks. All rights reserved. Licensed under the MIT license.
// See the LICENSE file in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Globalization;
using Microsoft.Extensions.Logging;
using System.Linq;
using Npgsql;
using System.Threading;
using System.Text;
using System.Threading.Tasks;

namespace ArgentSea.Pg
{


    public static class CopyMapper
    {
        #region Extension methods
        public static ShardBatch<TShard, TResult> Add<TShard, TModel, TResult>(this ShardBatch<TShard, TResult> batch, List<TModel> models, string tableName) where TShard : IComparable where TModel : class, new()
        {
            batch.Add(new CopyModelStep<TShard, TModel, TResult>(models, tableName));
            return batch;
        }

        public static ShardBatch<TShard, TResult> Add<TShard, TRecord, TResult>(this ShardBatch<TShard, TResult> batch, List<ShardKey<TShard, TRecord>> keys, string tableName, PgParameterMapAttribute shardIdDefinition, PgParameterMapAttribute recordIdDefinition) where TShard : IComparable where TRecord : IComparable
        {
            batch.Add(new CopyKeysStep<TShard, TRecord, TResult>(keys, tableName, shardIdDefinition, recordIdDefinition));
            return batch;
        }

        public static ShardBatch<TShard, TResult> Add<TShard, TRecord, TChildId, TResult>(this ShardBatch<TShard, TResult> batch, List<ShardChild<TShard, TRecord, TChildId>> keys, string tableName, PgParameterMapAttribute shardIdDefinition, PgParameterMapAttribute recordIdDefinition, PgParameterMapAttribute childIdDefinition) where TShard : IComparable where TRecord : IComparable where TChildId : IComparable
        {
            batch.Add(new CopyChildrenStep<TShard, TRecord, TChildId, TResult>(keys, tableName, shardIdDefinition, recordIdDefinition, childIdDefinition));
            return batch;
        }

        #endregion
        #region Private classes
        private class CopyKeysStep<TShard, TRecord, TResult> : BatchStep<TShard, TResult> where TShard : IComparable where TRecord : IComparable
        {
            private readonly List<ShardKey<TShard, TRecord>> _keys;
            private readonly string _tableName;
            private readonly PgParameterMapAttribute _shardIdDefinition;
            private readonly PgParameterMapAttribute _recordIdDefinition;

            public CopyKeysStep(List<ShardKey<TShard, TRecord>> keys, string tableName, PgParameterMapAttribute shardIdDefinition, PgParameterMapAttribute recordIdDefinition)
            {
                _keys = keys;
                _tableName = tableName;
                _shardIdDefinition = shardIdDefinition;
                _recordIdDefinition = recordIdDefinition;
            }
            protected override async Task<TResult> Execute(TShard shardId, DbConnection connection, DbTransaction transaction, string connectionName, IDataProviderServiceFactory services, ILogger logger, CancellationToken cancellation)
            {
                if (!_tableName.All((c) => { return char.IsLetterOrDigit(c) || c == '.'; }))
                {
                    throw new Exception("The table name can only contain letters or numbers, and optionally a “.” schema separator.");
                }
                cancellation.ThrowIfCancellationRequested();

                var cmd = connection.CreateCommand();
                cmd.Transaction = transaction;

                string importDef;
                if (_tableName.Contains('.'))
                {
                    var parsedTableName = _tableName.Replace(".", "\".\"");
                    cmd.CommandText = $"CREATE TABLE IF NOT EXISTS \"{ parsedTableName }\" (\"{ _shardIdDefinition.ColumnDefinition }\", \"{ _recordIdDefinition.ColumnDefinition }\");";
                    importDef = $"COPY \"{ parsedTableName } (\"{ _shardIdDefinition.ColumnName }\", \"{ _recordIdDefinition.ColumnName }\") FROM STDIN (FORMAT BINARY)";
                }
                else
                {
                    cmd.CommandText = $"CREATE TEMP TABLE IF NOT EXISTS \"{ _tableName }\" ({ _shardIdDefinition.ColumnDefinition}, { _recordIdDefinition.ColumnDefinition});";
                    importDef = $"COPY { _tableName } (shardid, recordid) FROM STDIN (FORMAT BINARY)";
                }
                logger.CopySqlStatements(cmd.CommandText, importDef);

                cancellation.ThrowIfCancellationRequested();
                await cmd.ExecuteNonQueryAsync(cancellation);

                using (var importer = ((NpgsqlConnection)connection).BeginBinaryImport(importDef))
                {
                    foreach (var key in _keys)
                    {
                        importer.StartRow();
                        importer.Write<TShard>(key.ShardId);
                        importer.Write<TRecord>(key.RecordId);
                    }
                    importer.Complete();
                }
                return default(TResult);
            }
        }

        private class CopyChildrenStep<TShard, TRecord, TChildId, TResult> : BatchStep<TShard, TResult> where TShard : IComparable where TRecord : IComparable where TChildId : IComparable
        {
            private readonly List<ShardChild<TShard, TRecord, TChildId>> _keys;
            private readonly string _tableName;
            private readonly PgParameterMapAttribute _shardIdDefinition;
            private readonly PgParameterMapAttribute _recordIdDefinition;
            private readonly PgParameterMapAttribute _childIdDefinition;

            public CopyChildrenStep(List<ShardChild<TShard, TRecord, TChildId>> keys, string tableName, PgParameterMapAttribute shardIdDefinition, PgParameterMapAttribute recordIdDefinition, PgParameterMapAttribute childIdDefinition)
            {
                _keys = keys;
                _tableName = tableName;
                _shardIdDefinition = shardIdDefinition;
                _recordIdDefinition = recordIdDefinition;
                _childIdDefinition = childIdDefinition;
            }
            protected override async Task<TResult> Execute(TShard shardId, DbConnection connection, DbTransaction transaction, string connectionName, IDataProviderServiceFactory services, ILogger logger, CancellationToken cancellation)
            {
                //await CopyMapper.CopyChildren<TShard, TRecord, TChildId>(_keys, _shardIdDefinition, _recordIdDefinition, _childIdDefinition, (NpgsqlConnection)connection, _tableName, logger, cancellationToken);
                if (!_tableName.All((c) => { return char.IsLetterOrDigit(c) || c == '.'; }))
                {
                    throw new Exception("The table name can only contain letters or numbers, and optionally a “.” schema separator.");
                }
                cancellation.ThrowIfCancellationRequested();

                var cmd = connection.CreateCommand();
                cmd.Transaction = transaction;

                string importDef;
                if (_tableName.Contains('.'))
                {
                    var parsedTableName = _tableName.Replace(".", "\".\"");
                    cmd.CommandText = $"CREATE TABLE IF NOT EXISTS \"{ parsedTableName }\" (\"{ _shardIdDefinition.ColumnDefinition }\", \"{ _recordIdDefinition.ColumnDefinition }\", \"{ _childIdDefinition.ColumnDefinition }\");";
                    importDef = $"COPY \"{ parsedTableName } ({ _shardIdDefinition.ColumnName }, { _recordIdDefinition.ColumnName }) FROM STDIN (FORMAT BINARY)";
                }
                else
                {
                    cmd.CommandText = $"CREATE TEMP TABLE IF NOT EXISTS \"{ _tableName }\" (\"{ _shardIdDefinition.ColumnDefinition}\", \"{ _recordIdDefinition.ColumnDefinition}\", \"{ _childIdDefinition.ColumnDefinition}\");";
                    importDef = $"COPY { _tableName } (shardid, recordid) FROM STDIN (FORMAT BINARY)";
                }
                logger.CopySqlStatements(cmd.CommandText, importDef);

                cancellation.ThrowIfCancellationRequested();
                await cmd.ExecuteNonQueryAsync(cancellation);

                using (var importer = ((NpgsqlConnection)connection).BeginBinaryImport(importDef))
                {
                    foreach (var key in _keys)
                    {
                        importer.StartRow();
                        importer.Write<TShard>(key.ShardId);
                        importer.Write<TRecord>(key.RecordId);
                        importer.Write<TChildId>(key.ChildId);
                    }
                    importer.Complete();
                }
                return default(TResult);
            }
        }

        private class CopyModelStep<TShard, TModel, TResult> : BatchStep<TShard, TResult> where TShard : IComparable where TModel : class, new()
        {
            private readonly List<TModel> _models;
            private readonly string _tableName;
            private readonly PgParameterMapAttribute _shardIdDefinition;
            private readonly PgParameterMapAttribute _recordIdDefinition;
            public CopyModelStep(List<TModel> models, string tableName)
            {
                _models = models;
                _tableName = tableName;
            }
            protected override async Task<TResult> Execute(TShard shardId, DbConnection connection, DbTransaction transaction, string connectionName, IDataProviderServiceFactory services, ILogger logger, CancellationToken cancellation)
            {
                if (!_tableName.All((c) => { return char.IsLetterOrDigit(c) || c == '.'; }))
                {
                    throw new Exception("The table name can only contain letters or numbers, and optionally a “.” schema separator.");
                }
                var tModel = typeof(TModel);
                var lazyCopyDelegate = _setCopyParamCache.GetOrAdd(tModel, new Lazy<(string tableDef, string importDef, Delegate setRow)>(()
                   => BuildCopyDelegateAndQuery<TModel>(logger), LazyThreadSafetyMode.ExecutionAndPublication));
                if (lazyCopyDelegate.IsValueCreated)
                {
                    PgLoggingExtensions.PgCopyCacheHit(logger, tModel);
                }
                else
                {
                    PgLoggingExtensions.PgCopyCacheMiss(logger, tModel);
                }
                var meta = lazyCopyDelegate.Value;
                var setRow = (Action<TModel, NpgsqlBinaryImporter, ILogger>)meta.setRow;

                string tableDef;
                string importDef;
                if (_tableName.Contains('.'))
                {
                    var parsedTableName = _tableName.Replace(".", "\".\"");
                    tableDef = meta.tableDef.Replace("<<TABLEDEF>>", $"TABLE IF NOT EXISTS \"{ parsedTableName }\"");
                    importDef = meta.tableDef.Replace("<<TABLEDEF>>", $"\"{ parsedTableName }\"");
                }
                else
                {
                    tableDef = meta.tableDef.Replace("<<TABLEDEF>>", $"TEMP TABLE  \"{ _tableName }\"");
                    importDef = meta.tableDef.Replace("<<TABLEDEF>>", $"\"{ _tableName }\"");
                }

                logger.CopySqlStatements(tableDef, importDef);
                var cmd = connection.CreateCommand();
                cmd.CommandText = tableDef;
                cmd.Transaction = transaction;
                cancellation.ThrowIfCancellationRequested();
                await cmd.ExecuteNonQueryAsync(cancellation);
                using (var importer = ((NpgsqlConnection)connection).BeginBinaryImport(importDef))
                {
                    foreach (var row in _models)
                    {
                        setRow(row, importer, logger);
                    }
                    importer.Complete();
                }
                return default(TResult);
            }
        }

        #endregion

        #region Private helper methods

        private static ConcurrentDictionary<Type, Lazy<(string tableDef, string importDef, Delegate setRow)>> _setCopyParamCache = new ConcurrentDictionary<Type, Lazy<(string tableDef, string importDef, Delegate setRow)>>();

        private static (string, string, Action<TModel, NpgsqlBinaryImporter, ILogger>) BuildCopyDelegateAndQuery<TModel>(ILogger logger) where TModel : class
        {
            var tModel = typeof(TModel);
            var variables = new List<ParameterExpression>();
            var expressions = new List<Expression>();

            var prmModel = Expression.Parameter(tModel, "model");
            var expImporter = Expression.Parameter(typeof(ILogger), "importer");
            var expLogger = Expression.Parameter(typeof(ILogger), "logger");
            var exprInPrms = new ParameterExpression[] { prmModel, expImporter, expLogger };

            expressions.Add(Expression.Call(expImporter, typeof(NpgsqlBinaryImporter).GetMethod(nameof(NpgsqlBinaryImporter.StartRow))));

            var tableSB = new StringBuilder();
            var insertSB = new StringBuilder();
            tableSB.AppendLine("CREATE <<TABLEDEF>> (");
            insertSB.Append("COPY <<TABLEDEF>> (");

            IterateCopyProperties(tModel, tableSB, insertSB, expressions, expImporter, prmModel, expLogger, variables, logger);

            tableSB.Append(");");
            insertSB.Append(") FROM STDIN BINARY;");

            var expBlock = Expression.Block(variables, expressions);
            var lambda = Expression.Lambda<Action<TModel, NpgsqlBinaryImporter, ILogger>>(expBlock, exprInPrms);
            logger.CreatedExpressionTreeForCopy(tModel, expBlock);
            return (tableSB.ToString(), insertSB.ToString(), lambda.Compile());
        }

        private static void IterateCopyProperties(Type tModel, StringBuilder tableSB, StringBuilder insertSB, List<Expression> expressions, ParameterExpression expImporter, Expression prmModel, ParameterExpression expLogger, List<ParameterExpression> variables, ILogger logger)
        {
            var unorderedProps = tModel.GetProperties();
            var props = unorderedProps.OrderBy(prop => prop.MetadataToken);
            var miLogTrace = typeof(PgLoggingExtensions).GetMethod(nameof(PgLoggingExtensions.TraceCopyMapperProperty));
            var miWrite = typeof(NpgsqlBinaryImporter).GetMethod(nameof(NpgsqlBinaryImporter.Write));
            var miWriteNull = typeof(NpgsqlBinaryImporter).GetMethod(nameof(NpgsqlBinaryImporter.WriteNull));
            var isFirstColumn = true;
            foreach (var prop in props)
            {
                MemberExpression expOriginalProperty = Expression.Property(prmModel, prop);
                Expression expProperty = expOriginalProperty;
                var isShardKey = prop.IsDefined(typeof(MapShardKeyAttribute), true);
                var isShardChild = prop.IsDefined(typeof(MapShardChildAttribute), true);
                Type propType = prop.PropertyType;
                if ((isShardKey || isShardChild) && prop.IsDefined(typeof(PgParameterMapAttribute), true))
                {
                    var foundShardId = false;
                    var foundRecordId = false;
                    var foundChildId = false;
                    string shardIdPrm;
                    string recordIdPrm;
                    string childIdPrm;
                    expressions.Add(Expression.Call(miLogTrace, expLogger, Expression.Constant(prop.Name)));

                    Expression expDetectNullOrEmpty;
                    if (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        expProperty = Expression.Property(expProperty, propType.GetProperty(nameof(Nullable<int>.Value)));
                        propType = Nullable.GetUnderlyingType(propType);
                        expDetectNullOrEmpty = Expression.Property(expOriginalProperty, propType.GetProperty(nameof(Nullable<int>.HasValue)));
                    }
                    else
                    {
                        expDetectNullOrEmpty = Expression.NotEqual(expOriginalProperty, Expression.Property(null, propType.GetProperty(nameof(ShardKey<int, int>.Empty))));
                    }

                    if (isShardKey)
                    {
                        var shdData = prop.GetCustomAttribute<MapShardKeyAttribute>(true);
                        shardIdPrm = shdData.ShardIdName;
                        recordIdPrm = shdData.RecordIdName;
                        childIdPrm = null;
                    }
                    else
                    {
                        var shdData = prop.GetCustomAttribute<MapShardChildAttribute>(true);
                        shardIdPrm = shdData.ShardIdName;
                        recordIdPrm = shdData.RecordIdName;
                        childIdPrm = shdData.ChildIdName;
                    }
                    var attrPMs = prop.GetCustomAttributes<PgParameterMapAttribute>(true);
                    foreach (var attrPM in attrPMs)
                    {
                        if (!string.IsNullOrEmpty(shardIdPrm) && attrPM.Name == shardIdPrm)
                        {
                            foundShardId = true;
                            var tDataShardId = propType.GetGenericArguments()[0];
                            if (!attrPM.IsValidType(tDataShardId))
                            {
                                throw new InvalidMapTypeException(prop, attrPM.SqlType);
                            }
                            var expShardProperty = Expression.Property(expProperty, propType.GetProperty(nameof(ShardKey<int, int>.ShardId)));

                            expressions.Add(Expression.IfThenElse(
                                expDetectNullOrEmpty, //if
                                Expression.Call(miWriteNull), //then
                                Expression.Call(expImporter, miWrite.MakeGenericMethod(new Type[] { expShardProperty.Type }), expShardProperty, Expression.Constant(attrPM.SqlType)) //else
                                ));
                            BuildQueryColumnText(ref isFirstColumn, tableSB, insertSB, attrPM.ColumnDefinition, attrPM.ColumnName);
                        }
                        if (attrPM.Name == recordIdPrm)
                        {
                            foundRecordId = true;
                            var tDataRecordId = propType.GetGenericArguments()[1];
                            if (!attrPM.IsValidType(tDataRecordId))
                            {
                                throw new InvalidMapTypeException(prop, attrPM.SqlType);
                            }
                            var expRecordProperty = Expression.Property(expProperty, propType.GetProperty(nameof(ShardKey<int, int>.RecordId)));
                            expressions.Add(Expression.IfThenElse(
                                expDetectNullOrEmpty, //if
                                Expression.Call(miWriteNull), //then
                                Expression.Call(expImporter, miWrite.MakeGenericMethod(new Type[] { expRecordProperty.Type }), expRecordProperty, Expression.Constant(attrPM.SqlType)) //else
                                ));
                            BuildQueryColumnText(ref isFirstColumn, tableSB, insertSB, attrPM.ColumnDefinition, attrPM.ColumnName);
                        }
                        if (isShardChild && attrPM.Name == childIdPrm)
                        {
                            foundChildId = true;
                            var tDataChildId = propType.GetGenericArguments()[2];
                            if (!attrPM.IsValidType(tDataChildId))
                            {
                                throw new InvalidMapTypeException(prop, attrPM.SqlType);
                            }
                            var expChildProperty = Expression.Property(expProperty, propType.GetProperty(nameof(ShardChild<int, int, int>.ChildId)));
                            expressions.Add(Expression.IfThenElse(
                                expDetectNullOrEmpty, //if
                                Expression.Call(miWriteNull), //then
                                Expression.Call(expImporter, miWrite.MakeGenericMethod(new Type[] { expChildProperty.Type }), expChildProperty, Expression.Constant(attrPM.SqlType)) //else
                                ));
                            BuildQueryColumnText(ref isFirstColumn, tableSB, insertSB, attrPM.ColumnDefinition, attrPM.ColumnName);
                        }
                    }
                    if (!string.IsNullOrEmpty(shardIdPrm) && !foundShardId)
                    {
                        throw new MapAttributeMissingException(MapAttributeMissingException.ShardElement.ShardId, shardIdPrm);
                    }
                    if (!foundRecordId)
                    {
                        throw new MapAttributeMissingException(MapAttributeMissingException.ShardElement.RecordId, recordIdPrm);
                    }
                    if (isShardChild && !foundChildId)
                    {
                        throw new MapAttributeMissingException(MapAttributeMissingException.ShardElement.ChildId, childIdPrm);
                    }
                }
                else if (prop.IsDefined(typeof(PgParameterMapAttribute), true))
                {
                    bool alreadyFound = false;
                    var attrPMs = prop.GetCustomAttributes<PgParameterMapAttribute>(true);
                    foreach (var attrPM in attrPMs) // should only ever be one.
                    {
                        if (alreadyFound)
                        {
                            throw new MultipleMapAttributesException(prop);
                        }
                        alreadyFound = true;
                        var dataName = attrPM.ColumnName;
                        expressions.Add(Expression.Call(miLogTrace, expLogger, Expression.Constant(prop.Name)));
                        Expression expIfNull = null;
                        if (!attrPM.IsValidType(propType))
                        {
                            throw new InvalidMapTypeException(prop, attrPM.SqlType);
                        }
                        if (propType == typeof(double) || propType == typeof(float))
                        {
                            expIfNull = Expression.Call(propType.GetMethod(nameof(double.IsNaN)), expProperty );
                        }
                        else if (propType == typeof(Guid))
                        {
                            expIfNull = Expression.Equal(expProperty, Expression.Constant(Guid.Empty));
                        }
                        else if (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            expProperty = Expression.Property(expProperty, propType.GetProperty(nameof(Nullable<int>.Value)));
                            propType = Nullable.GetUnderlyingType(propType);
                            if (propType.IsEnum)
                            {
                                expProperty = Expression.Convert(expProperty, propType);
                            }
                            expIfNull = Expression.Property(expOriginalProperty, propType.GetProperty(nameof(Nullable<int>.HasValue)));
                        }
                        else if (propType.IsEnum)
                        {
                            if (attrPM.SqlType == (int)NpgsqlTypes.NpgsqlDbType.Text 
                                || attrPM.SqlType == (int)NpgsqlTypes.NpgsqlDbType.Char 
                                || attrPM.SqlType == (int)NpgsqlTypes.NpgsqlDbType.Varchar)
                            {
                                var miEnumToString = typeof(Enum).GetMethod(nameof(Enum.ToString), new Type[] { });
                                expProperty = Expression.Call(expProperty, miEnumToString);
                            }
                            else
                            {
                                expProperty = Expression.Convert(expProperty, propType);
                            }

                        }
                        else if (!propType.IsValueType)
                        {
                            expIfNull = Expression.Equal(expProperty, Expression.Constant(null, propType));
                        }
                        if (expIfNull is null)
                        {
                            expressions.Add(Expression.Call(expImporter, miWrite.MakeGenericMethod(new Type[] { propType }), expProperty, Expression.Constant(attrPM.SqlType)));
                            BuildQueryColumnText(ref isFirstColumn, tableSB, insertSB, attrPM.ColumnDefinition, attrPM.ColumnName);
                        }
                        else
                        {
                            expressions.Add(Expression.IfThenElse(
                                expIfNull, //if
                                Expression.Call(miWriteNull), //then
                                Expression.Call(expImporter, miWrite.MakeGenericMethod(new Type[] { propType }), expProperty, Expression.Constant(attrPM.SqlType)) //else
                                ));
                            BuildQueryColumnText(ref isFirstColumn, tableSB, insertSB, attrPM.ColumnDefinition, attrPM.ColumnName);
                        }
                    }
                }
                else if (prop.IsDefined(typeof(MapToModel)) && !propType.IsValueType)
                {
                    IterateCopyProperties(propType, tableSB, insertSB, expressions, expImporter, expProperty, expLogger, variables, logger);
                }
            }
            if (isFirstColumn)
            {
                throw new NoMappingAttributesFoundException();
            }
        }

        private static void BuildQueryColumnText(ref bool isFirstColumn, StringBuilder tableSB, StringBuilder insertSB, string columnDefinition, string columnName)
        {
            if (!isFirstColumn)
            {
                tableSB.AppendLine(",");
                insertSB.Append(",");
                isFirstColumn = false;
            }
            tableSB.Append("    ");
            tableSB.Append(columnDefinition);
            tableSB.Append(" NULL");
            insertSB.Append("\"");
            insertSB.Append(columnName);
            insertSB.Append("\"");
        }
        #endregion
    }
}