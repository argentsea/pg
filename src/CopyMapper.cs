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
        #region Add Copy Model<>
        public static DatabaseBatch<TResult> Add<TModel, TResult>(this DatabaseBatch<TResult> batch, List<TModel> models, string tableName) where TModel : class, new()
            => Add<TModel, TResult>(batch, (IList<TModel>)models, tableName);

        public static DatabaseBatch<TResult> Add<TModel, TResult>(this DatabaseBatch<TResult> batch, IList<TModel> models, string tableName) where TModel : class, new()
        {
            batch.Add(new CopyModelStep<TModel, TResult>(models, tableName));
            return batch;
        }

        public static ShardBatch<TResult> Add<TModel, TResult>(this ShardBatch<TResult> batch, List<TModel> models, string tableName) where TModel : class, new()
            => Add<TModel, TResult>(batch, (IList<TModel>)models, tableName);

        public static ShardBatch<TResult> Add<TModel, TResult>(this ShardBatch<TResult> batch, IList<TModel> models, string tableName) where TModel : class, new()
        {
            batch.Add(new CopyModelStep<TModel, TResult>(models, tableName));
            return batch;
        }
        #endregion
        #region Add Copy ShardKey<TRecord> List
        public static ShardBatch<TResult> Add<TRecord, TResult>(this ShardBatch<TResult> batch, List<ShardKey<TRecord>> keys, string tableName, PgParameterMapAttribute shardIdDefinition, PgParameterMapAttribute recordIdDefinition)where TRecord : IComparable
            => Add<TRecord, TResult>(batch, (IList<ShardKey<TRecord>>)keys, tableName, shardIdDefinition, recordIdDefinition);

        public static ShardBatch<TResult> Add<TRecord, TResult>(this ShardBatch<TResult> batch, IList<ShardKey<TRecord>> keys, string tableName, PgParameterMapAttribute shardIdDefinition, PgParameterMapAttribute recordIdDefinition)where TRecord : IComparable
        {
            batch.Add(new CopyKeysStep<TRecord, TResult>(keys, tableName, shardIdDefinition, recordIdDefinition));
            return batch;
        }
        #endregion
        #region Add Copy ShardKey<TRecord, TChild> List
        public static ShardBatch<TResult> Add<TRecord, TChildId, TResult>(this ShardBatch<TResult> batch, List<ShardKey<TRecord, TChildId>> keys, string tableName, PgParameterMapAttribute shardIdDefinition, PgParameterMapAttribute recordIdDefinition, PgParameterMapAttribute childIdDefinition) where TRecord : IComparable where TChildId : IComparable
            => Add<TRecord, TChildId, TResult>(batch, (IList<ShardKey<TRecord, TChildId>>)keys, tableName, shardIdDefinition, recordIdDefinition, childIdDefinition);

        public static ShardBatch<TResult> Add<TRecord, TChildId, TResult>(this ShardBatch<TResult> batch, IList<ShardKey<TRecord, TChildId>> keys, string tableName, PgParameterMapAttribute shardIdDefinition, PgParameterMapAttribute recordIdDefinition, PgParameterMapAttribute childIdDefinition) where TRecord : IComparable where TChildId : IComparable
        {
            batch.Add(new CopyChildrenStep<TRecord, TChildId, TResult>(keys, tableName, shardIdDefinition, recordIdDefinition, childIdDefinition));
            return batch;
        }

        public static ShardSetBatch Add<TRecord, TChildId>(this ShardSetBatch batch, List<ShardKey<TRecord, TChildId>> keys, string tableName, PgParameterMapAttribute shardIdDefinition, PgParameterMapAttribute recordIdDefinition, PgParameterMapAttribute childIdDefinition) where TRecord : IComparable where TChildId : IComparable
            => Add<TRecord, TChildId>(batch, (IList<ShardKey<TRecord, TChildId>>)keys, tableName, shardIdDefinition, recordIdDefinition, childIdDefinition);

        public static ShardSetBatch Add<TRecord, TChildId>(this ShardSetBatch batch, IList<ShardKey<TRecord, TChildId>> keys, string tableName, PgParameterMapAttribute shardIdDefinition, PgParameterMapAttribute recordIdDefinition, PgParameterMapAttribute childIdDefinition) where TRecord : IComparable where TChildId : IComparable
        {
            batch.Add(new CopyChildrenStep<TRecord, TChildId, object>(keys, tableName, shardIdDefinition, recordIdDefinition, childIdDefinition));
            return batch;
        }
        #endregion
        #region Add Copy ShardKey<TRecord, TChild, TGrandChild> List
        public static ShardBatch<TResult> Add<TRecord, TChildId, TGrandChildId, TResult>(this ShardBatch<TResult> batch, List<ShardKey<TRecord, TChildId, TGrandChildId>> keys, string tableName, PgParameterMapAttribute shardIdDefinition, PgParameterMapAttribute recordIdDefinition, PgParameterMapAttribute childIdDefinition, PgParameterMapAttribute grandChildIdDefinition) where TRecord : IComparable where TChildId : IComparable where TGrandChildId : IComparable
            => Add<TRecord, TChildId, TGrandChildId, TResult>(batch, (IList<ShardKey<TRecord, TChildId, TGrandChildId>>)keys, tableName, shardIdDefinition, recordIdDefinition, childIdDefinition, grandChildIdDefinition);

        public static ShardBatch<TResult> Add<TRecord, TChildId, TGrandChildId, TResult>(this ShardBatch<TResult> batch, IList<ShardKey<TRecord, TChildId, TGrandChildId>> keys, string tableName, PgParameterMapAttribute shardIdDefinition, PgParameterMapAttribute recordIdDefinition, PgParameterMapAttribute childIdDefinition, PgParameterMapAttribute grandChildIdDefinition) where TRecord : IComparable where TChildId : IComparable where TGrandChildId : IComparable
        {
            batch.Add(new CopyGrandChildrenStep<TRecord, TChildId, TGrandChildId, TResult>(keys, tableName, shardIdDefinition, recordIdDefinition, childIdDefinition, grandChildIdDefinition));
            return batch;
        }

        public static ShardSetBatch Add<TRecord, TChildId, TGrandChildId>(this ShardSetBatch batch, List<ShardKey<TRecord, TChildId, TGrandChildId>> keys, string tableName, PgParameterMapAttribute shardIdDefinition, PgParameterMapAttribute recordIdDefinition, PgParameterMapAttribute childIdDefinition, PgParameterMapAttribute grandChildIdDefinition) where TRecord : IComparable where TChildId : IComparable where TGrandChildId : IComparable
            => Add<TRecord, TChildId, TGrandChildId>(batch, (IList<ShardKey<TRecord, TChildId, TGrandChildId>>)keys, tableName, shardIdDefinition, recordIdDefinition, childIdDefinition, grandChildIdDefinition);

        public static ShardSetBatch Add<TRecord, TChildId, TGrandChildId>(this ShardSetBatch batch, IList<ShardKey<TRecord, TChildId, TGrandChildId>> keys, string tableName, PgParameterMapAttribute shardIdDefinition, PgParameterMapAttribute recordIdDefinition, PgParameterMapAttribute childIdDefinition, PgParameterMapAttribute grandChildIdDefinition) where TRecord : IComparable where TChildId : IComparable where TGrandChildId : IComparable
        {
            batch.Add(new CopyGrandChildrenStep<TRecord, TChildId, TGrandChildId, object>(keys, tableName, shardIdDefinition, recordIdDefinition, childIdDefinition, grandChildIdDefinition));
            return batch;
        }
        #endregion
        #region Add Copy ShardKey<TRecord, TChild, TGrandChild, TGreatGrandChild> List
        public static ShardBatch<TResult> Add<TRecord, TChildId, TGrandChildId, TGreatGrandChildId, TResult>(this ShardBatch<TResult> batch, List<ShardKey<TRecord, TChildId, TGrandChildId, TGreatGrandChildId>> keys, string tableName, PgParameterMapAttribute shardIdDefinition, PgParameterMapAttribute recordIdDefinition, PgParameterMapAttribute childIdDefinition, PgParameterMapAttribute grandChildIdDefinition, PgParameterMapAttribute greatGrandChildIdDefinition) where TRecord : IComparable where TChildId : IComparable where TGrandChildId : IComparable where TGreatGrandChildId : IComparable
            => Add<TRecord, TChildId, TGrandChildId, TGreatGrandChildId, TResult>(batch, (IList<ShardKey<TRecord, TChildId, TGrandChildId, TGreatGrandChildId>>)keys, tableName, shardIdDefinition, recordIdDefinition, childIdDefinition, grandChildIdDefinition, greatGrandChildIdDefinition);

        public static ShardBatch<TResult> Add<TRecord, TChildId, TGrandChildId, TGreatGrandChildId, TResult>(this ShardBatch<TResult> batch, IList<ShardKey<TRecord, TChildId, TGrandChildId, TGreatGrandChildId>> keys, string tableName, PgParameterMapAttribute shardIdDefinition, PgParameterMapAttribute recordIdDefinition, PgParameterMapAttribute childIdDefinition, PgParameterMapAttribute grandChildIdDefinition, PgParameterMapAttribute greatGrandChildIdDefinition) where TRecord : IComparable where TChildId : IComparable where TGrandChildId : IComparable where TGreatGrandChildId : IComparable
        {
            batch.Add(new CopyGreatGrandChildrenStep<TRecord, TChildId, TGrandChildId, TGreatGrandChildId, TResult>(keys, tableName, shardIdDefinition, recordIdDefinition, childIdDefinition, grandChildIdDefinition, greatGrandChildIdDefinition));
            return batch;
        }

        public static ShardSetBatch Add<TRecord, TChildId, TGrandChildId, TGreatGrandChildId>(this ShardSetBatch batch, List<ShardKey<TRecord, TChildId, TGrandChildId, TGreatGrandChildId>> keys, string tableName, PgParameterMapAttribute shardIdDefinition, PgParameterMapAttribute recordIdDefinition, PgParameterMapAttribute childIdDefinition, PgParameterMapAttribute grandChildIdDefinition, PgParameterMapAttribute greatGrandChildIdDefinition) where TRecord : IComparable where TChildId : IComparable where TGrandChildId : IComparable where TGreatGrandChildId : IComparable
            => Add<TRecord, TChildId, TGrandChildId, TGreatGrandChildId>(batch, (IList<ShardKey<TRecord, TChildId, TGrandChildId, TGreatGrandChildId>>)keys, tableName, shardIdDefinition, recordIdDefinition, childIdDefinition, grandChildIdDefinition, greatGrandChildIdDefinition);

        public static ShardSetBatch Add<TRecord, TChildId, TGrandChildId, TGreatGrandChildId>(this ShardSetBatch batch, IList<ShardKey<TRecord, TChildId, TGrandChildId, TGreatGrandChildId>> keys, string tableName, PgParameterMapAttribute shardIdDefinition, PgParameterMapAttribute recordIdDefinition, PgParameterMapAttribute childIdDefinition, PgParameterMapAttribute grandChildIdDefinition, PgParameterMapAttribute greatGrandChildIdDefinition) where TRecord : IComparable where TChildId : IComparable where TGrandChildId : IComparable where TGreatGrandChildId : IComparable
        {
            batch.Add(new CopyGreatGrandChildrenStep<TRecord, TChildId, TGrandChildId, TGreatGrandChildId, object>(keys, tableName, shardIdDefinition, recordIdDefinition, childIdDefinition, grandChildIdDefinition, greatGrandChildIdDefinition));
            return batch;
        }

        #endregion
        #region Add Copy List<Model>
        public static ShardSetBatch Add<TModel>(this ShardSetBatch batch, List<TModel> models, string tableName) where TModel : class, new()
            => Add<TModel>(batch, (IList<TModel>)models, tableName);

        public static ShardSetBatch Add<TModel>(this ShardSetBatch batch, IList<TModel> models, string tableName) where TModel : class, new()
        {
            batch.Add(new CopyModelStep<TModel, object>(models, tableName));
            return batch;
        }
        #endregion
        #region Add Copy List<TRecord>
        public static ShardSetBatch Add<TRecord>(this ShardSetBatch batch, List<ShardKey<TRecord>> keys, string tableName, PgParameterMapAttribute shardIdDefinition, PgParameterMapAttribute recordIdDefinition) where TRecord : IComparable
            => Add<TRecord>(batch, (IList<ShardKey<TRecord>>)keys, tableName, shardIdDefinition, recordIdDefinition);

        public static ShardSetBatch Add<TRecord>(this ShardSetBatch batch, IList<ShardKey<TRecord>> keys, string tableName, PgParameterMapAttribute shardIdDefinition, PgParameterMapAttribute recordIdDefinition) where TRecord : IComparable
        {
            batch.Add(new CopyKeysStep<TRecord, object>(keys, tableName, shardIdDefinition, recordIdDefinition));
            return batch;
        }
        #endregion
        #endregion
        #region Private classes
        private class CopyKeysStep<TRecord, TResult> : BatchStep<TResult>where TRecord : IComparable
        {
            private readonly IList<ShardKey<TRecord>> _keys;
            private readonly string _tableName;
            private readonly PgParameterMapAttribute _shardIdDefinition;
            private readonly PgParameterMapAttribute _recordIdDefinition;

            public CopyKeysStep(IList<ShardKey<TRecord>> keys, string tableName, PgParameterMapAttribute shardIdDefinition, PgParameterMapAttribute recordIdDefinition)
            {
                _keys = keys;
                _tableName = tableName;
                _shardIdDefinition = shardIdDefinition;
                _recordIdDefinition = recordIdDefinition;
            }
            protected override async Task<TResult> Execute(short shardId, DbConnection connection, DbTransaction transaction, string connectionName, IDataProviderServiceFactory services, ILogger logger, CancellationToken cancellation)
            {
                CopyMapper.ValidatePgTableName(_tableName);
                cancellation.ThrowIfCancellationRequested();

                var cmd = connection.CreateCommand();
                cmd.Transaction = transaction;

                string importDef;
                if (_tableName.Contains('.'))
                {
                    var parsedTableName = _tableName.Replace(".", "\".\"");
                    cmd.CommandText = $"CREATE TABLE IF NOT EXISTS \"{ parsedTableName }\" (\"{ _shardIdDefinition.ColumnDefinition }\", \"{ _recordIdDefinition.ColumnDefinition }\");";
                    importDef = $"COPY \"{ parsedTableName }\" (\"{ _shardIdDefinition.ColumnName }\", \"{ _recordIdDefinition.ColumnName }\") FROM STDIN (FORMAT BINARY)";
                }
                else
                {
                    cmd.CommandText = $"CREATE TEMP TABLE IF NOT EXISTS \"{ _tableName }\" ({ _shardIdDefinition.ColumnDefinition}, { _recordIdDefinition.ColumnDefinition}) ON COMMIT DROP;";
                    importDef = $"COPY \"{ _tableName }\" (\"{ _shardIdDefinition.ColumnName }\", \"{ _recordIdDefinition.ColumnName }\") FROM STDIN (FORMAT BINARY)";
                }
                logger.CopySqlStatements(cmd.CommandText, importDef);

                cancellation.ThrowIfCancellationRequested();
                await cmd.ExecuteNonQueryAsync(cancellation);

                using (var importer = ((NpgsqlConnection)connection).BeginBinaryImport(importDef))
                {
                    foreach (var key in _keys)
                    {
                        importer.StartRow();
                        importer.Write(key.ShardId);
                        importer.Write<TRecord>(key.RecordId);
                    }
                    importer.Complete();
                }
                return default(TResult);
            }
        }

        private class CopyChildrenStep<TRecord, TChildId, TResult> : BatchStep<TResult>where TRecord : IComparable where TChildId : IComparable
        {
            private readonly IList<ShardKey<TRecord, TChildId>> _keys;
            private readonly string _tableName;
            private readonly PgParameterMapAttribute _shardIdDefinition;
            private readonly PgParameterMapAttribute _recordIdDefinition;
            private readonly PgParameterMapAttribute _childIdDefinition;

            public CopyChildrenStep(IList<ShardKey<TRecord, TChildId>> keys, string tableName, PgParameterMapAttribute shardIdDefinition, PgParameterMapAttribute recordIdDefinition, PgParameterMapAttribute childIdDefinition)
            {
                _keys = keys;
                _tableName = tableName;
                _shardIdDefinition = shardIdDefinition;
                _recordIdDefinition = recordIdDefinition;
                _childIdDefinition = childIdDefinition;
            }
            protected override async Task<TResult> Execute(short shardId, DbConnection connection, DbTransaction transaction, string connectionName, IDataProviderServiceFactory services, ILogger logger, CancellationToken cancellation)
            {
                ValidatePgTableName(_tableName);
                cancellation.ThrowIfCancellationRequested();

                var cmd = connection.CreateCommand();
                cmd.Transaction = transaction;

                string importDef;
                if (_tableName.Contains('.'))
                {
                    var parsedTableName = _tableName.Replace(".", "\".\"");
                    cmd.CommandText = $"CREATE TABLE IF NOT EXISTS \"{ parsedTableName }\" (\"{ _shardIdDefinition.ColumnDefinition }\", \"{ _recordIdDefinition.ColumnDefinition }\", \"{ _childIdDefinition.ColumnDefinition }\");";
                    importDef = $"COPY \"{ parsedTableName }\" (\"{ _shardIdDefinition.ColumnName }\", \"{ _recordIdDefinition.ColumnName }\", \"{_childIdDefinition.ColumnName}\") FROM STDIN (FORMAT BINARY)";
                }
                else
                {
                    cmd.CommandText = $"CREATE TEMP TABLE IF NOT EXISTS \"{ _tableName }\" (\"{ _shardIdDefinition.ColumnDefinition}\", \"{ _recordIdDefinition.ColumnDefinition}\", \"{ _childIdDefinition.ColumnDefinition}\") ON COMMIT DROP;";
                    importDef = $"COPY \"{ _tableName }\" (\"{ _shardIdDefinition.ColumnName }\", \"{ _recordIdDefinition.ColumnName }\", \"{_childIdDefinition.ColumnName}\") FROM STDIN (FORMAT BINARY)";
                }
                logger.CopySqlStatements(cmd.CommandText, importDef);

                cancellation.ThrowIfCancellationRequested();
                await cmd.ExecuteNonQueryAsync(cancellation);

                using (var importer = ((NpgsqlConnection)connection).BeginBinaryImport(importDef))
                {
                    foreach (var key in _keys)
                    {
                        importer.StartRow();
                        importer.Write(key.ShardId);
                        importer.Write<TRecord>(key.RecordId);
                        importer.Write<TChildId>(key.ChildId);
                    }
                    importer.Complete();
                }
                return default(TResult);
            }
        }

        private class CopyGrandChildrenStep<TRecord, TChildId, TGrandChildId, TResult> : BatchStep<TResult> where TRecord : IComparable where TChildId : IComparable where TGrandChildId : IComparable
        {
            private readonly IList<ShardKey<TRecord, TChildId, TGrandChildId>> _keys;
            private readonly string _tableName;
            private readonly PgParameterMapAttribute _shardIdDefinition;
            private readonly PgParameterMapAttribute _recordIdDefinition;
            private readonly PgParameterMapAttribute _childIdDefinition;
            private readonly PgParameterMapAttribute _grandChildIdDefinition;

            public CopyGrandChildrenStep(IList<ShardKey<TRecord, TChildId, TGrandChildId>> keys, string tableName, PgParameterMapAttribute shardIdDefinition, PgParameterMapAttribute recordIdDefinition, PgParameterMapAttribute childIdDefinition, PgParameterMapAttribute grandChildIdDefinition)
            {
                _keys = keys;
                _tableName = tableName;
                _shardIdDefinition = shardIdDefinition;
                _recordIdDefinition = recordIdDefinition;
                _childIdDefinition = childIdDefinition;
                _grandChildIdDefinition = grandChildIdDefinition;
            }
            protected override async Task<TResult> Execute(short shardId, DbConnection connection, DbTransaction transaction, string connectionName, IDataProviderServiceFactory services, ILogger logger, CancellationToken cancellation)
            {
                ValidatePgTableName(_tableName);
                cancellation.ThrowIfCancellationRequested();

                var cmd = connection.CreateCommand();
                cmd.Transaction = transaction;

                string importDef;
                if (_tableName.Contains('.'))
                {
                    var parsedTableName = _tableName.Replace(".", "\".\"");
                    cmd.CommandText = $"CREATE TABLE IF NOT EXISTS \"{ parsedTableName }\" (\"{ _shardIdDefinition.ColumnDefinition }\", \"{ _recordIdDefinition.ColumnDefinition }\", \"{ _childIdDefinition.ColumnDefinition }\", \"{ _grandChildIdDefinition.ColumnDefinition }\");";
                    importDef = $"COPY \"{ parsedTableName }\" (\"{ _shardIdDefinition.ColumnName }\", \"{ _recordIdDefinition.ColumnName }\", \"{_childIdDefinition.ColumnName}\", \"{_grandChildIdDefinition.ColumnName}\") FROM STDIN (FORMAT BINARY)";
                }
                else
                {
                    cmd.CommandText = $"CREATE TEMP TABLE IF NOT EXISTS \"{ _tableName }\" (\"{ _shardIdDefinition.ColumnDefinition}\", \"{ _recordIdDefinition.ColumnDefinition}\", \"{ _childIdDefinition.ColumnDefinition}\", \"{ _grandChildIdDefinition.ColumnDefinition }\") ON COMMIT DROP;";
                    importDef = $"COPY \"{ _tableName }\" (\"{ _shardIdDefinition.ColumnName }\", \"{ _recordIdDefinition.ColumnName }\", \"{_childIdDefinition.ColumnName}\", \"{_grandChildIdDefinition.ColumnName}\") FROM STDIN (FORMAT BINARY)";
                }
                logger.CopySqlStatements(cmd.CommandText, importDef);

                cancellation.ThrowIfCancellationRequested();
                await cmd.ExecuteNonQueryAsync(cancellation);

                using (var importer = ((NpgsqlConnection)connection).BeginBinaryImport(importDef))
                {
                    foreach (var key in _keys)
                    {
                        importer.StartRow();
                        importer.Write(key.ShardId);
                        importer.Write<TRecord>(key.RecordId);
                        importer.Write<TChildId>(key.ChildId);
                        importer.Write<TGrandChildId>(key.GrandChildId);
                    }
                    importer.Complete();
                }
                return default(TResult);
            }
        }


        private class CopyGreatGrandChildrenStep<TRecord, TChildId, TGrandChildId, TGreatGrandChildId, TResult> : BatchStep<TResult> where TRecord : IComparable where TChildId : IComparable where TGrandChildId : IComparable where TGreatGrandChildId : IComparable
        {
            private readonly IList<ShardKey<TRecord, TChildId, TGrandChildId, TGreatGrandChildId>> _keys;
            private readonly string _tableName;
            private readonly PgParameterMapAttribute _shardIdDefinition;
            private readonly PgParameterMapAttribute _recordIdDefinition;
            private readonly PgParameterMapAttribute _childIdDefinition;
            private readonly PgParameterMapAttribute _grandChildIdDefinition;
            private readonly PgParameterMapAttribute _greatGrandChildIdDefinition;

            public CopyGreatGrandChildrenStep(IList<ShardKey<TRecord, TChildId, TGrandChildId, TGreatGrandChildId>> keys, string tableName, PgParameterMapAttribute shardIdDefinition, PgParameterMapAttribute recordIdDefinition, PgParameterMapAttribute childIdDefinition, PgParameterMapAttribute grandChildIdDefinition, PgParameterMapAttribute greatGrandChildIdDefinition)
            {
                _keys = keys;
                _tableName = tableName;
                _shardIdDefinition = shardIdDefinition;
                _recordIdDefinition = recordIdDefinition;
                _childIdDefinition = childIdDefinition;
                _grandChildIdDefinition = grandChildIdDefinition;
                _greatGrandChildIdDefinition = greatGrandChildIdDefinition;
            }
            protected override async Task<TResult> Execute(short shardId, DbConnection connection, DbTransaction transaction, string connectionName, IDataProviderServiceFactory services, ILogger logger, CancellationToken cancellation)
            {
                ValidatePgTableName(_tableName);
                cancellation.ThrowIfCancellationRequested();

                var cmd = connection.CreateCommand();
                cmd.Transaction = transaction;

                string importDef;
                if (_tableName.Contains('.'))
                {
                    var parsedTableName = _tableName.Replace(".", "\".\"");
                    cmd.CommandText = $"CREATE TABLE IF NOT EXISTS \"{ parsedTableName }\" (\"{ _shardIdDefinition.ColumnDefinition }\", \"{ _recordIdDefinition.ColumnDefinition }\", \"{ _childIdDefinition.ColumnDefinition }\", \"{ _grandChildIdDefinition.ColumnDefinition }\", \"{ _greatGrandChildIdDefinition.ColumnDefinition }\");";
                    importDef = $"COPY \"{ parsedTableName }\" (\"{ _shardIdDefinition.ColumnName }\", \"{ _recordIdDefinition.ColumnName }\", \"{_childIdDefinition.ColumnName}\", \"{_grandChildIdDefinition.ColumnName}\", \"{_greatGrandChildIdDefinition.ColumnName}\") FROM STDIN (FORMAT BINARY)";
                }
                else
                {
                    cmd.CommandText = $"CREATE TEMP TABLE IF NOT EXISTS \"{ _tableName }\" (\"{ _shardIdDefinition.ColumnDefinition}\", \"{ _recordIdDefinition.ColumnDefinition}\", \"{ _childIdDefinition.ColumnDefinition}\", \"{ _grandChildIdDefinition.ColumnDefinition }\", \"{ _greatGrandChildIdDefinition.ColumnDefinition }\") ON COMMIT DROP;";
                    importDef = $"COPY \"{ _tableName }\" (\"{ _shardIdDefinition.ColumnName }\", \"{ _recordIdDefinition.ColumnName }\", \"{_childIdDefinition.ColumnName}\", \"{_grandChildIdDefinition.ColumnName}\", \"{_greatGrandChildIdDefinition.ColumnName}\") FROM STDIN (FORMAT BINARY)";
                }
                logger.CopySqlStatements(cmd.CommandText, importDef);

                cancellation.ThrowIfCancellationRequested();
                await cmd.ExecuteNonQueryAsync(cancellation);

                using (var importer = ((NpgsqlConnection)connection).BeginBinaryImport(importDef))
                {
                    foreach (var key in _keys)
                    {
                        importer.StartRow();
                        importer.Write(key.ShardId);
                        importer.Write<TRecord>(key.RecordId);
                        importer.Write<TChildId>(key.ChildId);
                        importer.Write<TGrandChildId>(key.GrandChildId);
                        importer.Write<TGreatGrandChildId>(key.GreatGrandChildId);
                    }
                    importer.Complete();
                }
                return default(TResult);
            }
        }


        private class CopyModelStep<TModel, TResult> : BatchStep<TResult> where TModel : class, new()
        {
            private readonly IList<TModel> _models;
            private readonly string _tableName;
            public CopyModelStep(IList<TModel> models, string tableName)
            {
                _models = models;
                _tableName = tableName;
            }
            protected override async Task<TResult> Execute(short shardId, DbConnection connection, DbTransaction transaction, string connectionName, IDataProviderServiceFactory services, ILogger logger, CancellationToken cancellation)
            {
                ValidatePgTableName(_tableName);
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
                    importDef = meta.importDef.Replace("<<TABLEDEF>>", $"\"{ parsedTableName }\"");
                }
                else
                {
                    tableDef = meta.tableDef.Replace("<<TABLEDEF>>", $"TEMP TABLE  \"{ _tableName }\"");
                    tableDef = tableDef.Replace(");", ") ON COMMIT DROP;");
                    importDef = meta.importDef.Replace("<<TABLEDEF>>", $"\"{ _tableName }\"");
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

        #region Helper methods

        private static void ValidatePgTableName(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new Exception($"The table name was not defined.");
            }
            if (tableName.Length > 63)
            {
                throw new Exception($"The table name, “{ tableName }”, is not valid. PostgreSQL table names cannot exceed 63 characters.");
            }
            if (!char.IsLetter(tableName[0]) && tableName[0] != '_')
            {
                throw new Exception($"The table name, “{ tableName }”, is not valid. PostgreSQL table names must begin with a letter or underscore.");
            }
            if (!tableName.All((c) => { return char.IsLetterOrDigit(c) || c == '.' || c == '_' || c == '$'; }))
            {
                throw new Exception($"The table name, “{ tableName }”, can only contain letters, numbers, dollar signs, underscores, or a “.” schema separator.");
            }
        }

        private static ConcurrentDictionary<Type, Lazy<(string tableDef, string importDef, Delegate setRow)>> _setCopyParamCache = new ConcurrentDictionary<Type, Lazy<(string tableDef, string importDef, Delegate setRow)>>();

        private static (string, string, Action<TModel, NpgsqlBinaryImporter, ILogger>) BuildCopyDelegateAndQuery<TModel>(ILogger logger) where TModel : class
        {
            var tModel = typeof(TModel);
            var variables = new List<ParameterExpression>();
            var expressions = new List<Expression>();

            var prmModel = Expression.Parameter(tModel, "model");
            var expImporter = Expression.Parameter(typeof(NpgsqlBinaryImporter), "importer");
            var expLogger = Expression.Parameter(typeof(ILogger), "logger");
            var exprInPrms = new ParameterExpression[] { prmModel, expImporter, expLogger };

            expressions.Add(Expression.Call(expImporter, typeof(NpgsqlBinaryImporter).GetMethod(nameof(NpgsqlBinaryImporter.StartRow))));

            var tableSB = new StringBuilder();
            var insertSB = new StringBuilder();
            tableSB.AppendLine("CREATE <<TABLEDEF>> (");
            insertSB.Append("COPY <<TABLEDEF>> (");

            var isFirstColumn = true;
            IterateCopyProperties(tModel, ref isFirstColumn, tableSB, insertSB, expressions, expImporter, prmModel, expLogger, variables, logger);

            tableSB.Append(");");
            insertSB.Append(") FROM STDIN BINARY;");

            var expBlock = Expression.Block(variables, expressions);
            var lambda = Expression.Lambda<Action<TModel, NpgsqlBinaryImporter, ILogger>>(expBlock, exprInPrms);
            logger.CreatedExpressionTreeForCopy(tModel, expBlock);
            return (tableSB.ToString(), insertSB.ToString(), lambda.Compile());
        }

        private static void IterateCopyProperties(Type tModel, ref bool isFirstColumn, StringBuilder tableSB, StringBuilder insertSB, List<Expression> expressions, ParameterExpression expImporter, Expression prmModel, ParameterExpression expLogger, List<ParameterExpression> variables, ILogger logger)
        {
            var unorderedProps = tModel.GetProperties();
            var props = unorderedProps.OrderBy(prop => prop.MetadataToken);
            var miLogTrace = typeof(PgLoggingExtensions).GetMethod(nameof(PgLoggingExtensions.TraceCopyMapperProperty));

            var miWrite = typeof(NpgsqlBinaryImporter).GetMethods().Single(
                method =>
                    method.Name == "Write" &&
                    method.GetGenericArguments().Length == 1 &&
                    method.GetParameters().Length == 2 &&
                    method.GetParameters()[1].ParameterType == typeof(NpgsqlTypes.NpgsqlDbType)
                );

            var miWriteNull = typeof(NpgsqlBinaryImporter).GetMethod(nameof(NpgsqlBinaryImporter.WriteNull));
            foreach (var prop in props)
            {
                MemberExpression expOriginalProperty = Expression.Property(prmModel, prop);
                Expression expProperty = expOriginalProperty;
                Type propType = prop.PropertyType;
                var shdAttr = ExpressionHelpers.GetMapShardKeyAttribute(prop, propType, out var isNullable, out var isShardKey, out var isShardChild, out var isShardGrandChild, out var isShardGreatGrandChild);

                if (!(shdAttr is null) && (isShardKey || isShardChild || isShardGrandChild || isShardGrandChild))
                {
                    var foundRecordId = false;
                    var foundChildId = false;
                    var foundGrandChildId = false;
                    var foundGreatGrandChildId = false;
                    expressions.Add(Expression.Call(miLogTrace, expLogger, Expression.Constant(prop.Name)));

                    Expression expIsNotNullOrEmpty;
                    if (isNullable)
                    {
                        expProperty = Expression.Property(expProperty, propType.GetProperty(nameof(Nullable<int>.Value)));
                        propType = Nullable.GetUnderlyingType(propType);
                        expIsNotNullOrEmpty = Expression.Property(expOriginalProperty, propType.GetProperty(nameof(Nullable<int>.HasValue)));
                    }
                    else
                    {
                        expIsNotNullOrEmpty = Expression.NotEqual(expOriginalProperty, Expression.Property(null, propType.GetProperty(nameof(ShardKey<int>.Empty))));
                    }

                    if (!(shdAttr.ShardParameter is null))
                    {
                        var expShardProperty = Expression.Property(expProperty, propType.GetProperty(nameof(ShardKey<int>.ShardId)));
                        expressions.Add(Expression.IfThenElse(
                            expIsNotNullOrEmpty, //if
                            Expression.Call(expImporter, miWrite.MakeGenericMethod(new Type[] { expShardProperty.Type }), expShardProperty, Expression.Constant(NpgsqlTypes.NpgsqlDbType.Smallint)), //then
                            Expression.Call(expImporter, miWriteNull) //else
                            ));
                        BuildQueryColumnText(ref isFirstColumn, tableSB, insertSB, $"\"{shdAttr.ShardIdName}\" smallint", shdAttr.ShardIdName);
                    }


                    var attrPMs = prop.GetCustomAttributes<PgParameterMapAttribute>(true);
                    foreach (var attrPM in attrPMs)
                    {
                        if (attrPM.Name == shdAttr.RecordIdName)
                        {
                            foundRecordId = true;
                            var tDataRecordId = propType.GetGenericArguments()[0];
                            if (!attrPM.IsValidType(tDataRecordId))
                            {
                                throw new InvalidMapTypeException(prop, attrPM.SqlType);
                            }
                            var expRecordProperty = Expression.Property(expProperty, propType.GetProperty(nameof(ShardKey<int>.RecordId)));
                            expressions.Add(Expression.IfThenElse(
                                expIsNotNullOrEmpty, //if
                                Expression.Call(expImporter, miWrite.MakeGenericMethod(new Type[] { expRecordProperty.Type }), expRecordProperty, Expression.Constant((NpgsqlTypes.NpgsqlDbType)attrPM.SqlType)), //then
                                Expression.Call(expImporter, miWriteNull) //else
                                ));
                            BuildQueryColumnText(ref isFirstColumn, tableSB, insertSB, attrPM.ColumnDefinition, attrPM.ColumnName);
                        }
                        if ((isShardChild || isShardGrandChild || isShardGreatGrandChild) && attrPM.Name == shdAttr.ChildIdName)
                        {
                            foundChildId = true;
                            var tDataChildId = propType.GetGenericArguments()[1];
                            if (!attrPM.IsValidType(tDataChildId))
                            {
                                throw new InvalidMapTypeException(prop, attrPM.SqlType);
                            }
                            var expChildProperty = Expression.Property(expProperty, propType.GetProperty(nameof(ShardKey<int, int>.ChildId)));
                            expressions.Add(Expression.IfThenElse(
                                expIsNotNullOrEmpty, //if
                                Expression.Call(expImporter, miWrite.MakeGenericMethod(new Type[] { expChildProperty.Type }), expChildProperty, Expression.Constant((NpgsqlTypes.NpgsqlDbType)attrPM.SqlType)), //then
                                Expression.Call(expImporter, miWriteNull) //else
                                ));
                            BuildQueryColumnText(ref isFirstColumn, tableSB, insertSB, attrPM.ColumnDefinition, attrPM.ColumnName);
                        }
                        if ((isShardGrandChild || isShardGreatGrandChild) && attrPM.Name == shdAttr.GrandChildIdName)
                        {
                            foundGrandChildId = true;
                            var tDataChildId = propType.GetGenericArguments()[2];
                            if (!attrPM.IsValidType(tDataChildId))
                            {
                                throw new InvalidMapTypeException(prop, attrPM.SqlType);
                            }
                            var expChildProperty = Expression.Property(expProperty, propType.GetProperty(nameof(ShardKey<int, int, int>.ChildId)));
                            expressions.Add(Expression.IfThenElse(
                                expIsNotNullOrEmpty, //if
                                Expression.Call(expImporter, miWrite.MakeGenericMethod(new Type[] { expChildProperty.Type }), expChildProperty, Expression.Constant((NpgsqlTypes.NpgsqlDbType)attrPM.SqlType)), //then
                                Expression.Call(expImporter, miWriteNull) //else
                                ));
                            BuildQueryColumnText(ref isFirstColumn, tableSB, insertSB, attrPM.ColumnDefinition, attrPM.ColumnName);
                        }
                        if (isShardGreatGrandChild && attrPM.Name == shdAttr.GreatGrandChildIdName)
                        {
                            foundGreatGrandChildId = true;
                            var tDataChildId = propType.GetGenericArguments()[3];
                            if (!attrPM.IsValidType(tDataChildId))
                            {
                                throw new InvalidMapTypeException(prop, attrPM.SqlType);
                            }
                            var expChildProperty = Expression.Property(expProperty, propType.GetProperty(nameof(ShardKey<int, int, int, int>.ChildId)));
                            expressions.Add(Expression.IfThenElse(
                                expIsNotNullOrEmpty, //if
                                Expression.Call(expImporter, miWrite.MakeGenericMethod(new Type[] { expChildProperty.Type }), expChildProperty, Expression.Constant((NpgsqlTypes.NpgsqlDbType)attrPM.SqlType)), //then
                                Expression.Call(expImporter, miWriteNull) //else
                                ));
                            BuildQueryColumnText(ref isFirstColumn, tableSB, insertSB, attrPM.ColumnDefinition, attrPM.ColumnName);
                        }
                    }
                    if (!foundRecordId)
                    {
                        throw new MapAttributeMissingException(MapAttributeMissingException.ShardElement.RecordId, shdAttr.RecordIdName);
                    }
                    if ((isShardChild || isShardGrandChild || isShardGreatGrandChild) && !foundChildId)
                    {
                        throw new MapAttributeMissingException(MapAttributeMissingException.ShardElement.ChildId, shdAttr.ChildIdName);
                    }
                    if ((isShardGrandChild || isShardGreatGrandChild) && !foundGrandChildId)
                    {
                        throw new MapAttributeMissingException(MapAttributeMissingException.ShardElement.GrandChildId, shdAttr.GrandChildIdName);
                    }
                    if (isShardGreatGrandChild && !foundGreatGrandChildId)
                    {
                        throw new MapAttributeMissingException(MapAttributeMissingException.ShardElement.GreatGrandChildId, shdAttr.GreatGrandChildIdName);
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
                        else if (isNullable)
                        {
                            expProperty = Expression.Property(expProperty, propType.GetProperty(nameof(Nullable<int>.Value)));
                            propType = Nullable.GetUnderlyingType(propType);
                            if (propType.IsEnum)
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
                                    propType = Enum.GetUnderlyingType(propType);
                                    expProperty = Expression.Convert(expProperty, propType);
                                }
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
                                propType = Enum.GetUnderlyingType(propType);
                                expProperty = Expression.Convert(expProperty, propType);
                            }

                        }
                        else if (!propType.IsValueType)
                        {
                            expIfNull = Expression.Equal(expProperty, Expression.Constant(null, propType));
                        }
                        if (expIfNull is null)
                        {
                            expressions.Add(Expression.Call(expImporter, miWrite.MakeGenericMethod(new Type[] { propType }), expProperty, Expression.Constant((NpgsqlTypes.NpgsqlDbType)attrPM.SqlType)));
                            BuildQueryColumnText(ref isFirstColumn, tableSB, insertSB, attrPM.ColumnDefinition, attrPM.ColumnName);
                        }
                        else
                        {
                            expressions.Add(Expression.IfThenElse(
                                expIfNull, //if
                                Expression.Call(expImporter, miWriteNull), //then
                                Expression.Call(expImporter, miWrite.MakeGenericMethod(new Type[] { propType }), expProperty, Expression.Constant((NpgsqlTypes.NpgsqlDbType)attrPM.SqlType)) //else
                                ));
                            BuildQueryColumnText(ref isFirstColumn, tableSB, insertSB, attrPM.ColumnDefinition, attrPM.ColumnName);
                        }
                    }
                }
                else if (prop.IsDefined(typeof(MapToModel)) && !propType.IsValueType)
                {
                    IterateCopyProperties(propType, ref isFirstColumn, tableSB, insertSB, expressions, expImporter, expProperty, expLogger, variables, logger);
                }
            }
            if (isFirstColumn)
            {
                throw new NoMappingAttributesFoundException($"No mapping attributes could be found on the “{ tModel.ToString() }” model class provided to the Mapper.");
            }
        }

        private static void BuildQueryColumnText(ref bool isFirstColumn, StringBuilder tableSB, StringBuilder insertSB, string columnDefinition, string columnName)
        {
            if (!isFirstColumn)
            {
                tableSB.AppendLine(",");
                insertSB.Append(",");
            }
            else
            {
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