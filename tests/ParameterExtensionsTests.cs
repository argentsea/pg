using System;
using Xunit;
using ArgentSea;
using ArgentSea.Pg;
using FluentAssertions;
using Npgsql;
using NpgsqlTypes;

namespace ArgentSea.Pg.Test
{
    public class ParameterExtensionsTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public void IntegerTests(int value)
        {
            var prms = new QueryParameterCollection();
            prms.AddPgIntegerInputParameter("1", value);
            prms.AddPgIntegerInputParameter("2", (int?)value);
            prms["1"].GetInteger().Should().Be(value, "that was the integer value");
            prms["2"].GetNullableInteger().Should().Be(value, "that was the integer value");
        }
        [Fact]
        public void NullIntegerTest()
        {
            var prm = new NpgsqlParameter("", NpgsqlDbType.Integer) { Value = System.DBNull.Value };
            prm.GetNullableInteger().Should().Be(null, "that was null database integer value");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(short.MaxValue)]
        [InlineData(short.MinValue)]
        public void ShortTests(short value)
        {
            var prms = new QueryParameterCollection();
            prms.AddPgSmallintInputParameter("1", value);
            prms.AddPgSmallintInputParameter("2", (short?)value);
            prms["1"].GetShort().Should().Be(value, "that was the short value");
            prms["2"].GetNullableShort().Should().Be(value, "that was the short value");
        }
        [Fact]
        public void NullShortTest()
        {
            var prm = new NpgsqlParameter("", NpgsqlDbType.Smallint) { Value = System.DBNull.Value };
            prm.GetNullableShort().Should().Be(null, "that was null database short value");
        }
        [Theory]
        [InlineData(0)]
        [InlineData(long.MaxValue)]
        [InlineData(long.MinValue)]
        public void LongTests(long value)
        {
            var prms = new QueryParameterCollection();
            prms.AddPgBigintInputParameter("1", value);
            prms.AddPgBigintInputParameter("2", (long?)value);
            prms["1"].GetLong().Should().Be(value, "that was the long value");
            prms["2"].GetNullableLong().Should().Be(value, "that was the long value");
        }
        [Fact]
        public void NullLongTest()
        {
            var prm = new NpgsqlParameter("", NpgsqlDbType.Bigint) { Value = System.DBNull.Value };
            prm.GetNullableLong().Should().Be(null, "that was null database long value");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(byte.MaxValue)]
        public void ByteTests(byte value)
        {
            var prms = new QueryParameterCollection();
            prms.AddPgInternalCharInputParameter("1", value);
            prms.AddPgInternalCharInputParameter("2", (byte?)value);
            prms["1"].GetByte().Should().Be(value, "that was the byte value");
            prms["2"].GetNullableByte().Should().Be(value, "that was the byte value");
        }
        [Fact]
        public void NullByteTest()
        {
            var prm = new NpgsqlParameter("", NpgsqlDbType.InternalChar) { Value = System.DBNull.Value };
            prm.GetNullableByte().Should().Be(null, "that was null database byte value");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void BooleanTests(bool value)
        {
            var prms = new QueryParameterCollection();
            prms.AddPgBooleanInputParameter("1", value);
            prms.AddPgBooleanInputParameter("2", (bool?)value);
            prms["1"].GetBoolean().Should().Be(value, "that was the boolean value");
            prms["2"].GetNullableBoolean().Should().Be(value, "that was the boolean value");
        }
        [Fact]
        public void NullBooleanTest()
        {
            var prm = new NpgsqlParameter("", NpgsqlDbType.Boolean) { Value = System.DBNull.Value };
            prm.GetNullableBoolean().Should().Be(null, "that was null database boolean value");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(double.MaxValue)]
        [InlineData(double.MinValue)]
        public void DoubleTests(double value)
        {
            var prms = new QueryParameterCollection();
            prms.AddPgDoubleInputParameter("1", value);
            prms.AddPgDoubleInputParameter("2", (double?)value);
            prms["1"].GetDouble().Should().Be(value, "that was the double value");
            prms["2"].GetNullableDouble().Should().Be(value, "that was the double value");
        }
        [Fact]
        public void NullDoubleTest()
        {
            var prm = new NpgsqlParameter("", NpgsqlDbType.Double) { Value = System.DBNull.Value };
            prm.GetNullableDouble().Should().Be(null, "that was a null database double value");
            double.IsNaN(prm.GetDouble()).Should().Be(true, "that was a null database double value");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(float.MaxValue)]
        [InlineData(float.MinValue)]
        public void FloatTests(float value)
        {
            var prms = new QueryParameterCollection();
            prms.AddPgRealInputParameter("1", value);
            prms.AddPgRealInputParameter("2", (float?)value);
            prms["1"].GetFloat().Should().Be(value, "that was the single value");
            prms["2"].GetNullableFloat().Should().Be(value, "that was the single value");
        }
        [Fact]
        public void NullFloatTest()
        {
            var prm = new NpgsqlParameter("", NpgsqlDbType.Real) { Value = System.DBNull.Value };
            prm.GetNullableFloat().Should().Be(null, "that was null database single value");
            float.IsNaN(prm.GetFloat()).Should().Be(true, "that was a null database float value");
        }

        [Theory]
        [InlineData(null, -1)]
        [InlineData("", 4)]
        [InlineData("Test", 25)]
        public void VarCharTests(string value, int length)
        {
            var prms = new QueryParameterCollection();
            prms.AddPgVarcharInputParameter("1", value, length);
            if (value is null)
            {
                prms["1"].Value.Should().Be(System.DBNull.Value, "null should save as dbNull");
            }
            prms["1"].GetString().Should().Be(value, "that was the string value");
        }
        [Theory]
        [InlineData(null, -1)]
        [InlineData("", 4)]
        [InlineData("Test", 25)]
        public void CharTests(string value, int length)
        {
            var prms = new QueryParameterCollection();
            prms.AddPgCharInputParameter("1", value, length);
            if (value is null)
            {
                prms["1"].Value.Should().Be(System.DBNull.Value, "null should save as dbNull");
            }
            //prms["1"].GetString().Should().Be(value?.PadRight(length, ' '), "that was the string value");
            prms["1"].GetString().Should().Be(value, "that was the string value");
        }
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("Test")]
        public void TextTests(string value)
        {
            var prms = new QueryParameterCollection();
            prms.AddPgTextInputParameter("1", value);
            if (value is null)
            {
                prms["1"].Value.Should().Be(System.DBNull.Value, "null should save as dbNull");
            }
            prms["1"].GetString().Should().Be(value, "that was the string value");
        }

