using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Data.SqlClient;
using FluentAssertions;
using NSubstitute;
using Npgsql;
using Microsoft.Extensions.Logging;

namespace ArgentSea.Pg.Test
{
    public class MappingTests
    {

        [Fact]
        public void ValidateInParameterMapper()
        {

            var smv = new PgMapModel()
            {
                ArgentSeaTestDataId = 1,
                Name = "Test2",
                Iso3166 = "US",
                BigCount = 4,
                ValueCount = 5,
                SmallCount = 6,
                ByteCount = 7,
                TrueFalse = true,
                GuidValue = Guid.NewGuid(),
                GuidNull = Guid.NewGuid(),
                Birthday = new DateTime(2008, 8, 8),
                RightNow = new DateTime(2009, 9, 9),
              NowHere = new DateTime(1900, 1, 1, 13, 15, 0),
                NowElsewhere = new DateTimeOffset(2011, 11, 11, 11, 11, 11, new TimeSpan(11, 11, 00)),
                WakeUp = new TimeSpan(12, 12, 12),
                Latitude = 13.13m,
                Longitude = 14.14,
                Altitude = 15.15f,
                Ratio = 16.1,
                Temperature = 32.1f,
                LongStory = "16",
                Numbers = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 },
                MissingBits = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 19 },
                Price = 21.0m,
                EnvTarget = EnvironmentVariableTarget.User,
                Color = ConsoleColor.Blue,
                Modifier = ConsoleModifiers.Control,
              KeyValues = new Dictionary<string, string>() { { "one", "1" }, { "two", "2" } },
              CleanOutStuff = new TimeSpan(48, 0, 0),
                GarbageCollectorNotificationStatus = GCNotificationStatus.NotApplicable,
                RecordKey = new ShardKey<int>('x', (short)2, 1234),
                RecordChild = new ShardKey<int, short>('y', (short)3, 4567, (short)-23456),
                DataShard2 = new ShardKey<long>('z', (short)22, 123432L),
                ChildShard2 = new ShardKey<int, string>('!', (short)255, 255, "testing123")
            };
            var dbLogger = Substitute.For<Microsoft.Extensions.Logging.ILogger>();
            var prms = new ParameterCollection();

            prms.CreateInputParameters<PgMapModel>(smv, dbLogger);

