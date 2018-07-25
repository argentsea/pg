using System;
using System.Threading;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ArgentSea.Pg
{
    /// <summary>
    /// This configuration class contains the configuration information for a shard set.
    /// </summary>
    /// <typeparam name="TShard"></typeparam>
    public class PgShardSets<TShard> : ArgentSea.ShardDataStores<TShard, PgShardConnectionOptions<TShard>> where TShard : IComparable
	{
		public PgShardSets(
			IOptions<PgShardConnectionOptions<TShard>> configOptions,
			IOptions<DataSecurityOptions> securityOptions,
			IOptions<DataResilienceOptions> resilienceStrategiesOptions,
			ILogger<PgShardSets<TShard>> logger
			) : base(configOptions, securityOptions, resilienceStrategiesOptions, new DataProviderServiceFactory(), logger)
		{

		}
	}
}
