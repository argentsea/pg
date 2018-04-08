using System;
using System.Collections.Generic;
using System.Text;
using ArgentSea;

namespace ArgentSea.Pg
{
	public class PgDbConnectionOptions : IDbDataConfigurationOptions
	{
		public PgDbConnectionConfiguration[] PgDbConnections { get; set; }
		public IDbConnectionConfiguration[] DbConnectionsInternal { get => PgDbConnections; }

	}
	public class PgDbConnectionConfiguration : IDbConnectionConfiguration
	{
		public string DatabaseKey { get; set; }
		public string SecurityKey { get; set; }
		public string DataResilienceKey { get; set; }

		public IConnectionConfiguration DataConnectionInternal { get => DataConnection; }
		public PgConnectionConfiguration DataConnection { get; set; }
	}
}
