using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using ArgentSea;
using ArgentSea.Pg;

namespace Microsoft.Extensions.DependencyInjection
{

	public static class PgServiceBuilderExtensions
	{
		private const string cResilienceOptionName = "ResilienceConfig";
		private const string cSecurityOptionName = "SecurityConfig";
		private const string cDataOptionName = "PgConfig";

		/// <summary>
		/// Loads configuration into injectable Options and the DbDataStores service. This overload does not load ShardSets.  ILogger service should have already be created.
		/// </summary>
		/// <param name="services"></param>
		/// <param name="config"></param>
		/// <returns></returns>
		public static IServiceCollection AddPgServices(
			this IServiceCollection services,
			IConfiguration config
			)
		{
			//services.Configure<DataResilienceOptions>(cResilienceOptionName, config);
			//services.Configure<DataSecurityOptions>(cSecurityOptionName, config);
			//services.AddSingleton<IDataProviderServices>(new DataProviderServices());
			//services.Configure<PgDbConnectionOptions>(cDataOptionName, config);
			services.Configure<DataResilienceOptions>(config);
			services.Configure<DataSecurityOptions>(config);
			services.AddSingleton<IDataProviderServices>(new DataProviderServices());
			services.Configure<PgDbConnectionOptions>(config);
			services.AddSingleton<DbDataStores<PgDbConnectionOptions>, DbDataStores<PgDbConnectionOptions>>();
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
			//services.Configure<PgShardConnectionOptions<TShard>>(cDataOptionName, config);
			services.Configure<PgShardConnectionOptions<TShard>>(config);
			services.AddSingleton<ShardDataStores<TShard, PgShardConnectionOptions<TShard>>, ShardDataStores<TShard, PgShardConnectionOptions<TShard>>>();
			return services;
		}
	}
}
