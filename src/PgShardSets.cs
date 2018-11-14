// © John Hicks. All rights reserved. Licensed under the MIT license.
// See the LICENSE file in the repository root for more information.

using System;
using System.Threading;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ArgentSea.Pg
{
    /// <summary>
    /// A collection of ShardSets.
    /// </summary>
    /// <typeparam name="TShard">The type of the shardId index value.</typeparam>
    public class PgShardSets<TShard> : ArgentSea.ShardSetsBase<TShard, PgShardConnectionOptions<TShard>> where TShard : IComparable
	{
		public PgShardSets(
			IOptions<PgShardConnectionOptions<TShard>> configOptions,
            IOptions<PgGlobalPropertiesOptions> globalOptions,
			ILogger<PgShardSets<TShard>> logger
			) : base(configOptions, new DataProviderServiceFactory(), globalOptions?.Value, logger)
		{

		}
	}
}