        public static TheoryData<byte[], int> BinaryTestData => new TheoryData<byte[], int>
        {
            { null, 5 },
            { new byte[] { 1, 2, 3 }, 50 },
            { new byte[] { 0 }, 1 }
        };

        [Theory]
        [MemberData(nameof(BinaryTestData))]
        public void BinaryTests(byte[] value, int length)
        {
            var prms = new QueryParameterCollection();
            prms.AddPgByteaInputParameter("1", value, length);
            prms["1"].Size.Should().Be(length, "that was the specified length");
            if (value is null)
            {
                prms["1"].Value.Should().Be(System.DBNull.Value, "null should save as dbNull");
                prms["1"].GetBytes().Should().BeNull("null was specified");
            }
            else
            {
                var data = prms["1"].GetBytes();
                for (var i = 0; i < data.Length; i++)
                {
                    data[i].Should().Be(value[i], "that was the array value");
                }
            }
        }

        public static TheoryData<DateTime> DateTimeTestData => new TheoryData<DateTime>
        {
            { DateTime.UtcNow },
            { DateTime.Now },
            { DateTime.MinValue },
            { DateTime.MaxValue }
        };

        [Theory]
        [MemberData(nameof(DateTimeTestData))]
        public void TimestampTests(DateTime value)
        {
            var prms = new QueryParameterCollection();
            prms.AddPgTimestampInputParameter("1", value);
            prms.AddPgTimestampInputParameter("2", (DateTime?)value);
            prms["1"].GetDateTime().Should().Be(value, "that was the datetime value");
            prms["2"].GetNullableDateTime().Should().Be(value, "that was the datetime value");
        }
        [Fact]
        public void NullDateTimeTest()
        {
            var prm = new NpgsqlParameter("", NpgsqlDbType.Timestamp) { Value = System.DBNull.Value };
            prm.GetNullableDateTime().Should().Be(null, "that was null database datetime value");
        }

        [Theory]
        [MemberData(nameof(DateTimeTestData))]
        public void DateTests(DateTime value)
        {
            var prms = new QueryParameterCollection();
            prms.AddPgDateInputParameter("1", value);
            prms.AddPgDateInputParameter("2", (DateTime?)value);
            prms["1"].GetDateTime().Date.Should().Be(value.Date, "that was the date value");
            prms["2"].GetNullableDateTime().Value.Date.Should().Be(value.Date, "that was the date value");
        }

        [Theory]
        [MemberData(nameof(DateTimeTestData))]
        public void TimeTests(DateTime value)
        {
            var prms = new QueryParameterCollection();
            prms.AddPgTimeInputParameter("1", value.TimeOfDay);
            prms.AddPgTimeInputParameter("2", (TimeSpan?)value.TimeOfDay);
            prms["1"].GetTimeSpan().Should().Be(value.TimeOfDay, "that was the time value");
            prms["2"].GetNullableTimeSpan().Value.Should().Be(value.TimeOfDay, "that was the time value");
        }

        public static TheoryData<DateTimeOffset> DateTimeOffsetTestData => new TheoryData<DateTimeOffset>
        {
            { DateTimeOffset.Now },
            { DateTimeOffset.MinValue },
            { DateTimeOffset.MaxValue },
            { new DateTimeOffset(2018, 6, 10, 4, 22, 34, new TimeSpan(-4, 0, 0)) },
            { new DateTimeOffset(new DateTime(DateTime.Now.Ticks, DateTimeKind.Unspecified), new TimeSpan(-4, 0, 0)) }
        };

