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
    public class PgShardSets : ArgentSea.ShardSetsBase<PgShardConnectionOptions>
	{
		public PgShardSets(
			IOptions<PgShardConnectionOptions> configOptions,
            IOptions<PgGlobalPropertiesOptions> globalOptions,
			ILogger<PgShardSets> logger
			) : base(configOptions, new DataProviderServiceFactory(), globalOptions?.Value, logger)
		{

		}
	}
}
