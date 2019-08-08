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
	public class PgShardConnectionOptions : IShardSetsConfigurationOptions
    {
        public IShardSetConnectionsConfiguration[] ShardSetsConfigInternal { get => PgShardSets; }
		public PgShardConnectionsConfiguration[] PgShardSets { get; set; }

        public class PgShardConnectionsConfiguration : PgConnectionPropertiesBase, IShardSetConnectionsConfiguration
        {
            public string ShardSetName { get; set; }
            public short DefaultShardId { get; set; }
            public IShardConnectionConfiguration[] ShardsConfigInternal { get => Shards; }
			public PgShardConnectionConfiguration[] Shards { get; set; }

            public IShardConnectionConfiguration ReadConfigInternal => Read;
            public IShardConnectionConfiguration WriteConfigInternal => Write;
            public PgShardConnectionConfiguration Read { get; set; }
            public PgShardConnectionConfiguration Write { get; set; }
        }

        public class PgShardConnectionConfiguration : PgConnectionPropertiesBase, IShardConnectionConfiguration
		{
			public short ShardId { get; set; }
			public IDataConnection ReadConnectionInternal { get => ReadConnection; }
			public IDataConnection WriteConnectionInternal { get => WriteConnection; }
            public PgConnectionConfiguration ReadConnection { get; set; } = new PgConnectionConfiguration();
			public PgConnectionConfiguration WriteConnection { get; set; } = new PgConnectionConfiguration();
        }

	}
}