            Assert.True(((NpgsqlParameter)prms["ArgentSeaTestDataId"]).NpgsqlDbType == NpgsqlTypes.NpgsqlDbType.Integer);
            ((NpgsqlParameter)prms["ArgentSeaTestDataId"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Integer, "that is the correct data type");
            ((NpgsqlParameter)prms["ArgentSeaTestDataId"]).Value.Should().Be(smv.ArgentSeaTestDataId, "that is the assigned value");
            ((NpgsqlParameter)prms["Name"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Varchar, "that is the correct data type");
            ((NpgsqlParameter)prms["Name"]).Value.Should().Be(smv.Name, "that is the assigned value");
            ((NpgsqlParameter)prms["Name"]).Size.Should().Be(255, "that is the max length");
            ((NpgsqlParameter)prms["Iso3166"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Char, "that is the correct data type");
            ((NpgsqlParameter)prms["Iso3166"]).Value.Should().Be(smv.Iso3166, "that is the assigned value");
            ((NpgsqlParameter)prms["Iso3166"]).Size.Should().Be(2, "that is the fixed length");
            ((NpgsqlParameter)prms["BigCount"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Bigint, "that is the correct data type");
            ((NpgsqlParameter)prms["BigCount"]).Value.Should().Be(smv.BigCount, "that is the assigned value");
            ((NpgsqlParameter)prms["ValueCount"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Integer, "that is the correct data type");
            ((NpgsqlParameter)prms["ValueCount"]).Value.Should().Be(smv.ValueCount, "that is the assigned value");
            ((NpgsqlParameter)prms["SmallCount"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Smallint, "that is the correct data type");
            ((NpgsqlParameter)prms["SmallCount"]).Value.Should().Be(smv.SmallCount, "that is the assigned value");
            ((NpgsqlParameter)prms["ByteValue"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.InternalChar, "that is the correct data type");
            ((NpgsqlParameter)prms["ByteValue"]).Value.Should().Be(smv.ByteCount, "that is the assigned value");
            ((NpgsqlParameter)prms["TrueFalse"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Boolean, "that is the correct data type");
            ((NpgsqlParameter)prms["TrueFalse"]).Value.Should().Be(smv.TrueFalse, "that is the assigned value");
            ((NpgsqlParameter)prms["GuidValue"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Uuid, "that is the correct data type");
            ((NpgsqlParameter)prms["GuidValue"]).Value.Should().Be(smv.GuidValue, "that is the assigned value");
            ((NpgsqlParameter)prms["GuidNull"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Uuid, "that is the correct data type");
            ((NpgsqlParameter)prms["GuidNull"]).Value.Should().Be(smv.GuidNull, "that is the assigned value");
            ((NpgsqlParameter)prms["Birthday"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Date, "that is the correct data type");
            ((NpgsqlParameter)prms["Birthday"]).Value.Should().Be(smv.Birthday, "that is the assigned value");
            ((NpgsqlParameter)prms["RightNow"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Timestamp, "that is the correct data type");
            ((NpgsqlParameter)prms["RightNow"]).Value.Should().Be(smv.RightNow, "that is the assigned value");
            ((NpgsqlParameter)prms["NowHere"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.TimeTz, "that is the correct data type");
            ((NpgsqlParameter)prms["NowHere"]).Value.Should().Be(smv.NowHere, "that is the assigned value");
            ((NpgsqlParameter)prms["NowElsewhere"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.TimestampTz, "that is the correct data type");
            ((NpgsqlParameter)prms["NowElsewhere"]).Value.Should().Be(smv.NowElsewhere, "that is the assigned value");
            ((NpgsqlParameter)prms["WakeUp"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Time, "that is the correct data type");
            ((NpgsqlParameter)prms["WakeUp"]).Value.Should().Be(smv.WakeUp, "that is the assigned value");
            ((NpgsqlParameter)prms["Latitude"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Numeric, "that is the correct data type");
            ((NpgsqlParameter)prms["Latitude"]).Value.Should().Be(smv.Latitude, "that is the assigned value");
            ((NpgsqlParameter)prms["Latitude"]).Precision.Should().Be(9, "that is the specified precision");
            ((NpgsqlParameter)prms["Latitude"]).Scale.Should().Be(6, "that is the specified scale");
            ((NpgsqlParameter)prms["Longitude"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Double, "that is the correct data type");
            ((NpgsqlParameter)prms["Longitude"]).Value.Should().Be(smv.Longitude, "that is the assigned value");
            ((NpgsqlParameter)prms["Altitude"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Real, "that is the correct data type");
            ((NpgsqlParameter)prms["Altitude"]).Value.Should().Be(smv.Altitude, "that is the assigned value");
            ((NpgsqlParameter)prms["Ratio"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Double, "that is the correct data type");
            ((NpgsqlParameter)prms["Ratio"]).Value.Should().Be(smv.Ratio, "that is the assigned value");
            ((NpgsqlParameter)prms["Temperature"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Real, "that is the correct data type");
            ((NpgsqlParameter)prms["Temperature"]).Value.Should().Be(smv.Temperature, "that is the assigned value");
            ((NpgsqlParameter)prms["LongStory"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Text, "that is the correct data type");
            ((NpgsqlParameter)prms["LongStory"]).Value.Should().Be(smv.LongStory, "that is the assigned value");
            ((NpgsqlParameter)prms["Numbers"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Array | NpgsqlTypes.NpgsqlDbType.Integer , "that is the correct data type");
            ((NpgsqlParameter)prms["Numbers"]).Value.Should().Be(smv.Numbers, "that is the assigned value");
            ((NpgsqlParameter)prms["MissingBits"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Bytea, "that is the correct data type");
            ((NpgsqlParameter)prms["MissingBits"]).Value.Should().Be(smv.MissingBits, "that is the assigned value");
            ((NpgsqlParameter)prms["MissingBits"]).Size.Should().Be(12, "that is the max length");
            ((NpgsqlParameter)prms["Price"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Money, "that is the correct data type");
            ((NpgsqlParameter)prms["Price"]).Value.Should().Be(smv.Price, "that is the assigned value");
            ((NpgsqlParameter)prms["EnvironmentTarget"]).Value.Should().Be(smv.EnvTarget.ToString(), "that is the assigned value");
            ((NpgsqlParameter)prms["EnvironmentTarget"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Varchar, "that is the correct data type");
            ((NpgsqlParameter)prms["ConsoleColor"]).Value.Should().Be((short)smv.Color, "that is the assigned value");
            ((NpgsqlParameter)prms["ConsoleColor"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Smallint, "that is the correct data type");
            ((NpgsqlParameter)prms["ConsoleModifiers"]).Value.Should().Be(ConsoleModifiers.Control.ToString(), "that is the assigned value");
            ((NpgsqlParameter)prms["ConsoleModifiers"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Varchar, "that is the correct data type");
            ((NpgsqlParameter)prms["KeyValues"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Hstore, "that is the correct data type");
            ((NpgsqlParameter)prms["KeyValues"]).Value.Should().Be(smv.KeyValues, "that is the assigned value");
            ((NpgsqlParameter)prms["CleanOut"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Interval, "that is the correct data type");
            ((NpgsqlParameter)prms["CleanOut"]).Value.Should().Be(smv.CleanOutStuff, "that is the assigned value");
            ((NpgsqlParameter)prms["GCNotificationStatus"]).Value.Should().Be(GCNotificationStatus.NotApplicable.ToString(), "that is the assigned value");
            ((NpgsqlParameter)prms["GCNotificationStatus"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Char, "that is the correct data type");
            ((NpgsqlParameter)prms["DataRecordId"]).Value.Should().Be(smv.RecordKey.Value.RecordId, "that is the assigned value");
            ((NpgsqlParameter)prms["ParentRecordId"]).Value.Should().Be(smv.RecordChild.RecordId, "that is the assigned value");
            ((NpgsqlParameter)prms["ChildRecordId"]).Value.Should().Be(smv.RecordChild.ChildId, "that is the assigned value");
            ((NpgsqlParameter)prms["ChildShard2"]).Value.Should().Be(smv.ChildShard2.Value.Key.ShardId, "that is the assigned value");
            ((NpgsqlParameter)prms["ParentRecord2Id"]).Value.Should().Be(smv.ChildShard2.Value.Key.RecordId, "that is the assigned value");
            ((NpgsqlParameter)prms["ChildRecord2Id"]).Value.Should().Be(smv.ChildShard2.Value.ChildId, "that is the assigned value");
        }
        [Fact]
        public void ValidateInParameterNullMapper()
        {
            var smv = new PgMapModel()
            {
                ArgentSeaTestDataId = 1,
                Name = null,
                Iso3166 = null,
                BigCount = null,
                ValueCount = null,
                SmallCount = null,
                ByteCount = null,
                TrueFalse = null,
                GuidValue = Guid.Empty,
                GuidNull = null,
                Birthday = null,
                RightNow = null,
                NowHere = null,
                NowElsewhere = null,
                WakeUp = null,
                Latitude = null,
                Longitude = double.NaN,
                Altitude = float.NaN,
                Temperature = null,
                Ratio = null,
                LongStory = null,
                MissingBits = null,
                Price = null,
                EnvTarget = EnvironmentVariableTarget.Machine,
                Color = ConsoleColor.DarkMagenta,
                Modifier = ConsoleModifiers.Shift,
                KeyValues = null,
                CleanOutStuff = TimeSpan.Zero,
                GarbageCollectorNotificationStatus = GCNotificationStatus.Succeeded,
                RecordKey = null,
                RecordChild = ShardKey<int, short>.Empty,
                DataShard2 = ShardKey<long>.Empty,
                ChildShard2 = null
            };
            var dbLogger = Substitute.For<Microsoft.Extensions.Logging.ILogger>();
            var prms = new ParameterCollection();

            prms.CreateInputParameters<PgMapModel>(smv, dbLogger);
            Assert.True(((NpgsqlParameter)prms["ArgentSeaTestDataId"]).NpgsqlDbType == NpgsqlTypes.NpgsqlDbType.Integer);
            ((NpgsqlParameter)prms["ArgentSeaTestDataId"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Integer, "that is the correct data type");
            ((NpgsqlParameter)prms["ArgentSeaTestDataId"]).Value.Should().Be(smv.ArgentSeaTestDataId, "that is the assigned value");
            ((NpgsqlParameter)prms["ArgentSeaTestDataId"]).Direction.Should().Be(System.Data.ParameterDirection.Input, "this should be an input parameter");
            ((NpgsqlParameter)prms["Name"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Varchar, "that is the correct data type");
            ((NpgsqlParameter)prms["Name"]).Value.Should().Be(System.DBNull.Value, "null should be DBNull");
            ((NpgsqlParameter)prms["Name"]).Size.Should().Be(255, "that is the max length");
            ((NpgsqlParameter)prms["Name"]).Direction.Should().Be(System.Data.ParameterDirection.Input, "this should be an input parameter");
            ((NpgsqlParameter)prms["Iso3166"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Char, "that is the correct data type");
            ((NpgsqlParameter)prms["Iso3166"]).Value.Should().Be(System.DBNull.Value, "null should be DBNull");
            ((NpgsqlParameter)prms["Iso3166"]).Size.Should().Be(2, "that is the fixed length");
            ((NpgsqlParameter)prms["Iso3166"]).Direction.Should().Be(System.Data.ParameterDirection.Input, "this should be an input parameter");
            ((NpgsqlParameter)prms["BigCount"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Bigint, "that is the correct data type");
            ((NpgsqlParameter)prms["BigCount"]).Value.Should().Be(System.DBNull.Value, "null should be DBNull");
            ((NpgsqlParameter)prms["BigCount"]).Direction.Should().Be(System.Data.ParameterDirection.Input, "this should be an input parameter");
            ((NpgsqlParameter)prms["ValueCount"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Integer, "that is the correct data type");
            ((NpgsqlParameter)prms["ValueCount"]).Value.Should().Be(System.DBNull.Value, "null should be DBNull");
            ((NpgsqlParameter)prms["ValueCount"]).Direction.Should().Be(System.Data.ParameterDirection.Input, "this should be an input parameter");
            ((NpgsqlParameter)prms["SmallCount"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Smallint, "that is the correct data type");
            ((NpgsqlParameter)prms["SmallCount"]).Value.Should().Be(System.DBNull.Value, "null should be DBNull");
            ((NpgsqlParameter)prms["SmallCount"]).Direction.Should().Be(System.Data.ParameterDirection.Input, "this should be an input parameter");
            ((NpgsqlParameter)prms["ByteValue"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.InternalChar, "that is the correct data type");
            ((NpgsqlParameter)prms["ByteValue"]).Value.Should().Be(System.DBNull.Value, "null should be DBNull");
            ((NpgsqlParameter)prms["ByteValue"]).Direction.Should().Be(System.Data.ParameterDirection.Input, "this should be an input parameter");
            ((NpgsqlParameter)prms["TrueFalse"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Boolean, "that is the correct data type");
            ((NpgsqlParameter)prms["TrueFalse"]).Value.Should().Be(System.DBNull.Value, "null should be DBNull");
            ((NpgsqlParameter)prms["TrueFalse"]).Direction.Should().Be(System.Data.ParameterDirection.Input, "this should be an input parameter");
            ((NpgsqlParameter)prms["GuidValue"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Uuid, "that is the correct data type");
            ((NpgsqlParameter)prms["GuidValue"]).Value.Should().Be(System.DBNull.Value, "null should be DBNull");
            ((NpgsqlParameter)prms["GuidValue"]).Direction.Should().Be(System.Data.ParameterDirection.Input, "this should be an input parameter");
            ((NpgsqlParameter)prms["GuidNull"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Uuid, "that is the correct data type");
            ((NpgsqlParameter)prms["GuidNull"]).Value.Should().Be(System.DBNull.Value, "null should be DBNull");
            ((NpgsqlParameter)prms["GuidNull"]).Direction.Should().Be(System.Data.ParameterDirection.Input, "this should be an input parameter");
            ((NpgsqlParameter)prms["Birthday"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Date, "that is the correct data type");
            ((NpgsqlParameter)prms["Birthday"]).Value.Should().Be(System.DBNull.Value, "null should be DBNull");
            ((NpgsqlParameter)prms["Birthday"]).Direction.Should().Be(System.Data.ParameterDirection.Input, "this should be an input parameter");
            ((NpgsqlParameter)prms["RightNow"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Timestamp, "that is the correct data type");
            ((NpgsqlParameter)prms["RightNow"]).Value.Should().Be(System.DBNull.Value, "null should be DBNull");
            ((NpgsqlParameter)prms["RightNow"]).Direction.Should().Be(System.Data.ParameterDirection.Input, "this should be an input parameter");
            ((NpgsqlParameter)prms["NowHere"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.TimeTz, "that is the correct data type");
            ((NpgsqlParameter)prms["NowHere"]).Value.Should().Be(System.DBNull.Value, "null should be DBNull");
            ((NpgsqlParameter)prms["NowHere"]).Direction.Should().Be(System.Data.ParameterDirection.Input, "this should be an input parameter");
            ((NpgsqlParameter)prms["NowElsewhere"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.TimestampTz, "that is the correct data type");
            ((NpgsqlParameter)prms["NowElsewhere"]).Value.Should().Be(System.DBNull.Value, "null should be DBNull");
            ((NpgsqlParameter)prms["NowElsewhere"]).Direction.Should().Be(System.Data.ParameterDirection.Input, "this should be an input parameter");
            ((NpgsqlParameter)prms["WakeUp"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Time, "that is the correct data type");
            ((NpgsqlParameter)prms["WakeUp"]).Value.Should().Be(System.DBNull.Value, "null should be DBNull");
            ((NpgsqlParameter)prms["WakeUp"]).Direction.Should().Be(System.Data.ParameterDirection.Input, "this should be an input parameter");
            ((NpgsqlParameter)prms["Latitude"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Numeric, "that is the correct data type");
            ((NpgsqlParameter)prms["Latitude"]).Value.Should().Be(System.DBNull.Value, "null should be DBNull");
            ((NpgsqlParameter)prms["Latitude"]).Precision.Should().Be(9, "that is the specified precision");
            ((NpgsqlParameter)prms["Latitude"]).Scale.Should().Be(6, "that is the specified scale");
            ((NpgsqlParameter)prms["Latitude"]).Direction.Should().Be(System.Data.ParameterDirection.Input, "this should be an input parameter");
            ((NpgsqlParameter)prms["Longitude"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Double, "that is the correct data type");
            ((NpgsqlParameter)prms["Longitude"]).Value.Should().Be(System.DBNull.Value, "null should be DBNull");
            ((NpgsqlParameter)prms["Longitude"]).Direction.Should().Be(System.Data.ParameterDirection.Input, "this should be an input parameter");
            ((NpgsqlParameter)prms["Altitude"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Real, "that is the correct data type");
            ((NpgsqlParameter)prms["Altitude"]).Value.Should().Be(System.DBNull.Value, "null should be DBNull");
            ((NpgsqlParameter)prms["Altitude"]).Direction.Should().Be(System.Data.ParameterDirection.Input, "this should be an input parameter");
            ((NpgsqlParameter)prms["Ratio"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Double, "that is the correct data type");
            ((NpgsqlParameter)prms["Ratio"]).Value.Should().Be(System.DBNull.Value, "null should be DBNull");
            ((NpgsqlParameter)prms["Ratio"]).Direction.Should().Be(System.Data.ParameterDirection.Input, "this should be an input parameter");
            ((NpgsqlParameter)prms["Temperature"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Real, "that is the correct data type");
            ((NpgsqlParameter)prms["Temperature"]).Value.Should().Be(System.DBNull.Value, "null should be DBNull");
            ((NpgsqlParameter)prms["Temperature"]).Direction.Should().Be(System.Data.ParameterDirection.Input, "this should be an input parameter");
            ((NpgsqlParameter)prms["LongStory"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Text, "that is the correct data type");
            ((NpgsqlParameter)prms["LongStory"]).Value.Should().Be(System.DBNull.Value, "null should be DBNull");
            ((NpgsqlParameter)prms["LongStory"]).Direction.Should().Be(System.Data.ParameterDirection.Input, "this should be an input parameter");
            ((NpgsqlParameter)prms["Numbers"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Array | NpgsqlTypes.NpgsqlDbType.Integer, "that is the correct data type");
            ((NpgsqlParameter)prms["Numbers"]).Value.Should().Be(System.DBNull.Value, "null should be DBNull");
            ((NpgsqlParameter)prms["Numbers"]).Direction.Should().Be(System.Data.ParameterDirection.Input, "this should be an input parameter");
            ((NpgsqlParameter)prms["MissingBits"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Bytea, "that is the correct data type");
            ((NpgsqlParameter)prms["MissingBits"]).Value.Should().Be(System.DBNull.Value, "null should be DBNull");
            ((NpgsqlParameter)prms["MissingBits"]).Size.Should().Be(12, "that is the max length");
            ((NpgsqlParameter)prms["MissingBits"]).Direction.Should().Be(System.Data.ParameterDirection.Input, "this should be an input parameter");
            ((NpgsqlParameter)prms["Price"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Money, "that is the correct data type");
            ((NpgsqlParameter)prms["Price"]).Value.Should().Be(System.DBNull.Value, "null should be DBNull");
            ((NpgsqlParameter)prms["Price"]).Direction.Should().Be(System.Data.ParameterDirection.Input, "this should be an input parameter");
            ((NpgsqlParameter)prms["EnvironmentTarget"]).Value.Should().Be(smv.EnvTarget.ToString(), "that is the assigned value");
            ((NpgsqlParameter)prms["EnvironmentTarget"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Varchar, "that is the correct data type");
            ((NpgsqlParameter)prms["ConsoleColor"]).Value.Should().Be((short)smv.Color, "that is the assigned value");
            ((NpgsqlParameter)prms["ConsoleColor"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Smallint, "that is the correct data type");
            ((NpgsqlParameter)prms["ConsoleModifiers"]).Value.Should().Be(ConsoleModifiers.Shift.ToString(), "that is the assigned value");
            ((NpgsqlParameter)prms["ConsoleModifiers"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Varchar, "that is the correct data type");
            ((NpgsqlParameter)prms["KeyValues"]).Value.Should().Be(System.DBNull.Value, "that is the assigned value");
            ((NpgsqlParameter)prms["KeyValues"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Hstore, "that is the correct data type");
            ((NpgsqlParameter)prms["CleanOut"]).Value.Should().Be(TimeSpan.Zero, "that is the assigned value");
            ((NpgsqlParameter)prms["CleanOut"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Interval, "that is the correct data type");
            ((NpgsqlParameter)prms["GCNotificationStatus"]).Value.Should().Be(GCNotificationStatus.Succeeded.ToString(), "that is the assigned value");
            ((NpgsqlParameter)prms["GCNotificationStatus"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Char, "that is the correct data type");

            ((NpgsqlParameter)prms["DataRecordId"]).Value.Should().Be(System.DBNull.Value, "an empty value should create a db null parameter");
            ((NpgsqlParameter)prms["ParentRecordId"]).Value.Should().Be(System.DBNull.Value, "a null value should create a db null parameter");
            ((NpgsqlParameter)prms["ChildRecordId"]).Value.Should().Be(System.DBNull.Value, "a null value should create a db null parameter");
            ((NpgsqlParameter)prms["DataRecordId2"]).Value.Should().Be(System.DBNull.Value, "an empty value should create a db null parameter");
            ((NpgsqlParameter)prms["ChildShard2"]).Value.Should().Be(System.DBNull.Value, "a null value should create a db null parameter");
            ((NpgsqlParameter)prms["ParentRecord2Id"]).Value.Should().Be(System.DBNull.Value, "a null value should create a db null parameter");
            ((NpgsqlParameter)prms["ChildRecord2Id"]).Value.Should().Be(System.DBNull.Value, "a null value should create a db null parameter");

        }
        [Fact]
        public void ValidateOutParameterCreator()
        {
            var smv = new PgMapModel()
            {
                ArgentSeaTestDataId = 1,
                Name = null,
                Iso3166 = null,
                BigCount = null,
                ValueCount = null,
                SmallCount = null,
                ByteCount = null,
                TrueFalse = null,
                GuidValue = Guid.Empty,
                GuidNull = null,
                Birthday = null,
                RightNow = null,
                NowHere = null,
                NowElsewhere = null,
                WakeUp = null,
                Latitude = null,
                Longitude = double.NaN,
                Altitude = float.NaN,
                Ratio = null,
                Temperature = null,
                LongStory = null,
                MissingBits = null,
                Price = null,
                EnvTarget = EnvironmentVariableTarget.Machine,
                Color = ConsoleColor.DarkMagenta,
                Modifier = ConsoleModifiers.Shift,
                KeyValues = null,
                CleanOutStuff = new TimeSpan(48, 0, 0),
                GarbageCollectorNotificationStatus = GCNotificationStatus.Succeeded,
            };
            var dbLogger = Substitute.For<Microsoft.Extensions.Logging.ILogger>();
            var prms = new ParameterCollection();

            prms.CreateOutputParameters<PgMapModel>(dbLogger);

            ((NpgsqlParameter)prms["ArgentSeaTestDataId"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Integer, "that is the correct data type");
            ((NpgsqlParameter)prms["ArgentSeaTestDataId"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
            ((NpgsqlParameter)prms["Name"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Varchar, "that is the correct data type");
            ((NpgsqlParameter)prms["Name"]).Size.Should().Be(255, "that is the max length");
            ((NpgsqlParameter)prms["Name"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
            ((NpgsqlParameter)prms["Iso3166"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Char, "that is the correct data type");
            ((NpgsqlParameter)prms["Iso3166"]).Size.Should().Be(2, "that is the fixed length");
            ((NpgsqlParameter)prms["Iso3166"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
            ((NpgsqlParameter)prms["BigCount"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Bigint, "that is the correct data type");
            ((NpgsqlParameter)prms["BigCount"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
            ((NpgsqlParameter)prms["ValueCount"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Integer, "that is the correct data type");
            ((NpgsqlParameter)prms["ValueCount"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
            ((NpgsqlParameter)prms["SmallCount"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Smallint, "that is the correct data type");
            ((NpgsqlParameter)prms["SmallCount"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
            ((NpgsqlParameter)prms["ByteValue"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.InternalChar, "that is the correct data type");
            ((NpgsqlParameter)prms["ByteValue"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
            ((NpgsqlParameter)prms["TrueFalse"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Boolean, "that is the correct data type");
            ((NpgsqlParameter)prms["TrueFalse"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
            ((NpgsqlParameter)prms["GuidValue"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Uuid, "that is the correct data type");
            ((NpgsqlParameter)prms["GuidValue"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
            ((NpgsqlParameter)prms["GuidNull"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Uuid, "that is the correct data type");
            ((NpgsqlParameter)prms["GuidNull"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
            ((NpgsqlParameter)prms["Birthday"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Date, "that is the correct data type");
            ((NpgsqlParameter)prms["Birthday"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
            ((NpgsqlParameter)prms["RightNow"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Timestamp, "that is the correct data type");
            ((NpgsqlParameter)prms["RightNow"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
            ((NpgsqlParameter)prms["NowHere"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.TimeTz, "that is the correct data type");
            ((NpgsqlParameter)prms["NowHere"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
            ((NpgsqlParameter)prms["NowElsewhere"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.TimestampTz, "that is the correct data type");
            ((NpgsqlParameter)prms["NowElsewhere"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
            ((NpgsqlParameter)prms["WakeUp"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Time, "that is the correct data type");
            ((NpgsqlParameter)prms["WakeUp"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
            ((NpgsqlParameter)prms["Latitude"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Numeric, "that is the correct data type");
            ((NpgsqlParameter)prms["Latitude"]).Precision.Should().Be(9, "that is the specified precision");
            ((NpgsqlParameter)prms["Latitude"]).Scale.Should().Be(6, "that is the specified scale");
            ((NpgsqlParameter)prms["Latitude"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
            ((NpgsqlParameter)prms["Longitude"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Double, "that is the correct data type");
            ((NpgsqlParameter)prms["Longitude"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
            ((NpgsqlParameter)prms["Altitude"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Real, "that is the correct data type");
            ((NpgsqlParameter)prms["Altitude"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
            ((NpgsqlParameter)prms["Ratio"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Double, "that is the correct data type");
            ((NpgsqlParameter)prms["Ratio"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
            ((NpgsqlParameter)prms["Temperature"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Real, "that is the correct data type");
            ((NpgsqlParameter)prms["Temperature"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
            ((NpgsqlParameter)prms["LongStory"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Text, "that is the correct data type");
            ((NpgsqlParameter)prms["LongStory"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
            ((NpgsqlParameter)prms["Numbers"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Array | NpgsqlTypes.NpgsqlDbType.Integer, "that is the correct data type");
            ((NpgsqlParameter)prms["Numbers"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
            ((NpgsqlParameter)prms["MissingBits"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Bytea, "that is the correct data type");
            ((NpgsqlParameter)prms["MissingBits"]).Size.Should().Be(12, "that is the max length");
            ((NpgsqlParameter)prms["MissingBits"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
            ((NpgsqlParameter)prms["Price"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Money, "that is the correct data type");
            ((NpgsqlParameter)prms["Price"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
            ((NpgsqlParameter)prms["EnvironmentTarget"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
            ((NpgsqlParameter)prms["EnvironmentTarget"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Varchar, "that is the correct data type");
            ((NpgsqlParameter)prms["ConsoleColor"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
            ((NpgsqlParameter)prms["ConsoleColor"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Smallint, "that is the correct data type");
            ((NpgsqlParameter)prms["ConsoleModifiers"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
            ((NpgsqlParameter)prms["ConsoleModifiers"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Varchar, "that is the correct data type");
            ((NpgsqlParameter)prms["KeyValues"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
            ((NpgsqlParameter)prms["KeyValues"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Hstore, "that is the correct data type");
            ((NpgsqlParameter)prms["CleanOut"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
            ((NpgsqlParameter)prms["CleanOut"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Interval, "that is the correct data type");
            ((NpgsqlParameter)prms["GCNotificationStatus"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
            ((NpgsqlParameter)prms["GCNotificationStatus"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Char, "that is the correct data type");

            //((NpgsqlParameter)prms["DataShard"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
            //((NpgsqlParameter)prms["DataShard"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.TinyInt, "that is the correct data type");
            //((NpgsqlParameter)prms["DataRecordId"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
            //((NpgsqlParameter)prms["DataRecordId"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Int, "that is the correct data type");

            ////((NpgsqlParameter)prms["ChildShard"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
            ////((NpgsqlParameter)prms["ChildShard"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.TinyInt, "that is the correct data type");
            //((NpgsqlParameter)prms["ParentRecordId"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
            //((NpgsqlParameter)prms["ParentRecordId"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.Int, "that is the correct data type");
            //((NpgsqlParameter)prms["ChildRecordId"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
            //((NpgsqlParameter)prms["ChildRecordId"]).NpgsqlDbType.Should().Be(NpgsqlTypes.NpgsqlDbType.SmallInt, "that is the correct data type");

            //((NpgsqlParameter)prms["DataRecordId2"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
            //((NpgsqlParameter)prms["ChildShard2"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
            //((NpgsqlParameter)prms["ParentRecord2Id"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
            //((NpgsqlParameter)prms["ChildRecord2Id"]).Direction.Should().Be(System.Data.ParameterDirection.Output, "this should be an output parameter");
        }
        [Fact]
        public void ValidateOutParameterReader()
        {

            var cmd = new NpgsqlCommand();
            var guid = Guid.NewGuid();

            cmd.Parameters.Add(new NpgsqlParameter("ArgentSeaTestDataId", NpgsqlTypes.NpgsqlDbType.Integer) { Value = 10, Direction = System.Data.ParameterDirection.Output });
            cmd.Parameters.Add(new NpgsqlParameter("Name", NpgsqlTypes.NpgsqlDbType.Varchar, 255) { Value = "Test2", Direction = System.Data.ParameterDirection.Output });
            cmd.Parameters.Add(new NpgsqlParameter("Iso3166", NpgsqlTypes.NpgsqlDbType.Char, 2) { Value = "US", Direction = System.Data.ParameterDirection.Output });
            cmd.Parameters.Add(new NpgsqlParameter("BigCount", NpgsqlTypes.NpgsqlDbType.Bigint) { Value = 5L, Direction = System.Data.ParameterDirection.Output });
            cmd.Parameters.Add(new NpgsqlParameter("ValueCount", NpgsqlTypes.NpgsqlDbType.Integer) { Value = 6, Direction = System.Data.ParameterDirection.Output });
            cmd.Parameters.Add(new NpgsqlParameter("SmallCount", NpgsqlTypes.NpgsqlDbType.Smallint) { Value = (short)7, Direction = System.Data.ParameterDirection.Output });
            cmd.Parameters.Add(new NpgsqlParameter("ByteValue", NpgsqlTypes.NpgsqlDbType.InternalChar) { Value = (byte)8, Direction = System.Data.ParameterDirection.Output });
            cmd.Parameters.Add(new NpgsqlParameter("TrueFalse", NpgsqlTypes.NpgsqlDbType.Boolean) { Value = false, Direction = System.Data.ParameterDirection.Output });
            cmd.Parameters.Add(new NpgsqlParameter("GuidValue", NpgsqlTypes.NpgsqlDbType.Uuid) { Value = guid, Direction = System.Data.ParameterDirection.Output });
            cmd.Parameters.Add(new NpgsqlParameter("GuidNull", NpgsqlTypes.NpgsqlDbType.Uuid) { Value = guid, Direction = System.Data.ParameterDirection.Output });
            cmd.Parameters.Add(new NpgsqlParameter("Birthday", NpgsqlTypes.NpgsqlDbType.Date) { Value = new DateTime(2018, 1, 1), Direction = System.Data.ParameterDirection.Output });
            cmd.Parameters.Add(new NpgsqlParameter("RightNow", NpgsqlTypes.NpgsqlDbType.Timestamp) { Value = new DateTime(2018, 2, 1), Direction = System.Data.ParameterDirection.Output });
            cmd.Parameters.Add(new NpgsqlParameter("NowHere", NpgsqlTypes.NpgsqlDbType.TimeTz) { Value = new DateTimeOffset(new DateTime(1900, 1, 1, 17, 15, 32)), Direction = System.Data.ParameterDirection.Output });
            cmd.Parameters.Add(new NpgsqlParameter("ExactlyNow", NpgsqlTypes.NpgsqlDbType.TimeTz) { Value = new DateTime(2018, 3, 1), Direction = System.Data.ParameterDirection.Output });
            cmd.Parameters.Add(new NpgsqlParameter("NowElsewhere", NpgsqlTypes.NpgsqlDbType.TimestampTz) { Value = new DateTimeOffset(2018, 1, 1, 1, 1, 1, new TimeSpan()), Direction = System.Data.ParameterDirection.Output });
            cmd.Parameters.Add(new NpgsqlParameter("WakeUp", NpgsqlTypes.NpgsqlDbType.Time) { Value = new TimeSpan(12, 0, 0), Direction = System.Data.ParameterDirection.Output });
            cmd.Parameters.Add(new NpgsqlParameter("Latitude", NpgsqlTypes.NpgsqlDbType.Numeric) { Value = 12.345m, Precision = 9, Scale = 4, Direction = System.Data.ParameterDirection.Output });
            cmd.Parameters.Add(new NpgsqlParameter("Longitude", NpgsqlTypes.NpgsqlDbType.Double) { Value = 123.45, Direction = System.Data.ParameterDirection.Output });
            cmd.Parameters.Add(new NpgsqlParameter("Altitude", NpgsqlTypes.NpgsqlDbType.Real) { Value = 1234.5f, Direction = System.Data.ParameterDirection.Output });
            cmd.Parameters.Add(new NpgsqlParameter("Ratio", NpgsqlTypes.NpgsqlDbType.Double) { Value = 12345.6, Direction = System.Data.ParameterDirection.Output });
            cmd.Parameters.Add(new NpgsqlParameter("Temperature", NpgsqlTypes.NpgsqlDbType.Real) { Value = 123467.8f, Direction = System.Data.ParameterDirection.Output });
            cmd.Parameters.Add(new NpgsqlParameter("LongStory", NpgsqlTypes.NpgsqlDbType.Text) { Value = "Long story....", Direction = System.Data.ParameterDirection.Output });
            cmd.Parameters.Add(new NpgsqlParameter("MissingBits", NpgsqlTypes.NpgsqlDbType.Bytea) { Value = new byte[] { 1, 2, 4, 8, 16 }, Direction = System.Data.ParameterDirection.Output });
            cmd.Parameters.Add(new NpgsqlParameter("Price", NpgsqlTypes.NpgsqlDbType.Money) { Value = 1.2345m, Direction = System.Data.ParameterDirection.Output });
            cmd.Parameters.Add(new NpgsqlParameter("EnvironmentTarget", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = EnvironmentVariableTarget.Machine.ToString(), Direction = System.Data.ParameterDirection.Output });
            cmd.Parameters.Add(new NpgsqlParameter("ConsoleColor", NpgsqlTypes.NpgsqlDbType.Smallint) { Value = (short)ConsoleColor.Black, Direction = System.Data.ParameterDirection.Output });
            cmd.Parameters.Add(new NpgsqlParameter("ConsoleModifiers", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = ConsoleModifiers.Control.ToString(), Direction = System.Data.ParameterDirection.Output });
            cmd.Parameters.Add(new NpgsqlParameter("KeyValues", NpgsqlTypes.NpgsqlDbType.Hstore) { Value = new Dictionary<string, string> { { "one", "1" }, { "two", "2" } }, Direction = System.Data.ParameterDirection.Output });
            cmd.Parameters.Add(new NpgsqlParameter("CleanOut", NpgsqlTypes.NpgsqlDbType.Interval) { Value = new TimeSpan(3, 4, 5), Direction = System.Data.ParameterDirection.Output });
            cmd.Parameters.Add(new NpgsqlParameter("GCNotificationStatus", NpgsqlTypes.NpgsqlDbType.Char) { Value = GCNotificationStatus.Failed.ToString(), Direction = System.Data.ParameterDirection.Output });

            cmd.Parameters.Add(new NpgsqlParameter("DataShard", NpgsqlTypes.NpgsqlDbType.Smallint) { Value = (short)6, Direction = System.Data.ParameterDirection.Output });
            cmd.Parameters.Add(new NpgsqlParameter("DataRecordId", NpgsqlTypes.NpgsqlDbType.Integer) { Value = 4, Direction = System.Data.ParameterDirection.Output });

            cmd.Parameters.Add(new NpgsqlParameter("ChildShard", NpgsqlTypes.NpgsqlDbType.Smallint) { Value = (short)15, Direction = System.Data.ParameterDirection.Output });
            cmd.Parameters.Add(new NpgsqlParameter("ParentRecordId", NpgsqlTypes.NpgsqlDbType.Integer) { Value = 5, Direction = System.Data.ParameterDirection.Output });
            cmd.Parameters.Add(new NpgsqlParameter("ChildRecordId", NpgsqlTypes.NpgsqlDbType.Smallint) { Value = (short)6, Direction = System.Data.ParameterDirection.Output });

            cmd.Parameters.Add(new NpgsqlParameter("DataRecordId2", NpgsqlTypes.NpgsqlDbType.Bigint) { Value = long.MaxValue, Direction = System.Data.ParameterDirection.Output });

            cmd.Parameters.Add(new NpgsqlParameter("ChildShard2", NpgsqlTypes.NpgsqlDbType.Smallint) { Value = (short)255, Direction = System.Data.ParameterDirection.Output });
            cmd.Parameters.Add(new NpgsqlParameter("ParentRecord2Id", NpgsqlTypes.NpgsqlDbType.Integer) { Value = 12345, Direction = System.Data.ParameterDirection.Output });
            cmd.Parameters.Add(new NpgsqlParameter("ChildRecord2Id", NpgsqlTypes.NpgsqlDbType.Varchar, 255) { Value = "Test123", Direction = System.Data.ParameterDirection.Output });

            var dbLogger2 = new LoggerFactory();
            dbLogger2.AddConsole();
            var dbLogger = dbLogger2.CreateLogger("");
            var result = cmd.Parameters.ToModel<PgMapModel>((short)5, dbLogger);
            result.ArgentSeaTestDataId.Should().Be(10, "that was the output parameter value");
            result.Name.Should().Be("Test2", "that was the output parameter value");
            result.Iso3166.Should().Be("US", "that was the output parameter value");
            result.BigCount.Should().Be(5L, "that was the output parameter value");
            result.ValueCount.Should().Be(6, "that was the output parameter value");
            result.SmallCount.Should().Be(7, "that was the output parameter value");
            result.ByteCount.Should().Be(8, "that was the output parameter value");
            result.TrueFalse.Should().Be(false, "that was the output parameter value");
            result.GuidValue.Should().Be(guid, "that was the output parameter value");
            result.GuidNull.Should().Be(guid, "that was the output parameter value");
            result.Birthday.Should().Be(new DateTime(2018, 1, 1), "that was the output parameter value");
            result.RightNow.Should().Be(new DateTime(2018, 2, 1), "that was the output parameter value");
            result.NowHere.Should().Be(new DateTime(1900, 1, 1, 17, 15, 32), "that was the output parameter value");
            result.NowElsewhere.Should().Be(new DateTimeOffset(2018, 1, 1, 1, 1, 1, new TimeSpan()), "that was the output parameter value");
            result.WakeUp.Should().Be(new TimeSpan(12, 0, 0), "that was the output parameter value");
            result.Latitude.Should().Be(12.345m, "that was the output parameter value");
            result.Longitude.Should().Be(123.45, "that was the output parameter value");
            result.Altitude.Should().Be(1234.5f, "that was the output parameter value");
            result.Ratio.Should().Be(12345.6, "that was the output parameter value");
            result.Temperature.Should().Be(123467.8f, "that was the output parameter value");
            result.LongStory.Should().Be("Long story....", "that was the output parameter value");
            result.MissingBits.Should().Equal(new byte[] { 1, 2, 4, 8, 16 }, "that was the output parameter value");
            result.Price.Should().Be(1.2345m, "that was the output parameter value");

            result.EnvTarget.Should().Be(EnvironmentVariableTarget.Machine, "that was the output parameter value");
            result.Color.Should().Be(ConsoleColor.Black, "that was the output parameter value");
            result.Modifier.Should().Be(ConsoleModifiers.Control, "that was the output parameter value");
            result.KeyValues["one"].Should().Be("1", "that was the output parameter value");
            result.KeyValues["two"].Should().Be("2", "that was the output parameter value");
            result.CleanOutStuff.Should().Be(new TimeSpan(3, 4, 5), "that was the output parameter value");
            result.GarbageCollectorNotificationStatus.Should().Be(GCNotificationStatus.Failed, "that was the output parameter value");

            result.RecordKey.Value.Origin.Should().Be('x', "that is the data origin value");
            result.RecordKey.Value.ShardId.Should().Be(6, "that was the output parameter value");
            result.RecordKey.Value.RecordId.Should().Be(4, "that was the output parameter value");

            result.RecordChild.Key.Origin.Should().Be('y', "that is the data origin value");
            result.RecordChild.Key.ShardId.Should().Be(15, "that was the output parameter value");
            result.RecordChild.Key.RecordId.Should().Be(5, "that was the output parameter value");
            result.RecordChild.ChildId.Should().Be(6, "that was the output parameter value");

            result.DataShard2.Origin.Should().Be('A', "that is the data origin value");
            result.DataShard2.ShardId.Should().Be(5, "that is the value of the current shard");
            result.DataShard2.RecordId.Should().Be(long.MaxValue, "that is the record id");

            result.ChildShard2.Value.Origin.Should().Be('B', "that is the data origin value");
            result.ChildShard2.Value.ShardId.Should().Be(255, "that is the value of the current shard");
            result.ChildShard2.Value.RecordId.Should().Be(12345, "that is the record id");
            result.ChildShard2.Value.ChildId.Should().Be("Test123", "that is the child id");

        }
        [Fact]
        public void ValidateOutNullParameterReader()
        {

            var dbLogger = Substitute.For<Microsoft.Extensions.Logging.ILogger>();
            var prms = new ParameterCollection();
            var guid = Guid.NewGuid();


            prms.Add(new NpgsqlParameter("ArgentSeaTestDataId", NpgsqlTypes.NpgsqlDbType.Integer) { Value = 11, Direction = System.Data.ParameterDirection.Output });
            prms.Add(new NpgsqlParameter("Name", NpgsqlTypes.NpgsqlDbType.Varchar, 255) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });
            prms.Add(new NpgsqlParameter("Iso3166", NpgsqlTypes.NpgsqlDbType.Char, 2) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });
            prms.Add(new NpgsqlParameter("BigCount", NpgsqlTypes.NpgsqlDbType.Bigint) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });
            prms.Add(new NpgsqlParameter("ValueCount", NpgsqlTypes.NpgsqlDbType.Integer) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });
            prms.Add(new NpgsqlParameter("SmallCount", NpgsqlTypes.NpgsqlDbType.Smallint) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });
            prms.Add(new NpgsqlParameter("ByteValue", NpgsqlTypes.NpgsqlDbType.InternalChar) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });
            prms.Add(new NpgsqlParameter("TrueFalse", NpgsqlTypes.NpgsqlDbType.Boolean) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });
            prms.Add(new NpgsqlParameter("GuidValue", NpgsqlTypes.NpgsqlDbType.Uuid) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });
            prms.Add(new NpgsqlParameter("GuidNull", NpgsqlTypes.NpgsqlDbType.Uuid) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });
            prms.Add(new NpgsqlParameter("Birthday", NpgsqlTypes.NpgsqlDbType.Date) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });
            prms.Add(new NpgsqlParameter("NowHere", NpgsqlTypes.NpgsqlDbType.TimeTz) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });
            prms.Add(new NpgsqlParameter("RightNow", NpgsqlTypes.NpgsqlDbType.Timestamp) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });
            prms.Add(new NpgsqlParameter("NowElsewhere", NpgsqlTypes.NpgsqlDbType.TimestampTz) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });
            prms.Add(new NpgsqlParameter("WakeUp", NpgsqlTypes.NpgsqlDbType.Time) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });
            prms.Add(new NpgsqlParameter("Latitude", NpgsqlTypes.NpgsqlDbType.Numeric) { Value = System.DBNull.Value, Precision = 9, Scale = 4, Direction = System.Data.ParameterDirection.Output });
            prms.Add(new NpgsqlParameter("Longitude", NpgsqlTypes.NpgsqlDbType.Double) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });
            prms.Add(new NpgsqlParameter("Altitude", NpgsqlTypes.NpgsqlDbType.Real) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });
            prms.Add(new NpgsqlParameter("Ratio", NpgsqlTypes.NpgsqlDbType.Double) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });
            prms.Add(new NpgsqlParameter("Temperature", NpgsqlTypes.NpgsqlDbType.Real) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });
            prms.Add(new NpgsqlParameter("LongStory", NpgsqlTypes.NpgsqlDbType.Text) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });
            prms.Add(new NpgsqlParameter("Numbers", NpgsqlTypes.NpgsqlDbType.Array | NpgsqlTypes.NpgsqlDbType.Integer) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });
            prms.Add(new NpgsqlParameter("MissingBits", NpgsqlTypes.NpgsqlDbType.Bytea) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });
            prms.Add(new NpgsqlParameter("Price", NpgsqlTypes.NpgsqlDbType.Money) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });
            prms.Add(new NpgsqlParameter("EnvironmentTarget", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = EnvironmentVariableTarget.Process.ToString(), Direction = System.Data.ParameterDirection.Output });
            prms.Add(new NpgsqlParameter("ConsoleColor", NpgsqlTypes.NpgsqlDbType.Smallint) { Value = (short)ConsoleColor.DarkBlue, Direction = System.Data.ParameterDirection.Output });
            prms.Add(new NpgsqlParameter("ConsoleModifiers", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = ConsoleModifiers.Shift.ToString(), Direction = System.Data.ParameterDirection.Output });
            prms.Add(new NpgsqlParameter("KeyValues", NpgsqlTypes.NpgsqlDbType.Hstore) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });
            prms.Add(new NpgsqlParameter("CleanOut", NpgsqlTypes.NpgsqlDbType.Interval) { Value = new TimeSpan(5,6,7), Direction = System.Data.ParameterDirection.Output });
            prms.Add(new NpgsqlParameter("GCNotificationStatus", NpgsqlTypes.NpgsqlDbType.Char) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });

            prms.Add(new NpgsqlParameter("DataShard", NpgsqlTypes.NpgsqlDbType.Smallint) { Value = (short)2, Direction = System.Data.ParameterDirection.Output });
            prms.Add(new NpgsqlParameter("DataRecordId", NpgsqlTypes.NpgsqlDbType.Integer) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });
            prms.Add(new NpgsqlParameter("ChildShard", NpgsqlTypes.NpgsqlDbType.Smallint) { Value = (short)100, Direction = System.Data.ParameterDirection.Output });
            prms.Add(new NpgsqlParameter("ParentRecordId", NpgsqlTypes.NpgsqlDbType.Integer) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });
            prms.Add(new NpgsqlParameter("ChildRecordId", NpgsqlTypes.NpgsqlDbType.Bigint) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });
            prms.Add(new NpgsqlParameter("DataRecordId2", NpgsqlTypes.NpgsqlDbType.Bigint) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });
            prms.Add(new NpgsqlParameter("ChildShard2", NpgsqlTypes.NpgsqlDbType.Smallint) { Value = (short)1, Direction = System.Data.ParameterDirection.Output });
            prms.Add(new NpgsqlParameter("ParentRecord2Id", NpgsqlTypes.NpgsqlDbType.Integer) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });
            prms.Add(new NpgsqlParameter("ChildRecord2Id", NpgsqlTypes.NpgsqlDbType.Varchar, 255) { Value = System.DBNull.Value, Direction = System.Data.ParameterDirection.Output });

            var result = Mapper.ToModel<PgMapModel>(prms, 16, dbLogger);

            result.ArgentSeaTestDataId.Should().Be(11, "that was the output parameter value");
            result.Name.Should().BeNull("the output parameter was set to DbNull");
            result.Iso3166.Should().BeNull("the output parameter was set to DbNull");
            result.BigCount.Should().BeNull("the output parameter was set to DbNull");
            result.ValueCount.Should().BeNull("the output parameter was set to DbNull");
            result.SmallCount.Should().BeNull("the output parameter was set to DbNull");
            result.ByteCount.Should().BeNull("the output parameter was set to DbNull");
            result.TrueFalse.Should().BeNull("the output parameter was set to DbNull");
            result.GuidValue.Should().Be(Guid.Empty, "the output parameter was set to DbNull");
            result.GuidNull.Should().BeNull("the output parameter was set to DbNull");
            result.Birthday.Should().BeNull("the output parameter was set to DbNull");
            result.RightNow.Should().BeNull("the output parameter was set to DbNull");
            result.NowHere.Should().BeNull("the output parameter was set to DbNull");
            result.NowElsewhere.Should().BeNull("the output parameter was set to DbNull");
            result.WakeUp.Should().BeNull("the output parameter was set to DbNull");
            result.Latitude.Should().BeNull("the output parameter was set to DbNull");
            double.IsNaN(result.Longitude).Should().BeTrue("the output parameter was set to DbNull");
            float.IsNaN(result.Altitude).Should().BeTrue("the output parameter was set to DbNull");
            result.Ratio.Should().BeNull("the output parameter was set to DbNull");
            result.Temperature.Should().BeNull("the output parameter was set to DbNull");
            result.LongStory.Should().BeNull("the output parameter was set to DbNull");
            result.Numbers.Should().BeNull("the output parameter was set to DbNull");
            result.MissingBits.Should().Equal(null, "the output parameter was set to DbNull");
            result.Price.Should().BeNull("the output parameter was set to DbNull");
            result.EnvTarget.Should().Be(EnvironmentVariableTarget.Process, "that was the output parameter value");
            result.Color.Should().Be(ConsoleColor.DarkBlue, "that was the output parameter value");
            result.Modifier.Should().Be(ConsoleModifiers.Shift, "that was the output parameter value");
            result.KeyValues.Should().BeNull("the output parameter was set to DbNull");
            result.CleanOutStuff.Should().Be(new TimeSpan(5, 6, 7), "that was the output parameter value");
            result.GarbageCollectorNotificationStatus.HasValue.Should().BeFalse("the output parameter was set to DbNull");
            //result.RecordKey.HasValue.Should().BeFalse("because the input arguments are dbNull");
            //result.RecordChild.Origin.Should().Be('0', "that is the empty data origin value");
            //result.RecordChild.Key.ShardId.Should().Be(0, "that is the empty shardchild value");
            //result.RecordChild.Key.RecordId.Should().Be(0, "that is the empty shardchild value");
            //result.RecordChild.ChildId.Should().Be(0, "that is the empty shardchild value");
            //result.DataShard2.Origin.Should().Be('0', "that is the empty data origin value");
            //result.DataShard2.ShardId.Should().Be(0, "that is the empty shardKey value");
            //result.DataShard2.RecordId.Should().Be(0, "that is the empty shardKey value");
            //result.ChildShard2.Should().BeNull("because the data values are dbNull");
        }
        //[Fact]
        //public void ValidateSqlMetadataMapper()
        //{
        //    var smv = new PgMapModel()
        //    {
        //        ArgentSeaTestDataId = 1,
        //        Name = "Test2",
        //        Iso3166 = "US",
        //        BigCount = 4,
        //        ValueCount = 5,
        //        SmallCount = 6,
        //        ByteCount = 7,
        //        TrueFalse = true,
        //        GuidValue = Guid.NewGuid(),
        //        GuidNull = Guid.NewGuid(),
        //        Birthday = new DateTime(2008, 8, 8),
        //        RightNow = new DateTime(2009, 9, 9),
        //        NowHere = new DateTime(2010, 10, 10),
        //        NowElsewhere = new DateTimeOffset(2011, 11, 11, 11, 11, 11, new TimeSpan(11, 11, 00)),
        //        WakeUp = new TimeSpan(12, 12, 12),
        //        Latitude = 13.13m,
        //        Longitude = 14.14,
        //        Altitude = 15.15f,
        //        Ratio = 16.5,
        //        Temperature = 17.6f,
        //        LongStory = "18",
        //        Numbers = new int[] { 5, 6, 7, 8 },
        //        MissingBits = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 21 },
        //        Price = 23.0m,
        //        EnvTarget = EnvironmentVariableTarget.User,
        //        Color = ConsoleColor.Red,
        //        Modifier = ConsoleModifiers.Shift,
        //        KeyValues = new Dictionary<string, string>() { { "one", "1" }, { "two", "2" } },
        //        CleanOutStuff = new TimeSpan(3, 4, 5),
        //        GarbageCollectorNotificationStatus = GCNotificationStatus.NotApplicable,
        //        RecordKey = new ShardKey<short, int>(new DataOrigin('?'), (short)254, int.MaxValue),
        //        RecordChild = new ShardChild<short, int, long>(new DataOrigin('!'), (short)0, 35, long.MinValue),
        //        DataShard2 = new ShardKey<short, long>(new DataOrigin('*'), (short)0, 123L),
        //        ChildShard2 = new Nullable<ShardChild<short, int, string>>(new ShardChild<short, int, string>(new DataOrigin('@'), (short)200, (int)1234, "testing..."))
        //    };
        //    var dbLogger = Substitute.For<Microsoft.Extensions.Logging.ILogger>();

        //    var result = TvpMapper.ToTvpRecord<SqlMapModel>(smv, dbLogger);

        //    result.GetInt32(0).Should().Be(smv.ArgentSeaTestDataId, "that was the id value provided");
        //    result.GetString(1).Should().Be(smv.Name, "that was the Name value provided");
        //    result.GetString(2).Should().Be(smv.LatinName, "that was the Ansi value provided");
        //    result.GetString(3).Should().Be(smv.Iso3166, "that was the Iso3166 value provided");
        //    result.GetString(4).Should().Be(smv.Iso639, "that was the Iso639 value provided");
        //    result.GetInt64(5).Should().Be(smv.BigCount, "that was the value provided");
        //    result.GetInt32(6).Should().Be(smv.ValueCount, "that was the value provided");
        //    result.GetInt16(7).Should().Be(smv.SmallCount, "that was the value provided");
        //    result.GetByte(8).Should().Be(smv.ByteCount, "that was the value provided");
        //    result.GetBoolean(9).Should().Be(smv.TrueFalse.Value, "that was the value provided");
        //    result.GetGuid(10).Should().Be(smv.GuidValue, "that was the value provided");
        //    result.GetGuid(11).Should().Be(smv.GuidNull.Value, "that was the value provided");
        //    result.GetDateTime(12).Should().Be(smv.Birthday.Value, "that was the value provided");
        //    result.GetDateTime(13).Should().Be(smv.RightNow.Value, "that was the value provided");
        //    result.GetDateTime(14).Should().Be(smv.ExactlyNow.Value, "that was the value provided");
        //    result.GetDateTimeOffset(15).Should().Be(smv.NowElsewhere.Value, "that was the value provided");
        //    result.GetTimeSpan(16).Should().Be(smv.WakeUp.Value, "that was the value provided");
        //    result.GetDecimal(17).Should().Be(smv.Latitude, "that was the value provided");
        //    result.GetDouble(18).Should().Be(smv.Longitude, "that was the value provided");
        //    result.GetFloat(19).Should().Be(smv.Altitude, "that was the value provided");
        //    result.GetDouble(20).Should().Be(smv.Ratio, "that was the value provided");
        //    result.GetFloat(21).Should().Be(smv.Temperature, "that was the value provided");
        //    result.GetString(22).Should().Be(smv.LongStory, "that was the value provided");
        //    result.GetString(23).Should().Be(smv.LatinStory, "that was the value provided");
        //    var twoBits = new byte[smv.TwoBits.Length];
        //    result.GetBytes(24, 0, twoBits, 0, twoBits.Length);
        //    for (var i = 0; i < twoBits.Length; i++)
        //    {
        //        twoBits[i].Should().Be(smv.TwoBits[i], "that was the value provided");
        //    }
        //    var missingBits = new byte[smv.MissingBits.Length];
        //    result.GetBytes(25, 0, missingBits, 0, missingBits.Length);
        //    for (var i = 0; i < missingBits.Length; i++)
        //    {
        //        missingBits[i].Should().Be(smv.MissingBits[i], "that was the value provided");
        //    }
        //    var blob = new byte[smv.Blob.Length];
        //    result.GetBytes(26, 0, blob, 0, blob.Length);
        //    for (var i = 0; i < blob.Length; i++)
        //    {
        //        blob[i].Should().Be(smv.Blob[i], "that was the value provided");
        //    }
        //    result.GetDecimal(27).Should().Be(smv.Price, "that was the value provided");
        //    result.GetDecimal(28).Should().Be(smv.Cost, "that was the value provided");
        //    result.GetString(29).Should().Be(EnvironmentVariableTarget.User.ToString(), "that was the value provided");
        //    result.GetInt16(30).Should().Be((short)ConsoleColor.Red, "that was the value provided");
        //    result.GetString(31).Should().Be(ConsoleModifiers.Shift.ToString(), "that was the value provided");
        //    result.GetByte(32).Should().Be((byte)DayOfWeek.Wednesday, "that was the value provided");
        //    result.GetString(33).Should().Be(GCNotificationStatus.NotApplicable.ToString(), "that was the value provided");
        //    ////result.GetByte(34).Should().Be((byte)254, "that was the value provided");
        //    //result.GetInt32(34).Should().Be(int.MaxValue, "that was the value provided");
        //    ////result.GetByte(36).Should().Be((byte)0, "that was the value provided");
        //    //result.GetInt32(35).Should().Be(35, "that was the value provided");
        //    //result.GetInt16(36).Should().Be(short.MinValue, "that was the value provided");
        //    //result.GetByte(37).Should().Be(0, "that was the value provided");
        //    //result.GetInt32(38).Should().Be(123, "that was the value provided");
        //    //result.GetByte(39).Should().Be((byte)200, "that was the value provided");
        //    //result.GetInt32(40).Should().Be(1234, "that was the value provided");
        //    //result.GetInt16(41).Should().Be((short)5678, "that was the value provided");
        //}
        //[Fact]
        //public void ValidateSqlMetadataNullMapper()
        //{
        //    var smv = new SqlMapModel()
        //    {
        //        ArgentSeaTestDataId = 1,
        //        Name = null,
        //        LatinName = null,
        //        Iso3166 = null,
        //        Iso639 = null,
        //        BigCount = null,
        //        ValueCount = null,
        //        SmallCount = null,
        //        ByteCount = null,
        //        TrueFalse = null,
        //        GuidValue = Guid.Empty,
        //        GuidNull = null,
        //        Birthday = null,
        //        RightNow = null,
        //        ExactlyNow = null,
        //        NowElsewhere = null,
        //        WakeUp = null,
        //        Latitude = null,
        //        Longitude = double.NaN,
        //        Altitude = float.NaN,
        //        Ratio = null,
        //        Temperature = null,
        //        LongStory = null,
        //        LatinStory = null,
        //        TwoBits = null,
        //        MissingBits = null,
        //        Blob = null,
        //        Price = null,
        //        Cost = null,
        //        EnvTarget = EnvironmentVariableTarget.User,
        //        Color = ConsoleColor.Red,
        //        Modifier = ConsoleModifiers.Shift,
        //        DayOfTheWeek = null,
        //        GarbageCollectorNotificationStatus = null
        //        //RecordKey = ShardKey.Empty,
        //        //RecordChild = ShardChild.Empty,
        //        //RecordKeyTwo = null,
        //        //RecordChild2 = null
        //    };
        //    var dbLogger = Substitute.For<Microsoft.Extensions.Logging.ILogger>();
        //    var prms = new QueryParameterCollection();

        //    var result = TvpMapper.ToTvpRecord<SqlMapModel>(smv, dbLogger);

        //    result.GetInt32(0).Should().Be(smv.ArgentSeaTestDataId, "that was the id value provided");
        //    result.IsDBNull(1).Should().Be(true, "a null Name property was provided");
        //    result.IsDBNull(2).Should().Be(true, "a null LatinName property was provided");
        //    result.IsDBNull(3).Should().Be(true, "a null Iso3166 property was provided");
        //    result.IsDBNull(4).Should().Be(true, "a null Iso639 property was provided");
        //    result.IsDBNull(5).Should().Be(true, "a null BigCount property was provided");
        //    result.IsDBNull(6).Should().Be(true, "a null ValueCount property was provided");
        //    result.IsDBNull(7).Should().Be(true, "a null SmallCount property was provided");
        //    result.IsDBNull(8).Should().Be(true, "a null ByteValue property was provided");
        //    result.IsDBNull(9).Should().Be(true, "a null TrueFalse property was provided");
        //    result.IsDBNull(10).Should().Be(true, "an empty GuidValue property was provided");
        //    result.IsDBNull(11).Should().Be(true, "an null GuidNull property was provided");
        //    result.IsDBNull(12).Should().Be(true, "a null Birthday property was provided");
        //    result.IsDBNull(13).Should().Be(true, "a null RightNow property was provided");
        //    result.IsDBNull(14).Should().Be(true, "a null ExactlyNow property was provided");
        //    result.IsDBNull(15).Should().Be(true, "a null NowElsewhere property was provided");
        //    result.IsDBNull(16).Should().Be(true, "a null WakeUp property was provided");
        //    result.IsDBNull(17).Should().Be(true, "a null Latitude property was provided");
        //    result.IsDBNull(18).Should().Be(true, "a NaN Longitude property was provided");
        //    result.IsDBNull(19).Should().Be(true, "a NaN Altitude property was provided");
        //    result.IsDBNull(20).Should().Be(true, "a null Ratio property was provided");
        //    result.IsDBNull(21).Should().Be(true, "a null Temperature property was provided");
        //    result.IsDBNull(22).Should().Be(true, "a null LongStory property was provided");
        //    result.IsDBNull(23).Should().Be(true, "a null LatinStory property was provided");
        //    result.IsDBNull(24).Should().Be(true, "a null TwoBits property was provided");
        //    result.IsDBNull(25).Should().Be(true, "a null MissingBits property was provided");
        //    result.IsDBNull(26).Should().Be(true, "a null Blob property was provided");
        //    result.IsDBNull(27).Should().Be(true, "a null Price property was provided");
        //    result.IsDBNull(28).Should().Be(true, "a null Cost property was provided");
        //    result.IsDBNull(32).Should().Be(true, "a null DayOfTheWeek property was provided");
        //    result.IsDBNull(33).Should().Be(true, "a null GarbageCollectorNotificationStatus property was provided");
        //    //result.IsDBNull(34).Should().Be(true, "an empty RecordKey property was provided");
        //    //result.IsDBNull(35).Should().Be(true, "an empty RecordChild property was provided");
        //    //result.IsDBNull(36).Should().Be(true, "a empty  RecordChild property was provided");
        //    //result.IsDBNull(37).Should().Be(true, "a null RecordKeyTwo property was provided");
        //    //result.IsDBNull(38).Should().Be(true, "a null RecordKeyTwo property was provided");
        //    //result.IsDBNull(39).Should().Be(true, "a null RecordChild2 property was provided");
        //    //result.IsDBNull(40).Should().Be(true, "a null RecordChild2 property was provided");
        //    //result.IsDBNull(41).Should().Be(true, "a null RecordChild2 property was provided");
        //}

        [Fact]
        public void ValidateSqlDataReader()
        {
            var modelValues = new PgMapModel()
            {
                ArgentSeaTestDataId = 1,
                Name = "Test2",
                Iso3166 = "US",
                BigCount = 4,
                ValueCount = 5,
                SmallCount = 6,
                ByteCount = 7,
                TrueFalse = true,
                GuidValue = Guid.NewGuid(),
                GuidNull = Guid.NewGuid(),
                Birthday = new DateTime(2008, 8, 8),
                RightNow = new DateTime(2009, 9, 9),
                NowHere = new DateTime(2010, 10, 10),
                NowElsewhere = new DateTimeOffset(2011, 11, 11, 11, 11, 11, new TimeSpan(11, 11, 00)),
                WakeUp = new TimeSpan(12, 12, 12),
                Latitude = 13.13m,
                Longitude = 14.14,
                Altitude = 15.15f,
                Ratio = 16.1,
                Temperature = 32.1f,
                LongStory = "Once upon a time...",
                Numbers = new int[] { 4, 5, 6,7 },
                MissingBits = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 19 },
                Price = 21.0m,
                EnvTarget = EnvironmentVariableTarget.User,
                Color = ConsoleColor.Blue,
                Modifier = ConsoleModifiers.Control,
                KeyValues = new Dictionary<string, string>() { { "one", "1" }, { "two", "2" } },
                CleanOutStuff = new TimeSpan(4,5,6),
                GarbageCollectorNotificationStatus = GCNotificationStatus.NotApplicable,
                RecordKey = new Nullable<ShardKey<int>>(new ShardKey<int>('x', (short)2, 1234)),
                RecordChild = new ShardKey<int, short>('y', (short)3, 4567, (short)-23456),
                DataShard2 = new ShardKey<long>('A', (short)32, -1234L),
                ChildShard2 = new ShardKey<int, string>('B', (short)3, -4567, "testing...")
            };

            var rdr = Substitute.For<System.Data.Common.DbDataReader>();
            rdr.NextResult().Returns(false);
            rdr.Read().Returns(true, false);
            rdr.IsClosed.Returns(false);
            rdr.HasRows.Returns(true);

            rdr.FieldCount.Returns(39); //SET THIS IF YOU ADD COLUMNS!

            rdr.GetFieldValue<int>(0).Returns(modelValues.ArgentSeaTestDataId);
            rdr.GetFieldValue<string>(1).Returns(modelValues.Name);
            rdr.GetString(1).Returns(modelValues.Name);
            rdr.GetFieldValue<string>(2).Returns(modelValues.Iso3166);
            rdr.GetString(2).Returns(modelValues.Iso3166);
            rdr.GetFieldValue<long>(3).Returns(modelValues.BigCount.Value);
            rdr.GetFieldValue<int>(4).Returns(modelValues.ValueCount.Value);
            rdr.GetFieldValue<short>(5).Returns(modelValues.SmallCount.Value);
            rdr.GetFieldValue<byte>(6).Returns(modelValues.ByteCount.Value);
            rdr.GetFieldValue<bool>(7).Returns(modelValues.TrueFalse.Value);
            rdr.GetFieldValue<Guid>(8).Returns(modelValues.GuidValue);
            rdr.GetFieldValue<Guid>(9).Returns(modelValues.GuidNull.Value);
            rdr.GetFieldValue<DateTime>(10).Returns(modelValues.Birthday.Value);
            rdr.GetFieldValue<DateTime>(11).Returns(modelValues.RightNow.Value);
            rdr.GetFieldValue<DateTimeOffset>(12).Returns(modelValues.NowHere.Value);
            rdr.GetFieldValue<DateTimeOffset>(13).Returns(modelValues.NowElsewhere.Value);
            rdr.GetFieldValue<TimeSpan>(14).Returns(modelValues.WakeUp.Value);
            rdr.GetFieldValue<decimal>(15).Returns(modelValues.Latitude.Value);
            rdr.GetFieldValue<double>(16).Returns(modelValues.Longitude);
            rdr.GetFieldValue<float>(17).Returns(modelValues.Altitude);
            rdr.GetFieldValue<double>(18).Returns(modelValues.Ratio.Value);
            rdr.GetFieldValue<float>(19).Returns(modelValues.Temperature.Value);
            rdr.GetFieldValue<string>(20).Returns(modelValues.LongStory);
            rdr.GetString(20).Returns(modelValues.LongStory);
            rdr.GetFieldValue<int[]>(21).Returns(modelValues.Numbers);
            rdr.GetString(21).Returns(modelValues.LongStory);
            rdr.GetFieldValue<byte[]>(22).Returns(modelValues.MissingBits);
            rdr.GetFieldValue<decimal>(23).Returns(modelValues.Price.Value);
            rdr.GetFieldValue<string>(24).Returns(modelValues.EnvTarget.ToString());
            rdr.GetString(24).Returns(modelValues.EnvTarget.ToString());
            rdr.GetFieldValue<short>(25).Returns((short)modelValues.Color);
            rdr.GetFieldValue<string>(26).Returns(modelValues.Modifier.ToString());
            rdr.GetString(26).Returns(modelValues.Modifier.ToString());
            rdr.GetFieldValue<Dictionary<string, string>>(27).Returns(modelValues.KeyValues);
            rdr.GetFieldValue<TimeSpan>(28).Returns(modelValues.CleanOutStuff);
            rdr.GetFieldValue<string>(29).Returns(modelValues.GarbageCollectorNotificationStatus.ToString());
            rdr.GetString(29).Returns(modelValues.GarbageCollectorNotificationStatus.ToString());
            rdr.GetFieldValue<short>(30).Returns(modelValues.RecordKey.Value.ShardId);
            rdr.GetFieldValue<int>(31).Returns(modelValues.RecordKey.Value.RecordId);
            rdr.GetFieldValue<short>(32).Returns(modelValues.RecordChild.ShardId);
            rdr.GetFieldValue<int>(33).Returns(modelValues.RecordChild.RecordId);
            rdr.GetFieldValue<short>(34).Returns(modelValues.RecordChild.ChildId);
            rdr.GetFieldValue<long>(35).Returns(modelValues.DataShard2.RecordId);
            rdr.GetFieldValue<short>(36).Returns(modelValues.ChildShard2.Value.ShardId);
            rdr.GetFieldValue<int>(37).Returns(modelValues.ChildShard2.Value.RecordId);
            rdr.GetFieldValue<string>(38).Returns(modelValues.ChildShard2.Value.ChildId);
            rdr.GetString(38).Returns(modelValues.ChildShard2.Value.ChildId);

            rdr.IsDBNull(0).Returns(false);
            rdr.IsDBNull(1).Returns(false);
            rdr.IsDBNull(2).Returns(false);
            rdr.IsDBNull(3).Returns(false);
            rdr.IsDBNull(4).Returns(false);
            rdr.IsDBNull(5).Returns(false);
            rdr.IsDBNull(6).Returns(false);
            rdr.IsDBNull(7).Returns(false);
            rdr.IsDBNull(8).Returns(false);
            rdr.IsDBNull(9).Returns(false);
            rdr.IsDBNull(10).Returns(false);
            rdr.IsDBNull(11).Returns(false);
            rdr.IsDBNull(12).Returns(false);
            rdr.IsDBNull(13).Returns(false);
            rdr.IsDBNull(14).Returns(false);
            rdr.IsDBNull(15).Returns(false);
            rdr.IsDBNull(16).Returns(false);
            rdr.IsDBNull(17).Returns(false);
            rdr.IsDBNull(18).Returns(false);
            rdr.IsDBNull(19).Returns(false);
            rdr.IsDBNull(20).Returns(false);
            rdr.IsDBNull(21).Returns(false);
            rdr.IsDBNull(22).Returns(false);
            rdr.IsDBNull(23).Returns(false);
            rdr.IsDBNull(24).Returns(false);
            rdr.IsDBNull(25).Returns(false);
            rdr.IsDBNull(26).Returns(false);
            rdr.IsDBNull(27).Returns(false);
            rdr.IsDBNull(28).Returns(false);
            rdr.IsDBNull(29).Returns(false);
            rdr.IsDBNull(30).Returns(false);
            rdr.IsDBNull(31).Returns(false);
            rdr.IsDBNull(32).Returns(false);
            rdr.IsDBNull(33).Returns(false);
            rdr.IsDBNull(34).Returns(false);
            rdr.IsDBNull(35).Returns(false);
            rdr.IsDBNull(36).Returns(false);
            rdr.IsDBNull(37).Returns(false);
            rdr.IsDBNull(38).Returns(false);
            rdr.GetName(0).Returns("ArgentSeaTestDataId");
            rdr.GetName(1).Returns("Name");
            rdr.GetName(2).Returns("Iso3166");
            rdr.GetName(3).Returns("BigCount");
            rdr.GetName(4).Returns("ValueCount");
            rdr.GetName(5).Returns("SmallCount");
            rdr.GetName(6).Returns("ByteValue");
            rdr.GetName(7).Returns("TrueFalse");
            rdr.GetName(8).Returns("GuidValue");
            rdr.GetName(9).Returns("GuidNull");
            rdr.GetName(10).Returns("Birthday");
            rdr.GetName(11).Returns("RightNow");
            rdr.GetName(12).Returns("NowHere");
            rdr.GetName(13).Returns("NowElsewhere");
            rdr.GetName(14).Returns("WakeUp");
            rdr.GetName(15).Returns("Latitude");
            rdr.GetName(16).Returns("Longitude");
            rdr.GetName(17).Returns("Altitude");
            rdr.GetName(18).Returns("Ratio");
            rdr.GetName(19).Returns("Temperature");
            rdr.GetName(20).Returns("LongStory");
            rdr.GetName(21).Returns("Numbers");
            rdr.GetName(22).Returns("MissingBits");
            rdr.GetName(23).Returns("Price");
            rdr.GetName(24).Returns("EnvironmentTarget");
            rdr.GetName(25).Returns("ConsoleColor");
            rdr.GetName(26).Returns("ConsoleModifiers");
            rdr.GetName(27).Returns("KeyValues");
            rdr.GetName(28).Returns("CleanOut");
            rdr.GetName(29).Returns("GCNotificationStatus");
            rdr.GetName(30).Returns("DataShard");
            rdr.GetName(31).Returns("DataRecordId");
            rdr.GetName(32).Returns("ChildShard");
            rdr.GetName(33).Returns("ParentRecordId");
            rdr.GetName(34).Returns("ChildRecordId");
            rdr.GetName(35).Returns("DataRecordId2");
            rdr.GetName(36).Returns("ChildShard2");
            rdr.GetName(37).Returns("ParentRecord2Id");
            rdr.GetName(38).Returns("ChildRecord2Id");

            var dbLogger = Substitute.For<Microsoft.Extensions.Logging.ILogger>();
            var resultList = Mapper.ToList<PgMapModel>(rdr, (short)32, dbLogger);
            var result = resultList[0];
            result.ArgentSeaTestDataId.Should().Be(modelValues.ArgentSeaTestDataId, "that is the source value");
            result.Name.Should().Be(modelValues.Name, "that is the source value");
            result.Iso3166.Should().Be(modelValues.Iso3166, "that is the source value");
            result.BigCount.Should().Be(modelValues.BigCount, "that is the source value");
            result.ValueCount.Should().Be(modelValues.ValueCount, "that is the source value");
            result.SmallCount.Should().Be(modelValues.SmallCount, "that is the source value");
            result.ByteCount.Should().Be(modelValues.ByteCount, "that is the source value");
            result.TrueFalse.Should().Be(modelValues.TrueFalse, "that is the source value");
            result.GuidValue.Should().Be(modelValues.GuidValue, "that is the source value");
            result.GuidNull.Should().Be(modelValues.GuidNull, "that is the source value");
            result.Birthday.Should().Be(modelValues.Birthday, "that is the source value");
            result.RightNow.Should().Be(modelValues.RightNow, "that is the source value");
            result.NowHere.Should().Be(modelValues.NowHere, "that is the source value");
            result.NowElsewhere.Should().Be(modelValues.NowElsewhere, "that is the source value");
            result.WakeUp.Should().Be(modelValues.WakeUp, "that is the source value");
            result.Latitude.Should().Be(modelValues.Latitude, "that is the source value");
            result.Longitude.Should().Be(modelValues.Longitude, "that is the source value");
            result.Altitude.Should().Be(modelValues.Altitude, "that is the source value");
            result.Ratio.Should().Be(modelValues.Ratio, "that is the source value");
            result.Temperature.Should().Be(modelValues.Temperature, "that is the source value");
            result.LongStory.Should().Be(modelValues.LongStory, "that is the source value");
            result.MissingBits.Length.Should().Be(modelValues.MissingBits.Length, "that is the source value");
            result.Price.Should().Be(modelValues.Price, "that is the source value");
            result.EnvTarget.Should().Be(modelValues.EnvTarget, "that is the source value");
            result.Color.Should().Be(modelValues.Color, "that is the source value");
            result.Modifier.Should().Be(modelValues.Modifier, "that is the source value");
            result.Numbers[0].Should().Be(modelValues.Numbers[0], "that is the source value");
            result.Numbers[1].Should().Be(modelValues.Numbers[1], "that is the source value");
            result.Numbers[2].Should().Be(modelValues.Numbers[2], "that is the source value");
            result.Numbers[3].Should().Be(modelValues.Numbers[3], "that is the source value");
            result.KeyValues["one"].Should().Be("1", "that is the source value");
            result.KeyValues["two"].Should().Be("2", "that is the source value");
            result.CleanOutStuff.Should().Be(modelValues.CleanOutStuff, "that is the source value");
            result.GarbageCollectorNotificationStatus.Should().Be(modelValues.GarbageCollectorNotificationStatus, "that is the source value");
            result.RecordKey.HasValue.Should().BeTrue("because the row has values");
            result.RecordKey.Value.Should().Be(modelValues.RecordKey, "because that is the source value");
            result.RecordChild.Should().Be(modelValues.RecordChild, "because that is the source value");
            result.DataShard2.ShardId.Should().Be((short)32, "because that is the shardId passed into the method call");
            result.DataShard2.RecordId.Should().Be(-1234L, "because that is the recordId data value");
            result.ChildShard2.Value.Should().Be(modelValues.ChildShard2, "because that is the source value");
        }
        [Fact]
        public void ValidateNullSqlDataReader()
        {
            var modelValues = new PgMapModel()
            {
                ArgentSeaTestDataId = 1,
                Name = null,
                Iso3166 = null,
                BigCount = null,
                ValueCount = null,
                SmallCount = null,
                ByteCount = null,
                TrueFalse = null,
                GuidValue = Guid.Empty,
                GuidNull = null,
                Birthday = null,
                RightNow = null,
                NowHere = null,
                NowElsewhere = null,
                WakeUp = null,
                Latitude = null,
                Longitude = double.NaN,
                Altitude = float.NaN,
                Temperature = null,
                Ratio = null,
                LongStory = null,
                Numbers = null,
                MissingBits = null,
                Price = null,
                EnvTarget = EnvironmentVariableTarget.Machine,
                Color = ConsoleColor.DarkMagenta,
                Modifier = ConsoleModifiers.Shift,
                KeyValues = null,
                CleanOutStuff = TimeSpan.Zero,
                GarbageCollectorNotificationStatus = GCNotificationStatus.Succeeded,
                RecordKey = ShardKey<int>.Empty,
                RecordChild = ShardKey<int, short>.Empty,
                DataShard2 = ShardKey<long>.Empty,
                ChildShard2 = ShardKey<int, string>.Empty
            };


            var rdr = Substitute.For<System.Data.Common.DbDataReader>();
            rdr.NextResult().Returns(false);
            rdr.Read().Returns(true, false);
            rdr.IsClosed.Returns(false);
            rdr.HasRows.Returns(true);
            rdr.FieldCount.Returns(39);

            rdr.GetFieldValue<int>(0).Returns(modelValues.ArgentSeaTestDataId);
            rdr.GetFieldValue<string>(24).Returns(modelValues.EnvTarget.ToString());
            rdr.GetString(24).Returns(modelValues.EnvTarget.ToString());
            rdr.GetFieldValue<short>(25).Returns((short)modelValues.Color);
            rdr.GetFieldValue<string>(26).Returns(modelValues.Modifier.ToString());
            rdr.GetString(26).Returns(modelValues.Modifier.ToString());
            rdr.GetFieldValue<TimeSpan>(28).Returns(modelValues.CleanOutStuff);

            rdr.IsDBNull(0).Returns(false);
            rdr.IsDBNull(1).Returns(true);
            rdr.IsDBNull(2).Returns(true);
            rdr.IsDBNull(3).Returns(true);
            rdr.IsDBNull(4).Returns(true);
            rdr.IsDBNull(5).Returns(true);
            rdr.IsDBNull(6).Returns(true);
            rdr.IsDBNull(7).Returns(true);
            rdr.IsDBNull(8).Returns(true);
            rdr.IsDBNull(9).Returns(true);
            rdr.IsDBNull(10).Returns(true);
            rdr.IsDBNull(11).Returns(true);
            rdr.IsDBNull(12).Returns(true);
            rdr.IsDBNull(13).Returns(true);
            rdr.IsDBNull(14).Returns(true);
            rdr.IsDBNull(15).Returns(true);
            rdr.IsDBNull(16).Returns(true);
            rdr.IsDBNull(17).Returns(true);
            rdr.IsDBNull(18).Returns(true);
            rdr.IsDBNull(19).Returns(true);
            rdr.IsDBNull(20).Returns(true);
            rdr.IsDBNull(21).Returns(true);
            rdr.IsDBNull(22).Returns(true);
            rdr.IsDBNull(23).Returns(true);
            rdr.IsDBNull(24).Returns(false);
            rdr.IsDBNull(25).Returns(false);
            rdr.IsDBNull(26).Returns(false);
            rdr.IsDBNull(27).Returns(true);
            rdr.IsDBNull(28).Returns(false);
            rdr.IsDBNull(29).Returns(true);
             rdr.IsDBNull(30).Returns(true);
            rdr.IsDBNull(31).Returns(true);
             rdr.IsDBNull(32).Returns(true);
            rdr.IsDBNull(33).Returns(true);
            rdr.IsDBNull(34).Returns(true);
            rdr.IsDBNull(35).Returns(true);
             rdr.IsDBNull(36).Returns(true);
            rdr.IsDBNull(37).Returns(true);
            rdr.IsDBNull(38).Returns(true);
            rdr.GetName(0).Returns("ArgentSeaTestDataId");
            rdr.GetName(1).Returns("Name");
            rdr.GetName(2).Returns("Iso3166");
            rdr.GetName(3).Returns("BigCount");
            rdr.GetName(4).Returns("ValueCount");
            rdr.GetName(5).Returns("SmallCount");
            rdr.GetName(6).Returns("ByteValue");
            rdr.GetName(7).Returns("TrueFalse");
            rdr.GetName(8).Returns("GuidValue");
            rdr.GetName(9).Returns("GuidNull");
            rdr.GetName(10).Returns("Birthday");
            rdr.GetName(11).Returns("RightNow");
            rdr.GetName(12).Returns("NowHere");
            rdr.GetName(13).Returns("NowElsewhere");
            rdr.GetName(14).Returns("WakeUp");
            rdr.GetName(15).Returns("Latitude");
            rdr.GetName(16).Returns("Longitude");
            rdr.GetName(17).Returns("Altitude");
            rdr.GetName(18).Returns("Ratio");
            rdr.GetName(19).Returns("Temperature");
            rdr.GetName(20).Returns("LongStory");
            rdr.GetName(21).Returns("Numbers");
            rdr.GetName(22).Returns("MissingBits");
            rdr.GetName(23).Returns("Price");
            rdr.GetName(24).Returns("EnvironmentTarget");
            rdr.GetName(25).Returns("ConsoleColor");
            rdr.GetName(26).Returns("ConsoleModifiers");
            rdr.GetName(27).Returns("KeyValues");
            rdr.GetName(28).Returns("CleanOut");
            rdr.GetName(29).Returns("GCNotificationStatus");
            rdr.GetName(30).Returns("DataShard");
            rdr.GetName(31).Returns("DataRecordId");
            rdr.GetName(32).Returns("ChildShard");
            rdr.GetName(33).Returns("ParentRecordId");
            rdr.GetName(34).Returns("ChildRecordId");
            rdr.GetName(35).Returns("DataRecordId2");
            rdr.GetName(36).Returns("ChildShard2");
            rdr.GetName(37).Returns("ParentRecord2Id");
            rdr.GetName(38).Returns("ChildRecord2Id");
            //rdr.GetFieldValue<short>(30).Returns((short)1);
            //rdr.GetFieldValue<short>(32).Returns((short)1);
            //rdr.GetFieldValue<short>(36).Returns((short)1);

            var dbLogger = Substitute.For<Microsoft.Extensions.Logging.ILogger>();
            
            var resultList = Mapper.ToList<PgMapModel>(rdr, 200, dbLogger);

            var result = resultList[0];
            result.ArgentSeaTestDataId.Should().Be(modelValues.ArgentSeaTestDataId, "that is the source value");
            result.Name.Should().BeNull("the reader value is DbNull");
            result.Iso3166.Should().BeNull("the reader value is DbNull");
            result.BigCount.Should().BeNull("the reader value is DbNull");
            result.ValueCount.Should().BeNull("the reader value is DbNull");
            result.SmallCount.Should().BeNull("the reader value is DbNull");
            result.ByteCount.Should().BeNull("the reader value is DbNull");
            result.TrueFalse.Should().BeNull("the reader value is DbNull");
            result.GuidValue.Should().Be(Guid.Empty, "the reader value is DbNull");
            result.GuidNull.Should().BeNull("the reader value is DbNull");
            result.Birthday.Should().BeNull("the reader value is DbNull");
            result.RightNow.Should().BeNull("the reader value is DbNull");
            result.NowHere.Should().BeNull("the reader value is DbNull");
            result.NowElsewhere.Should().BeNull("the reader value is DbNull");
            result.WakeUp.Should().BeNull("the reader value is DbNull");
            result.Latitude.Should().BeNull("the reader value is DbNull");
            double.IsNaN(result.Longitude).Should().BeTrue("the reader value is DbNull");
            float.IsNaN(result.Altitude).Should().BeTrue("the reader value is DbNull");
            result.Ratio.Should().BeNull("the reader value is DbNull");
            result.Temperature.Should().BeNull("the reader value is DbNull");
            result.LongStory.Should().BeNull("the reader value is DbNull");
            result.Numbers.Should().BeNull("the reader value is DbNull");
            result.MissingBits.Should().BeNull("the reader value is DbNull");
            result.Price.Should().BeNull("the reader value is DbNull");
            result.EnvTarget.Should().Be(modelValues.EnvTarget, "that is the source value");
            result.Color.Should().Be(modelValues.Color, "that is the source value");
            result.Modifier.Should().Be(modelValues.Modifier, "that is the source value");
            result.KeyValues.Should().BeNull("the reader value is DbNull");
            result.CleanOutStuff.Should().Be(modelValues.CleanOutStuff, "that is the source value");
            result.GarbageCollectorNotificationStatus.Should().BeNull("the reader value is DbNull");
            result.RecordKey.Should().BeNull("the input values are null");
            result.RecordChild.Should().Be(ShardKey<int, short>.Empty, "the result should be empty");
            result.DataShard2.Should().Be(ShardKey<long>.Empty, "the result should be empty");
            result.ChildShard2.Should().BeNull("the input values are null");
        }
    }
}