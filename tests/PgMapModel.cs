using System;
using System.Collections.Generic;
using System.Text;
using ArgentSea.Pg;

namespace ArgentSea.Pg.Test
{
    class PgMapModel
    {
        /*
            [MapToPgArray]
            */
        [MapToPgInteger("ArgentSeaTestDataId")]
        public int ArgentSeaTestDataId { get; set; }

        [MapToPgVarchar("Name", 255)]
        public string Name { get; set; }

        [MapToPgChar("Iso3166", 2)]
        public string Iso3166 { get; set; }

        [MapToPgBigint("BigCount")]
        public long? BigCount { get; set; }

        [MapToPgInteger("ValueCount")]
        public int? ValueCount { get; set; }

        [MapToPgSmallint("SmallCount")]
        public short? SmallCount { get; set; }

        [MapToPgInternalChar("ByteValue")]
        public byte? ByteCount { get; set; }

        [MapToPgBoolean("TrueFalse")]
        public bool? TrueFalse { get; set; }

        [MapToPgUuid("GuidValue")]
        public Guid GuidValue { get; set; }

        [MapToPgUuid("GuidNull")]
        public Guid? GuidNull { get; set; }

        [MapToPgDate("Birthday")]
        public DateTime? Birthday { get; set; }

        [MapToPgTimestamp("RightNow")]
        public DateTime? RightNow { get; set; }

        [MapToPgTimeTz("NowHere")]
        public DateTimeOffset? NowHere { get; set; }

        [MapToPgTimestampTz("NowElsewhere")]
        public DateTimeOffset? NowElsewhere { get; set; }

        [MapToPgTime("WakeUp")]
        public TimeSpan? WakeUp { get; set; }

        [MapToPgNumeric("Latitude", 9, 6)]
        public decimal? Latitude { get; set; }

        [MapToPgDouble("Longitude")]
        public double Longitude { get; set; }

        [MapToPgReal("Altitude")]
        public float Altitude { get; set; }

        [MapToPgDouble("Ratio")]
        public double? Ratio { get; set; }

        [MapToPgReal("Temperature")]
        public float? Temperature { get; set; }

        [MapToPgText("LongStory")]
        public string LongStory { get; set; }

        [MapToPgArray("Numbers", NpgsqlTypes.NpgsqlDbType.Integer)]
        public int[] Numbers { get; set; }

        [MapToPgBytea("MissingBits", 12)]
        public byte[] MissingBits { get; set; }

        //[MapToPgVarBinary("Blob", -1)]
        //public byte[] Blob { get; set; }

        [MapToPgMoney("Price")]
        public decimal? Price { get; set; }

        [MapToPgVarchar("EnvironmentTarget", 7)]
        public System.EnvironmentVariableTarget EnvTarget { get; set; }

        [MapToPgSmallint("ConsoleColor")]
        public ConsoleColor Color { get; set; }

        [MapToPgVarchar("ConsoleModifiers", 7)]
        public ConsoleModifiers Modifier { get; set; }

        //[MapToPgTinyInt("DayOfTheWeek")]
        //public DayOfWeek? DayOfTheWeek { get; set; }

        [MapToPgHstore("KeyValues")]
        public IDictionary<string, string> KeyValues { get; set; }

        [MapToPgInterval("CleanOut")]
        public TimeSpan CleanOutStuff { get; set; }

        [MapToPgChar("GCNotificationStatus", 16)]
        public GCNotificationStatus? GarbageCollectorNotificationStatus { get; set; }

        [MapPgShardKey("DataShard", 'x', "DataRecordId")]
        [MapToPgInteger("DataRecordId")]
        public ShardKey<int>? RecordKey { get; set; } = ShardKey<int>.Empty;

        [MapPgShardKey("ChildShard", 'y', "ParentRecordId", "ChildRecordId")]
        [MapToPgInteger("ParentRecordId")]
        [MapToPgSmallint("ChildRecordId")]
        public ShardKey<int, short> RecordChild { get; set; } = ShardKey<int, short>.Empty;


        [MapPgShardKey('A', "DataRecordId2")]
        [MapToPgBigint("DataRecordId2")]
        public ShardKey<long> DataShard2 { get; set; } = new ShardKey<long>('A', 123, 54321L);

        [MapPgShardKey("ChildShard2", 'B', "ParentRecord2Id", "ChildRecord2Id")]
         [MapToPgInteger("ParentRecord2Id")]
        [MapToPgVarchar("ChildRecord2Id", 255)]
        public ShardKey<int, string>? ChildShard2 { get; set; } = null;



    }
}