        [Theory]
        [MemberData(nameof(DateTimeOffsetTestData))]
        public void TimestampTzTests(DateTimeOffset value)
        {
            var prms = new QueryParameterCollection();
            prms.AddPgTimestampTzInputParameter("1", value);
            prms.AddPgTimestampTzInputParameter("2", (DateTimeOffset?)value);
            prms["1"].GetDateTimeOffset().Should().Be(value, "that was the datetime offset value");
            prms["2"].GetNullableDateTimeOffset().Value.Should().Be(value, "that was the datetime offset value");
        }

        [Theory]
        [MemberData(nameof(DateTimeOffsetTestData))]
        public void IntervalTests(DateTimeOffset value)
        {
            var prms = new QueryParameterCollection();
            prms.AddPgIntervalInputParameter("1", value.TimeOfDay);
            prms.AddPgIntervalInputParameter("2", (TimeSpan?)value.TimeOfDay);
            prms["1"].GetTimeSpan().Should().Be(value.TimeOfDay, "that was the timespan value");
            prms["2"].GetNullableTimeSpan().Value.Should().Be(value.TimeOfDay, "that was the timespan value");
        }

        [Fact]
        public void NullDateTimeOffsetTest()
        {
            var prm = new NpgsqlParameter("", NpgsqlDbType.TimestampTz) { Value = System.DBNull.Value };
            prm.GetNullableDateTimeOffset().Should().Be(null, "that was null database datetime offset value");
        }

        [Theory]
        [MemberData(nameof(DateTimeOffsetTestData))]
        public void TimeTzTests(DateTimeOffset value)
        {
            var prms = new QueryParameterCollection();
            prms.AddPgTimeTzInputParameter("1", value);
            prms.AddPgTimeTzInputParameter("2", (DateTimeOffset?)value);
            prms["1"].GetDateTimeOffset().Should().Be(value, "that was the time value");
            prms["2"].GetNullableDateTimeOffset().Value.Should().Be(value, "that was the timezone value");

        }

        public static TheoryData<decimal, byte, byte> DecimalTestData => new TheoryData<decimal, byte, byte>
        {
            { 0M, 9, 2 },
            { decimal.MaxValue, 18, 0 },
            { decimal.MinValue, 22, 4 }
        };

        [Theory]
        [MemberData(nameof(DecimalTestData))]
        public void DecimalTests(decimal value, byte precision, byte scale)
        {
            var prms = new QueryParameterCollection();
            prms.AddPgNumericInputParameter("1", value, precision, scale);
            prms.AddPgNumericInputParameter("2", (decimal?)value, precision, scale);
            prms["1"].GetDecimal().Should().Be(value, "that was the decimal value");
            prms["2"].GetNullableDecimal().Should().Be(value, "that was the decimal value");
            prms["1"].Precision.Should().Be(precision, "that was the specified precision");
            prms["2"].Scale.Should().Be(scale, "that was the specified scale");

            if (value >= -92233720368547758.08M && value >= 92233720368547758.07M)
            {
                prms.AddPgMoneyInputParameter("3", value);
                prms.AddPgMoneyInputParameter("4", (decimal?)value);
                prms["3"].GetDecimal().Should().Be(value, "that was the decimal value");
                prms["4"].GetNullableDecimal().Should().Be(value, "that was the decimal value");
            }
        }
        [Fact]
        public void NullDecimalTest()
        {
            var prm = new NpgsqlParameter("", NpgsqlDbType.Numeric) { Value = System.DBNull.Value };
            prm.GetNullableDecimal().Should().Be(null, "that was null database decimal value");
        }
        public static TheoryData<Guid> GuidTestData => new TheoryData<Guid>
        {
            { Guid.NewGuid() }
        };
        [Theory]
        [MemberData(nameof(GuidTestData))]
        public void UuidTests(Guid value)
        {
            var prms = new QueryParameterCollection();
            prms.AddPgUuidInputParameter("1", value);
            prms.AddPgUuidInputParameter("2", (Guid?)value);
            prms["1"].GetGuid().Should().Be(value, "that was the guid value");
            prms["2"].GetNullableGuid().Should().Be(value, "that was the guid value");
        }
        [Fact]
        public void NullGuidTest()
        {
            var prm1 = new NpgsqlParameter("1", NpgsqlDbType.Uuid) { Value = System.DBNull.Value };
            prm1.GetNullableGuid().Should().BeNull("that was a null database guid value");
            prm1.GetGuid().Should().Be(Guid.Empty, "that was a null database guid value");
            var prm2 = new NpgsqlParameter("2", NpgsqlDbType.Uuid) { Value = Guid.Empty };
            prm2.GetGuid().Should().Be(Guid.Empty, "that was an empty database guid value");
            prm2.GetNullableGuid().Should().Be(Guid.Empty, "that was an empty database guid value");
        }
    }
}
