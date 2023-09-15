using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NetRpc
{
    /// <summary>
    /// 数字序列化
    /// </summary>
    public class NumberConverter : JsonConverterFactory
    {
        /// <summary>
        /// CanConvert
        /// </summary>
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(long) || typeToConvert == typeof(ulong)
                || typeToConvert == typeof(long?) || typeToConvert == typeof(ulong?)
                || typeToConvert == typeof(int) || typeToConvert == typeof(uint)
                || typeToConvert == typeof(int?) || typeToConvert == typeof(uint?)
                || typeToConvert == typeof(short) || typeToConvert == typeof(ushort)
                || typeToConvert == typeof(short?) || typeToConvert == typeof(ushort?)
                || typeToConvert == typeof(byte) || typeToConvert == typeof(float)
                || typeToConvert == typeof(byte?) || typeToConvert == typeof(float?)
                || typeToConvert == typeof(decimal) || typeToConvert == typeof(double)
                || typeToConvert == typeof(decimal?) || typeToConvert == typeof(double?);
        }

        /// <summary>
        /// CreateConverter
        /// </summary>
        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            if (typeof(long) == typeToConvert || typeof(long?) == typeToConvert)
            {
                return typeToConvert == typeof(long) ? new Int64Converter() : new NullInt64Converter();
            }
            else if (typeof(ulong) == typeToConvert || typeof(ulong?) == typeToConvert)
            {
                return typeToConvert == typeof(ulong) ? new UInt64Converter() : new NullUInt64Converter();
            }
            else if (typeof(int) == typeToConvert || typeof(int?) == typeToConvert)
            {
                return typeof(int) == typeToConvert ? new Int32Converter() : new NullInt32Converter();
            }
            else if (typeof(uint) == typeToConvert || typeof(uint?) == typeToConvert)
            {
                return typeof(uint) == typeToConvert ? new UInt32Converter() : new NullUInt32Converter();
            }
            else if (typeof(short) == typeToConvert || typeof(short?) == typeToConvert)
            {
                return typeof(short) == typeToConvert ? new Int16Converter() : new NullInt16Converter();
            }
            else if (typeof(ushort) == typeToConvert || typeof(ushort?) == typeToConvert)
            {
                return typeof(ushort) == typeToConvert ? new UInt16Converter() : new NullUInt16Converter();
            }
            else if (typeof(byte) == typeToConvert || typeof(byte?) == typeToConvert)
            {
                return typeof(byte) == typeToConvert ? new UInt8Converter() : new NullUInt8Converter();
            }
            else if (typeof(sbyte) == typeToConvert || typeof(sbyte?) == typeToConvert)
            {
                return typeof(sbyte) == typeToConvert ? new Int8Converter() : new NullInt8Converter();
            }
            else if (typeof(float) == typeToConvert || typeof(float?) == typeToConvert)
            {
                return typeof(float) == typeToConvert ? new FloatConverter() : new NullFloatConverter();
            }
            else if (typeof(double) == typeToConvert || typeof(double?) == typeToConvert)
            {
                return typeof(double) == typeToConvert ? new DoubleConverter() : new NullDoubleConverter();
            }
            else if (typeof(decimal) == typeToConvert || typeof(decimal?) == typeToConvert)
            {
                return typeof(decimal) == typeToConvert ? new DecimalConverter() : new NullDecimalConverter();
            }

            return default;
        }

        #region converter

        /// <summary>
        /// Int64序列化
        /// </summary>
        private class Int64Converter : JsonConverter<long>
        {
            /// <inheritdoc />
            public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.Null)
                {
                    return default;
                }
                else if (reader.TokenType == JsonTokenType.String)
                {
                    if (long.TryParse(reader.GetString(), out var value))
                    {
                        return value;
                    }

                    return default;
                }

                return reader.GetInt64();
            }

            /// <inheritdoc />
            public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(value.ToString());
            }
        }

        /// <summary>
        /// Int64序列化
        /// </summary>
        private class NullInt64Converter : JsonConverter<long?>
        {
            /// <inheritdoc />
            public override long? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.Null)
                {
                    return default;
                }
                else if (reader.TokenType == JsonTokenType.String)
                {
                    if (long.TryParse(reader.GetString(), out var value))
                    {
                        return value;
                    }

                    return default;
                }

                return reader.GetInt64();
            }

            /// <inheritdoc />
            public override void Write(Utf8JsonWriter writer, long? value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(value?.ToString());
            }
        }

        /// <summary>
        /// UInt64序列化
        /// </summary>
        private class NullUInt64Converter : JsonConverter<ulong?>
        {
            /// <inheritdoc />
            public override ulong? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.Null)
                {
                    return default;
                }
                else if (reader.TokenType == JsonTokenType.String)
                {
                    if (ulong.TryParse(reader.GetString(), out var value))
                    {
                        return value;
                    }

                    return default;
                }

                return reader.GetUInt64();
            }

            /// <inheritdoc />
            public override void Write(Utf8JsonWriter writer, ulong? value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(value?.ToString());
            }
        }

        /// <summary>
        /// UInt64序列化
        /// </summary>
        private class UInt64Converter : JsonConverter<ulong>
        {
            /// <inheritdoc />
            public override ulong Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.Null)
                {
                    return default;
                }
                else if (reader.TokenType == JsonTokenType.String)
                {
                    if (ulong.TryParse(reader.GetString(), out var value))
                    {
                        return value;
                    }

                    return default;
                }

                return reader.GetUInt64();
            }

            /// <inheritdoc />
            public override void Write(Utf8JsonWriter writer, ulong value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(value.ToString());
            }
        }

        /// <summary>
        /// Int32序列化
        /// </summary>
        private class Int32Converter : JsonConverter<int>
        {
            /// <inheritdoc />
            public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.Null)
                {
                    return default;
                }
                else if (reader.TokenType == JsonTokenType.String)
                {
                    if (int.TryParse(reader.GetString(), out var value))
                    {
                        return value;
                    }

                    return default;
                }

                return reader.GetInt32();
            }

            /// <inheritdoc />
            public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
            {
                writer.WriteNumberValue(value);
            }
        }

        /// <summary>
        /// Int32序列化
        /// </summary>
        private class NullInt32Converter : JsonConverter<int?>
        {
            /// <inheritdoc />
            public override int? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.Null)
                {
                    return default;
                }
                else if (reader.TokenType == JsonTokenType.String)
                {
                    if (int.TryParse(reader.GetString(), out var value))
                    {
                        return value;
                    }

                    return default;
                }

                return reader.GetInt32();
            }

            /// <inheritdoc />
            public override void Write(Utf8JsonWriter writer, int? value, JsonSerializerOptions options)
            {
                if (value.HasValue)
                    writer.WriteNumberValue(value.Value);
                else
                    writer.WriteNullValue();
            }
        }

        /// <summary>
        /// UInt32序列化
        /// </summary>
        private class UInt32Converter : JsonConverter<uint>
        {
            /// <inheritdoc />
            public override uint Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.Null)
                {
                    return default;
                }
                else if (reader.TokenType == JsonTokenType.String)
                {
                    if (uint.TryParse(reader.GetString(), out var value))
                    {
                        return value;
                    }

                    return default;
                }

                return reader.GetUInt32();
            }

            /// <inheritdoc />
            public override void Write(Utf8JsonWriter writer, uint value, JsonSerializerOptions options)
            {
                writer.WriteNumberValue(value);
            }
        }

        /// <summary>
        /// UInt32序列化
        /// </summary>
        private class NullUInt32Converter : JsonConverter<uint?>
        {
            /// <inheritdoc />
            public override uint? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.Null)
                {
                    return default;
                }
                else if (reader.TokenType == JsonTokenType.String)
                {
                    if (uint.TryParse(reader.GetString(), out var value))
                    {
                        return value;
                    }

                    return default;
                }

                return reader.GetUInt32();
            }

            /// <inheritdoc />
            public override void Write(Utf8JsonWriter writer, uint? value, JsonSerializerOptions options)
            {
                if (value.HasValue)
                    writer.WriteNumberValue(value.Value);
                else
                    writer.WriteNullValue();
            }
        }

        /// <summary>
        /// Int16序列化
        /// </summary>
        private class Int16Converter : JsonConverter<short>
        {
            /// <inheritdoc />
            public override short Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.Null)
                {
                    return default;
                }
                else if (reader.TokenType == JsonTokenType.String)
                {
                    if (short.TryParse(reader.GetString(), out var value))
                    {
                        return value;
                    }

                    return default;
                }

                return reader.GetInt16();
            }

            /// <inheritdoc />
            public override void Write(Utf8JsonWriter writer, short value, JsonSerializerOptions options)
            {
                writer.WriteNumberValue(value);
            }
        }

        /// <summary>
        /// Int16序列化
        /// </summary>
        private class NullInt16Converter : JsonConverter<short?>
        {
            /// <inheritdoc />
            public override short? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.Null)
                {
                    return default;
                }
                else if (reader.TokenType == JsonTokenType.String)
                {
                    if (short.TryParse(reader.GetString(), out var value))
                    {
                        return value;
                    }

                    return default;
                }

                return reader.GetInt16();
            }

            /// <inheritdoc />
            public override void Write(Utf8JsonWriter writer, short? value, JsonSerializerOptions options)
            {
                if (value.HasValue)
                    writer.WriteNumberValue(value.Value);
                else
                    writer.WriteNullValue();
            }
        }

        /// <summary>
        /// UInt16序列化
        /// </summary>
        private class UInt16Converter : JsonConverter<ushort>
        {
            /// <inheritdoc />
            public override ushort Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.Null)
                {
                    return default;
                }
                else if (reader.TokenType == JsonTokenType.String)
                {
                    if (ushort.TryParse(reader.GetString(), out var value))
                    {
                        return value;
                    }

                    return default;
                }

                return reader.GetUInt16();
            }

            /// <inheritdoc />
            public override void Write(Utf8JsonWriter writer, ushort value, JsonSerializerOptions options)
            {
                writer.WriteNumberValue(value);
            }
        }

        /// <summary>
        /// UInt16序列化
        /// </summary>
        private class NullUInt16Converter : JsonConverter<ushort?>
        {
            /// <inheritdoc />
            public override ushort? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.Null)
                {
                    return default;
                }
                else if (reader.TokenType == JsonTokenType.String)
                {
                    if (ushort.TryParse(reader.GetString(), out var value))
                    {
                        return value;
                    }

                    return default;
                }

                return reader.GetUInt16();
            }

            /// <inheritdoc />
            public override void Write(Utf8JsonWriter writer, ushort? value, JsonSerializerOptions options)
            {
                if (value.HasValue)
                    writer.WriteNumberValue(value.Value);
                else
                    writer.WriteNullValue();
            }
        }

        /// <summary>
        /// UInt8序列化
        /// </summary>
        private class UInt8Converter : JsonConverter<byte>
        {
            /// <inheritdoc />
            public override byte Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.Null)
                {
                    return default;
                }
                else if (reader.TokenType == JsonTokenType.String)
                {
                    if (byte.TryParse(reader.GetString(), out var value))
                    {
                        return value;
                    }

                    return default;
                }

                return reader.GetByte();
            }

            /// <inheritdoc />
            public override void Write(Utf8JsonWriter writer, byte value, JsonSerializerOptions options)
            {
                writer.WriteNumberValue(value);
            }
        }

        /// <summary>
        /// UInt8序列化
        /// </summary>
        private class NullUInt8Converter : JsonConverter<byte?>
        {
            /// <inheritdoc />
            public override byte? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.Null)
                {
                    return default;
                }
                else if (reader.TokenType == JsonTokenType.String)
                {
                    if (byte.TryParse(reader.GetString(), out var value))
                    {
                        return value;
                    }

                    return default;
                }

                return reader.GetByte();
            }

            /// <inheritdoc />
            public override void Write(Utf8JsonWriter writer, byte? value, JsonSerializerOptions options)
            {
                if (value.HasValue)
                    writer.WriteNumberValue(value.Value);
                else
                    writer.WriteNullValue();
            }
        }

        /// <summary>
        /// Int8序列化
        /// </summary>
        private class Int8Converter : JsonConverter<sbyte>
        {
            /// <inheritdoc />
            public override sbyte Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.Null)
                {
                    return default;
                }
                else if (reader.TokenType == JsonTokenType.String)
                {
                    if (sbyte.TryParse(reader.GetString(), out var value))
                    {
                        return value;
                    }

                    return default;
                }

                return reader.GetSByte();
            }

            /// <inheritdoc />
            public override void Write(Utf8JsonWriter writer, sbyte value, JsonSerializerOptions options)
            {
                writer.WriteNumberValue(value);
            }
        }

        /// <summary>
        /// Int8序列化
        /// </summary>
        private class NullInt8Converter : JsonConverter<sbyte?>
        {
            /// <inheritdoc />
            public override sbyte? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.Null)
                {
                    return default;
                }
                else if (reader.TokenType == JsonTokenType.String)
                {
                    if (sbyte.TryParse(reader.GetString(), out var value))
                    {
                        return value;
                    }

                    return default;
                }

                return reader.GetSByte();
            }

            /// <inheritdoc />
            public override void Write(Utf8JsonWriter writer, sbyte? value, JsonSerializerOptions options)
            {
                if (value.HasValue)
                    writer.WriteNumberValue(value.Value);
                else
                    writer.WriteNullValue();
            }
        }

        /// <summary>
        /// float序列化
        /// </summary>
        private class FloatConverter : JsonConverter<float>
        {
            /// <inheritdoc />
            public override float Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.Null)
                {
                    return 0;
                }
                else if (reader.TokenType == JsonTokenType.String)
                {
                    if (float.TryParse(reader.GetString(), out var value))
                    {
                        return value;
                    }

                    return 0;
                }

                return reader.GetSingle();
            }

            /// <inheritdoc />
            public override void Write(Utf8JsonWriter writer, float value, JsonSerializerOptions options)
            {
                writer.WriteNumberValue(value);
            }
        }

        /// <summary>
        /// float序列化
        /// </summary>
        private class NullFloatConverter : JsonConverter<float?>
        {
            /// <inheritdoc />
            public override float? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.Null)
                {
                    return default;
                }
                else if (reader.TokenType == JsonTokenType.String)
                {
                    if (float.TryParse(reader.GetString(), out var value))
                    {
                        return value;
                    }

                    return default;
                }

                return reader.GetSingle();
            }

            /// <inheritdoc />
            public override void Write(Utf8JsonWriter writer, float? value, JsonSerializerOptions options)
            {
                if (value.HasValue)
                    writer.WriteNumberValue(value.Value);
                else
                    writer.WriteNullValue();
            }
        }

        /// <summary>
        /// double序列化
        /// </summary>
        private class DoubleConverter : JsonConverter<double>
        {
            /// <inheritdoc />
            public override double Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.Null)
                {
                    return 0;
                }
                else if (reader.TokenType == JsonTokenType.String)
                {
                    if (double.TryParse(reader.GetString(), out var value))
                    {
                        return value;
                    }

                    return 0;
                }

                return reader.GetDouble();
            }

            /// <inheritdoc />
            public override void Write(Utf8JsonWriter writer, double value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(value.ToString());
            }
        }

        /// <summary>
        /// double序列化
        /// </summary>
        private class NullDoubleConverter : JsonConverter<double?>
        {
            /// <inheritdoc />
            public override double? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.Null)
                {
                    return default;
                }
                else if (reader.TokenType == JsonTokenType.String)
                {
                    if (double.TryParse(reader.GetString(), out var value))
                    {
                        return value;
                    }

                    return default;
                }

                return reader.GetDouble();
            }

            /// <inheritdoc />
            public override void Write(Utf8JsonWriter writer, double? value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(value?.ToString());
            }
        }

        /// <summary>
        /// decimal序列化
        /// </summary>
        private class DecimalConverter : JsonConverter<decimal>
        {
            /// <inheritdoc />
            public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.Null)
                {
                    return default;
                }
                else if (reader.TokenType == JsonTokenType.String)
                {
                    if (decimal.TryParse(reader.GetString(), out var value))
                    {
                        return value;
                    }

                    return default;
                }

                return reader.GetDecimal();
            }

            /// <inheritdoc />
            public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(value.ToString());
            }
        }

        /// <summary>
        /// decimal序列化
        /// </summary>
        private class NullDecimalConverter : JsonConverter<decimal?>
        {
            /// <inheritdoc />
            public override decimal? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.Null)
                {
                    return default;
                }
                else if (reader.TokenType == JsonTokenType.String)
                {
                    if (decimal.TryParse(reader.GetString(), out var value))
                    {
                        return value;
                    }

                    return default;
                }

                return reader.GetDecimal();
            }

            /// <inheritdoc />
            public override void Write(Utf8JsonWriter writer, decimal? value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(value?.ToString());
            }
        }

        #endregion
    }
}