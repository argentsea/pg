using Xunit;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel.Resolution;
using ArgentSea;
using ArgentSea.Pg;
using FluentAssertions;

namespace ArgentSea.Pg.Test
{
    public class ConfigurationTests
    {
        [Fact]
        public void TestConfigurationOptions()
        {
            var bldr = new ConfigurationBuilder()
                .AddJsonFile("configurationsettings.json");

            var config = bldr.Build();
            var services = new ServiceCollection();
            services.AddOptions();
            //services.AddSqlDbConfiguration(config);
            services.Configure<DataResilienceOptions>(config);
            services.Configure<DataSecurityOptions>(config);
            services.Configure<PgDbConnectionOptions>(config);
            services.Configure<PgShardConnectionOptions<int>>(config);

            var serviceProvider = services.BuildServiceProvider();

            var resilienceOptions = serviceProvider.GetService<IOptions<DataResilienceOptions>>();
            var securityOptions = serviceProvider.GetService<IOptions<DataSecurityOptions>>();
            var sqlDbOptions = serviceProvider.GetService<IOptions<PgDbConnectionOptions>>();
            var sqlShardOptions = serviceProvider.GetService<IOptions<PgShardConnectionOptions<int>>>();

            var securityData = securityOptions.Value;
            securityData.Credentials[0].SecurityKey.Should().Be("One", "that is the first value set in the configurationsettings.json configuration file");
            securityData.Credentials[1].SecurityKey.Should().Be("Two", "that is the second value set in the configurationsettings.json configuration file");

            var resilienceData = resilienceOptions.Value;
            resilienceData.DataResilienceStrategies.Length.Should().Be(2, "there are two strategies defined in configuration file");
            resilienceData.DataResilienceStrategies[0].ResilienceKey.Should().Be("local", "that is the first key in the configuration file");
            resilienceData.DataResilienceStrategies[1].ResilienceKey.Should().Be("remote", "that is the second key in the configuration file");

            var sqlDbData = sqlDbOptions.Value;
            sqlDbData.PgDbConnections.Length.Should().Be(2, "two conections are defined in the configuration file.");

            sqlDbData.PgDbConnections.Length.Should().Be(2, "two Db conections are defined in the configuration file.");
            sqlDbData.PgDbConnections[0].DataConnection.GetConnectionString().Should().Contain("10.10.1.22", "the servername should be in the connection string.");
            sqlDbData.PgDbConnections[1].DataConnection.GetConnectionString().Should().Contain("MyDB", "the database name should be in the connection string.");

            var sqlShardData = sqlShardOptions.Value;
            sqlShardData.PgShardSets.Length.Should().Be(2, "there are two shard sets defined");
        }
        [Fact]
        public void TestServiceBuilder()
        {
            var bldr = new ConfigurationBuilder()
                .AddJsonFile("configurationsettings.json");

            var config = bldr.Build();
            var services = new ServiceCollection();
            services.AddOptions();

            services.AddLogging();
            services.AddPgServices<short>(config);

            var serviceProvider = services.BuildServiceProvider();
            var resilienceOptions = serviceProvider.GetService<IOptions<DataResilienceOptions>>();
            var securityOptions = serviceProvider.GetService<IOptions<DataSecurityOptions>>();
            var sqlDbOptions = serviceProvider.GetService<IOptions<PgDbConnectionOptions>>();
            var sqlShardOptions = serviceProvider.GetService<IOptions<PgShardConnectionOptions<short>>>();
            var dbLogger = NSubstitute.Substitute.For<Microsoft.Extensions.Logging.ILogger<PgDatabases>>();

            var dbService = new PgDatabases(sqlDbOptions, securityOptions, resilienceOptions, dbLogger);
            dbService.Count.Should().Be(2, "two connections are defined in the configuration file");
            dbService["MainDb"].ConnectionString.Should().Contain("webUser", "the configuration file specifies this credential key user");
            dbService["MainDb"].ConnectionString.Should().Contain("user1234", "the configuration file specifies this credential key password");
            dbService["OtherDb"].ConnectionString.Should().Contain("Integrated Security=True", "the configuration file specifies this credential key");

            var shardLogger = NSubstitute.Substitute.For<Microsoft.Extensions.Logging.ILogger<ArgentSea.Pg.PgShardSets<short>>>();

            var shardService = new ArgentSea.Pg.PgShardSets<short>(sqlShardOptions, securityOptions, resilienceOptions, shardLogger);

            shardService.Count.Should().Be(2, "two shard sets are defined in the configuration file");
            shardService["Set1"].Count.Should().Be(2, "the configuration file has two shard connections defined on shard set Set1");
            shardService["Set2"].Count.Should().Be(2, "the configuration file has two shard connections defined on shard set Set2");
            shardService["Set1"][0].Read.ConnectionString.Should().Contain("webUser", "the configuration file specifies this credential key user");
            shardService["Set1"][0].Read.ConnectionString.Should().Contain("user1234", "the configuration file specifies this credential key password");
            shardService["Set2"][0].Read.ConnectionString.Should().Contain("Integrated Security=True", "the configuration file specifies this credential key");
        }
    }
}
