using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ArgentSea.Pg
{
    public class PgDatabases : DbDataStores<PgDbConnectionOptions>
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
