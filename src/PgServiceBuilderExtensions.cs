// © John Hicks. All rights reserved. Licensed under the MIT license.
// See the LICENSE file in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using ArgentSea;
using ArgentSea.Pg;
using System.IO;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// This static class adds the injectable PostgreSQL data services into the services collection.
    /// </summary>
	public static class PgServiceBuilderExtensions
	{
        /// <summary>
		/// Loads configuration into injectable Options and the DbDataStores and ShardDataStores services. ILogger service should have already be created.
		/// </summary>
		/// <param name="services"></param>
		/// <param name="config"></param>
		/// <returns></returns>
		public static IServiceCollection AddPgServices(
			this IServiceCollection services,
			IConfiguration config
			)
		{
            QueryStatement.Extension = "psql";
            var global = config.GetSection("PgGlobalSettings");
            services.Configure<PgGlobalPropertiesOptions>(global);
            services.Configure<PgDbConnectionOptions>(config);
            services.AddSingleton<PgDatabases>();
            return services;
		}

		/// <summary>
		/// Loads configuration into injectable Options and the DbDataStores and ShardDataStores services. ILogger service should have already be created.
		/// </summary>
		/// <typeparam name="TShard"></typeparam>
		/// <param name="services"></param>
		/// <param name="config"></param>
		/// <returns></returns>
		public static IServiceCollection AddPgServices<TShard>(
			this IServiceCollection services,
			IConfiguration config
			) where TShard : IComparable
		{

            services.AddPgServices(config);
            services.Configure<PgShardConnectionOptions<TShard>>(config);
            //services.AddSingleton<ShardSetsBase<TShard, PgShardConnectionOptions<TShard>>, PgShardSets<TShard>>();
            return services;
		}
	}
}
