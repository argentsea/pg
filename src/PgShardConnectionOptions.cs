// © John Hicks. All rights reserved. Licensed under the MIT license.
// See the LICENSE file in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Text;

namespace ArgentSea.Pg
{
    /// <summary>
    /// This options class contains the shard dataset configuration information.
    /// </summary>
    /// <typeparam name="TShard"></typeparam>
	public class PgShardConnectionOptions<TShard> : IShardSetsConfigurationOptions<TShard>
            where TShard : IComparable
    {
        public IShardSetConnectionsConfiguration<TShard>[] ShardSetsInternal { get => PgShardSets; }
		public PgShardConnectionsConfiguration[] PgShardSets { get; set; }

		public class PgShardConnectionsConfiguration : PgConnectionPropertiesBase, IShardSetConnectionsConfiguration<TShard>
        {
            public string ShardSetName { get; set; }
			public IShardConnectionConfiguration<TShard>[] ShardsInternal { get => Shards; }
			public PgShardConnectionConfiguration[] Shards { get; set; }
		}

		public class PgShardConnectionConfiguration : PgConnectionPropertiesBase, IShardConnectionConfiguration<TShard>
		{
			public TShard ShardId { get; set; }
			public IDataConnection ReadConnectionInternal { get => ReadConnection; }
			public IDataConnection WriteConnectionInternal { get => WriteConnection; }
            public PgConnectionConfiguration ReadConnection { get; set; } = new PgConnectionConfiguration();
			public PgConnectionConfiguration WriteConnection { get; set; } = new PgConnectionConfiguration();
        }

	}
}
