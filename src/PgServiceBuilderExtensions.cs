using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using ArgentSea;
using ArgentSea.Pg;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// This static class adds the injectable PostgreSQL data services into the services collection.
    /// </summary>
	public static class PgServiceBuilderExtensions
	{
        //private static IConfiguration _config;

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
            services.Configure<DataResilienceOptions>(config);
            services.Configure<DataSecurityOptions>(config);
            services.AddSingleton<PgDatabases>();
            services.Configure<PgDbConnectionOptions>(config);
            //_config = config;
            //services.PostConfigure<PgDbConnectionOptions>(FixAnnoyingConfigBindingThatWontWorkNoMatterWhat);
            return services;
		}

        //private static void FixAnnoyingConfigBindingThatWontWorkNoMatterWhat(PgDbConnectionOptions obj)
        //{
        //    var cfg = _config.GetSection("PgDbConnections");
        //    for (var i = 0; i < obj.PgDbConnections.Length; i++)
        //    {
        //        if (obj.PgDbConnections[i] is null)
        //        {
        //            obj.PgDbConnections[i] = new PgDbConnectionConfiguration();
        //            var cfgInstance = cfg.GetSection(i.ToString());
        //            if (cfgInstance is null)
        //            {
        //                throw new Exception($"No config for PgDbConnections:{i.ToString()}");
        //            }
        //            ConfigurationBinder.Bind(cfgInstance, obj.PgDbConnections[i]);
        //        }
        //    }
        //    _config = null;
        //}


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
			services.AddSingleton<ShardDataStores<TShard, PgShardConnectionOptions<TShard>>, ShardDataStores<TShard, PgShardConnectionOptions<TShard>>>();
			return services;
		}
	}
}
