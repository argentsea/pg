using System;
using System.Threading;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ArgentSea.Pg
{
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
