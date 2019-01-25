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

            services.Configure<PgGlobalPropertiesOptions>(config.GetSection("PgGlobalSettings"));
            services.Configure<PgDbConnectionOptions>(config);
            services.Configure<PgShardConnectionOptions<int>>(config);

            var serviceProvider = services.BuildServiceProvider();

            var globalOptions = serviceProvider.GetService<IOptions<PgGlobalPropertiesOptions>>();
            var sqlDbOptions = serviceProvider.GetService<IOptions<PgDbConnectionOptions>>();
            var sqlShardOptions = serviceProvider.GetService<IOptions<PgShardConnectionOptions<int>>>();

            var globalData = globalOptions.Value;
            globalData.RetryCount.Should().Be(15, "that is the first value set in the configurationsettings.json configuration file");

            var pgDbData = sqlDbOptions.Value;
            pgDbData.PgDbConnections.Length.Should().Be(2, "two conections are defined in the configuration file.");
            pgDbData.PgDbConnections[0].ReadConnectionInternal.SetAmbientConfiguration(globalData, null, null, pgDbData.PgDbConnections[0]);
            pgDbData.PgDbConnections[0].WriteConnectionInternal.SetAmbientConfiguration(globalData, null, null, pgDbData.PgDbConnections[0]);
            pgDbData.PgDbConnections[1].ReadConnectionInternal.SetAmbientConfiguration(globalData, null, null, pgDbData.PgDbConnections[1]);
            pgDbData.PgDbConnections[1].WriteConnectionInternal.SetAmbientConfiguration(globalData, null, null, pgDbData.PgDbConnections[1]);

            pgDbData.PgDbConnections[0].ReadConnection.GetConnectionString(null).Should().Be("Application Name=MyApp;Use Perf Counters=False;Database=MainDb;Host=10.10.25.1", "this is the value inherited from global configuration settings.");
            pgDbData.PgDbConnections[0].WriteConnection.GetConnectionString(null).Should().Be("Application Name=MyApp;Use Perf Counters=False;Database=MainDb;Host=10.10.25.1", "this is the value inherited from global configuration settings.");
            pgDbData.PgDbConnections[1].ReadConnection.GetConnectionString(null).Should().Be("Application Name=MyOtherApp;Use Perf Counters=False;Database=OtherDb;Host=10.10.25.2", "this is the value that overrides the global setting");
            pgDbData.PgDbConnections[1].WriteConnection.GetConnectionString(null).Should().Be("Application Name=MyOtherApp;Use Perf Counters=False;Database=OtherDb;Host=10.10.25.2", "this is the value that overrides the global setting");

            var pgShardData = sqlShardOptions.Value;
            pgShardData.PgShardSets.Length.Should().Be(2, "there are two shard sets defined");
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
            var globalOptions = serviceProvider.GetService<IOptions<PgGlobalPropertiesOptions>>();
            var pgDbOptions = serviceProvider.GetService<IOptions<PgDbConnectionOptions>>();
            var pgShardOptions = serviceProvider.GetService<IOptions<PgShardConnectionOptions<short>>>();
            var dbLogger = NSubstitute.Substitute.For<Microsoft.Extensions.Logging.ILogger<PgDatabases>>();

            var dbService = new PgDatabases(pgDbOptions, globalOptions, dbLogger);
            dbService.Count.Should().Be(2, "two connections are defined in the configuration file");
            dbService["MainDb"].Read.ConnectionString.Should().Be("Application Name=MyApp;Use Perf Counters=False;Database=MainDb;Host=10.10.25.1", "this is the value inherited from global configuratoin settings.");
            dbService["OtherDb"].Read.ConnectionString.Should().Be("Application Name=MyOtherApp;Use Perf Counters=False;Database=OtherDb;Host=10.10.25.2", "this is the value that overrides the global setting");
            var shardLogger = NSubstitute.Substitute.For<Microsoft.Extensions.Logging.ILogger<ArgentSea.Pg.PgShardSets<short>>>();

            var shardService = new ArgentSea.Pg.PgShardSets<short>(pgShardOptions, globalOptions, shardLogger);

            shardService.Count.Should().Be(2, "two shard sets are defined in the configuration file");
            shardService["Inherit"].Count.Should().Be(2, "the configuration file has two shard connections defined on shard set Set1");
            shardService["Explicit"].Count.Should().Be(2, "the configuration file has two shard connections defined on shard set Set2");

            shardService["Inherit"][0].Read.ConnectionString.Should().Be("Application Name=Inheritance;Use Perf Counters=False;Host=10.10.25.3", "the configuration file builds this connection string");
            shardService["Inherit"][0].Write.ConnectionString.Should().Be("Application Name=Inheritance;Use Perf Counters=False;Host=10.10.25.3", "the configuration file builds this connection string");
            shardService["Inherit"][1].Read.ConnectionString.Should().Be("Application Name=Inheritance;Use Perf Counters=False;Host=10.10.25.4", "the configuration file builds this connection string");
            shardService["Inherit"][1].Write.ConnectionString.Should().Be("Application Name=Inheritance;Use Perf Counters=False;Host=10.10.1.5", "the configuration file builds this connection string");
            shardService["Explicit"][0].Read.ConnectionString.Should().Be("Application Name=MyWebApp;Use Perf Counters=True;Password=pwd2345;Username=user;Integrated Security=True;Auto Prepare Min Usages=6;Check Certificate Revocation=True;Client Encoding=UTF16;Command Timeout=301;Connection Pruning Interval=11;Convert Infinity DateTime=True;Database=MyDb1;Encoding=UTF16;Enlist=False;Host=10.10.25.6;Include Realm=False;Internal Command Timeout=100;Keepalive=100;Kerberos Service Name=test;Load Table Composites=True;Max Auto Prepare=1;Maximum Pool Size=99;Minimum Pool Size=2;No Reset On Close=True;Persist Security Info=False;Pooling=False;Port=5433;Read Buffer Size=4096;Search Path=;Server Compatibility Mode=None;Socket Receive Buffer Size=4096;Socket Send Buffer Size=4096;SSL Mode=Disable;TCP Keepalive=False;Timeout=16;Timezone=America/Chicago;Trust Server Certificate=False;Use SSL Stream=True;Write Buffer Size=4096", "the configuration file builds this connection string");
            shardService["Explicit"][0].Write.ConnectionString.Should().Be(shardService["Explicit"][0].Read.ConnectionString, "the read and write connections should be the same");
            shardService["Explicit"][1].Read.ConnectionString.Should().Be("Application Name=MyWebApp3;Use Perf Counters=True;Password=pwd2345;Username=user1;Integrated Security=True;Auto Prepare Min Usages=6;Check Certificate Revocation=True;Client Encoding=UTF16;Command Timeout=301;Connection Pruning Interval=11;Convert Infinity DateTime=True;Database=MyDb1;Encoding=UTF16;Enlist=False;Host=10.10.25.6;Include Realm=False;Internal Command Timeout=100;Keepalive=30;Kerberos Service Name=test;Load Table Composites=True;Max Auto Prepare=1;Maximum Pool Size=99;Minimum Pool Size=2;No Reset On Close=True;Persist Security Info=False;Pooling=False;Port=5433;Read Buffer Size=4096;Search Path=;Server Compatibility Mode=NoTypeLoading;Socket Receive Buffer Size=4096;Socket Send Buffer Size=4096;SSL Mode=Disable;TCP Keepalive=False;Timeout=16;Timezone=America/Chicago;Trust Server Certificate=False;Use SSL Stream=True;Write Buffer Size=4096", "the configuration file builds this connection string");
            shardService["Explicit"][1].Write.ConnectionString.Should().Be("Application Name=MyWebApp5;Use Perf Counters=False;Password=pwd3456;Username=user2;Integrated Security=False;Auto Prepare Min Usages=7;Check Certificate Revocation=False;Client Encoding=UTF8;Command Timeout=302;Connection Pruning Interval=12;Convert Infinity DateTime=False;Database=MyDb3;Encoding=UTF8;Enlist=True;Host=10.10.25.7;Include Realm=True;Internal Command Timeout=101;Keepalive=20;Kerberos Service Name=test2;Load Table Composites=False;Max Auto Prepare=2;Maximum Pool Size=98;Minimum Pool Size=3;No Reset On Close=False;Persist Security Info=True;Pooling=True;Port=5434;Read Buffer Size=8192;Search Path=;Server Compatibility Mode=Redshift;Socket Receive Buffer Size=8192;Socket Send Buffer Size=8192;SSL Mode=Prefer;TCP Keepalive=True;Timeout=17;Timezone=America/Los_Angeles;Trust Server Certificate=True;Use SSL Stream=False;Write Buffer Size=8192", "the configuration file builds this connection string");
        }
    }
}
