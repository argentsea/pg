// © John Hicks. All rights reserved. Licensed under the MIT license.
// See the LICENSE file in the repository root for more information.

using System;
using System.Threading;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ArgentSea.Pg
{
    /// <summary>
    /// A collection of ShardSets (with a UInt16 shardId type).
    /// </summary>
    public class PgShardSets : ArgentSea.ShardSetsBase<short, PgShardConnectionOptions<short>>
    {
        public PgShardSets(
            IOptions<PgShardConnectionOptions<short>> configOptions,
            IOptions<DataSecurityOptions> securityOptions,
            IOptions<DataResilienceOptions> resilienceStrategiesOptions,
            ILogger<PgShardSets<short>> logger
            ) : base(configOptions, securityOptions, resilienceStrategiesOptions, new DataProviderServiceFactory(), logger)
        {

        }
    }

    /// <summary>
    /// A collection of ShardSets.
    /// </summary>
    /// <typeparam name="TShard">The type of the shardId index value.</typeparam>
    public class PgShardSets<TShard> : ArgentSea.ShardSetsBase<TShard, PgShardConnectionOptions<TShard>> where TShard : IComparable
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
