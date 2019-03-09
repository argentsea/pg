// © John Hicks. All rights reserved. Licensed under the MIT license.
// See the LICENSE file in the repository root for more information.

using System;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using System.Globalization;

namespace ArgentSea.Pg
{
    /// <summary>
    /// This static class provides the logging extension methods for handling table-valued parameter (TVP) mapping.
    /// </summary>
    public static class PgLoggingExtensions
    {

        private static readonly Action<ILogger, Type, Exception> _pgCopyCacheMiss;
        private static readonly Action<ILogger, Type, Exception> _pgCopyCacheHit;
        private static readonly Action<ILogger, string, Exception> _pgMapperCopyTrace;
        private static readonly Action<ILogger, string, string, Exception> _pgGetInExpressionTreeCreation;
        private static readonly Action<ILogger, string, string, Exception> _pgCopySqlStatements;

        static PgLoggingExtensions()
        {
            _pgCopyCacheMiss = LoggerMessage.Define<Type>(LogLevel.Information, new EventId((int)LoggingExtensions.EventIdentifier.MapperCacheStatus, nameof(PgCopyCacheMiss)), "No cached delegate was found for mapping type {modelT} to Pg row metadata; this is normal for the first execution.");
            _pgCopyCacheHit = LoggerMessage.Define<Type>(LogLevel.Debug, new EventId((int)LoggingExtensions.EventIdentifier.MapperCacheStatus, nameof(PgCopyCacheHit)), "A cached delegate for mapping type {modelT} to Pg row metadata was found.");
            _pgMapperCopyTrace = LoggerMessage.Define<string>(LogLevel.Trace, new EventId((int)LoggingExtensions.EventIdentifier.MapperProcessTrace, nameof(TraceCopyMapperProperty)), "Copy mapper is now processing property {name}.");
            _pgGetInExpressionTreeCreation = LoggerMessage.Define<string, string>(LogLevel.Debug, new EventId((int)LoggingExtensions.EventIdentifier.ExpressionTreeCreation, nameof(CreatedExpressionTreeForCopy)), "Compiled code to map model {model} to input parameters as:\r\n{code}.");
            _pgCopySqlStatements = LoggerMessage.Define<string, string>(LogLevel.Debug, new EventId((int)LoggingExtensions.EventIdentifier.CmdExecuted, nameof(CopySqlStatements)), "Copy mapper used a create table definition of: \r\n{tableDef} \r\n and used the copy import statment of:\r\n{copyDef}.");

        }

        public static void PgCopyCacheMiss(this ILogger logger, Type modelT)
        {
            _pgCopyCacheMiss(logger, modelT, null);
        }
        public static void PgCopyCacheHit(this ILogger logger, Type modelT)
        {
            _pgCopyCacheHit(logger, modelT, null);
        }
        public static void TraceCopyMapperProperty(this ILogger logger, string propertyName)
        {
            _pgMapperCopyTrace(logger, propertyName, null);
        }
        public static void CreatedExpressionTreeForCopy(this ILogger logger, Type model, Expression codeBlock)
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                using (System.IO.StringWriter writer = new System.IO.StringWriter(CultureInfo.CurrentCulture))
                {
                    DebugViewWriter.WriteTo(codeBlock, writer);
                    _pgGetInExpressionTreeCreation(logger, model.ToString(), writer.ToString(), null);
                }
            }
        }
        public static void CopySqlStatements(this ILogger logger, string tableDef, string importerDef)
        {
            _pgCopySqlStatements(logger, tableDef, importerDef, null);
        }

    }
}
