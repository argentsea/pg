using System;
using Microsoft.Extensions.Configuration;
using ArgentSea;
using Npgsql;
using Microsoft.Extensions.Options;
using ArgentSea.Pg;

namespace Microsoft.Extensions.DependencyInjection
{
    //public static class PgDataServiceBuilderExtensions
    //{
    //    public static IServiceCollection AddPgDataConfiguration<TShard>(
    //        this IServiceCollection services,
    //        IConfiguration config,
    //        string securityOptionName, 
    //        string dataOptionName
    //        ) where TShard : IComparable
    //    {

    //        services.Configure<DataSecurityOptions>(securityOptionName, config.GetSection("DataSecurity"));
    //        services.Configure<PgDataConfigurationsOptions<TShard>>(dataOptionName, config.GetSection("PgDataSources"));
    //        services.AddSingleton<IDataProviderServices>(new DataProviderServices());
    //        services.AddSingleton<DataStores<TShard>, DataStores<TShard>>();
    //        return services;
    //    }
    //    public static IServiceCollection AddPgDataConfiguration<TShard>(
    //        this IServiceCollection services,
    //        IConfiguration config
    //        ) where TShard : IComparable
    //    {
    //        return AddPgDataConfiguration<TShard>(services, config, Options.Options.DefaultName, Options.Options.DefaultName);
    //    }
    //}
}
