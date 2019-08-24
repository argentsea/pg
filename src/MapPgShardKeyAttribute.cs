using System;
using System.Collections.Generic;
using System.Text;
using ArgentSea;

namespace ArgentSea.Pg
{
    public class MapPgShardKeyAttribute : MapShardKeyAttribute
    {
        public MapPgShardKeyAttribute(char origin, string recordIdName)
            : base(null, origin, recordIdName, null, null, null) { }

        public MapPgShardKeyAttribute(string shardIdName, char origin, string recordIdName)
            : base(new MapToPgSmallintAttribute(shardIdName, false), origin, recordIdName, null, null, null) { }

        public MapPgShardKeyAttribute(char origin, string recordIdName, string childIdName)
            : base(null, origin, recordIdName, childIdName, null, null) { }

        public MapPgShardKeyAttribute(string shardIdName, char origin, string recordIdName, string childIdName)
            : base(new MapToPgSmallintAttribute(shardIdName, false), origin, recordIdName, childIdName, null, null) { }

        public MapPgShardKeyAttribute(char origin, string recordIdName, string childIdName, string grandChildIdName)
            : base(null, origin, recordIdName, childIdName, grandChildIdName, null) { }

        public MapPgShardKeyAttribute(string shardIdName, char origin, string recordIdName, string childIdName, string grandChildIdName)
            : base(new MapToPgSmallintAttribute(shardIdName, false), origin, recordIdName, childIdName, grandChildIdName, null) { }

        public MapPgShardKeyAttribute(char origin, string recordIdName, string childIdName, string grandChildIdName, string greatGrandChildIdName)
            : base(null, origin, recordIdName, childIdName, grandChildIdName, greatGrandChildIdName) { }

        public MapPgShardKeyAttribute(string shardIdName, char origin, string recordIdName, string childIdName, string grandChildIdName, string greatGrandChildIdName)
            : base(new MapToPgSmallintAttribute(shardIdName, false), origin, recordIdName, childIdName, grandChildIdName, greatGrandChildIdName) { }

    }
}
