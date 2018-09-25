using System;
using System.Collections.Generic;
using System.Text;
using ArgentSea;

namespace ArgentSea.Pg
{
    /// <summary>
    /// This configuration class defines an array of database <see cref="SqlConnectionConfiguration">connection configurations</see>. 
    /// <example>
    /// For example, you might configure your appsettings.json like this:
    /// <code>
    /// "PgDbConnections": [
    ///   {
    ///     "DatabaseKey": "MyDatabase",
    ///     "SecurityKey": "SecKey1",
    ///       "DataConnection": {
    ///         "Host": "localhost",
    ///         "Database": "MyDb"
    ///     }
    ///   }
    /// ]
    ///</code>
    /// Note that the SecurityKey must match a defined key in the DataSecurityOptions; likewise, a ResilienceKey (if defined) must match a key in the DataResilienceOptions array.
    ///</example>
    /// </summary>
	public class PgDbConnectionOptions : IDatabaseConfigurationOptions
	{
		public PgDbConnectionConfiguration[] PgDbConnections { get; set; }

        public IDatabaseConnectionConfiguration[] DbConnectionsInternal { get => PgDbConnections; }

	}
	public class PgDbConnectionConfiguration : IDatabaseConnectionConfiguration
	{
		public string DatabaseKey { get; set; }

		public IConnectionConfiguration DataConnectionInternal { get => DataConnection; }

        public PgConnectionConfiguration DataConnection { get; set; }
	}
}
