// © John Hicks. All rights reserved. Licensed under the MIT license.
// See the LICENSE file in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Text;
using ArgentSea;

namespace ArgentSea.Pg
{
    /// <summary>
    /// This configuration class defines an array of database <see cref="PgConnectionConfiguration">connection configurations</see>. 
    /// <example>
    /// For example, you might configure your appsettings.json like this:
    /// <code>
    /// "PgDbConnections": [
    ///   {
    ///     "DatabaseKey": "MyDatabase",
    ///       "DataConnection": {
    ///         "UserName": "webUser",
    ///         "Password": "pwd1234",
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
	public class PgDbConnectionConfiguration : PgConnectionPropertiesBase, IDatabaseConnectionConfiguration
    {
		public string DatabaseKey { get; set; }

        public IDataConnection ReadConnectionInternal { get => ReadConnection; }
        public IDataConnection WriteConnectionInternal { get => WriteConnection; }
        public PgConnectionConfiguration ReadConnection { get; set; } = new PgConnectionConfiguration();
        public PgConnectionConfiguration WriteConnection { get; set; } = new PgConnectionConfiguration();
    }
}
