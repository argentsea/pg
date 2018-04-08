using System;
using System.Collections.Generic;
using System.Text;

namespace ArgentSea.Pg
{
	public class PgShardConnectionOptions<TShard> : IShardDataConfigurationOptions<TShard>
	{
		public IShardConnectionsConfiguration<TShard>[] ShardSetsInternal { get => PgShardSets; }
		public PgShardConnectionsConfiguration[] PgShardSets { get; set; }

		public class PgShardConnectionsConfiguration : IShardConnectionsConfiguration<TShard>
		{
			public string ShardSetKey { get; set; }
			public string SecurityKey { get; set; }
			public string DataResilienceKey { get; set; }
			public IShardConnectionConfiguration<TShard>[] ShardsInternal { get => Shards; }
			public PgShardConnectionConfiguration[] Shards { get; set; }
		}

		public class PgShardConnectionConfiguration : IShardConnectionConfiguration<TShard>
		{
			public TShard ShardNumber { get; set; }
			public IConnectionConfiguration ReadConnectionInternal { get => ReadConnection; }
			public IConnectionConfiguration WriteConnectionInternal { get => WriteConnection; }
			public PgConnectionConfiguration ReadConnection { get; set; }
			public PgConnectionConfiguration WriteConnection { get; set; }
		}

	}
}
