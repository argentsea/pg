// © John Hicks. All rights reserved. Licensed under the MIT license.
// See the LICENSE file in the repository root for more information.

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ArgentSea.Pg
{
    /// <summary>
    /// This class manages the non-sharded PosgreSQL database connections.
    /// </summary>
    public class PgDatabases : DatabasesBase<PgDbConnectionOptions>
	{
		public PgDatabases(
			IOptions<PgDbConnectionOptions> configOptions,
			IOptions<PgGlobalPropertiesOptions> globalOptions,
			ILogger<PgDatabases> logger
			) : base(configOptions, (IDataProviderServiceFactory)new DataProviderServiceFactory(), globalOptions?.Value, logger)
		{

		}
	}
}
