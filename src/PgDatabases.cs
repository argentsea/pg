// © John Hicks. All rights reserved. Licensed under the MIT license.
// See the LICENSE file in the repository root for more information.

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ArgentSea.Pg
{
    /// <summary>
    /// This class manages the non-sharded SQL database connections.
    /// </summary>
    public class PgDatabases : DatabasesBase<PgDbConnectionOptions>
	{
		public PgDatabases(
			IOptions<PgDbConnectionOptions> configOptions,
			IOptions<DataSecurityOptions> securityOptions,
			IOptions<DataResilienceOptions> resilienceStrategiesOptions,
			ILogger<PgDatabases> logger
			) : base(configOptions, securityOptions, resilienceStrategiesOptions, (IDataProviderServiceFactory)new DataProviderServiceFactory(), logger)
		{

		}
	}
}
