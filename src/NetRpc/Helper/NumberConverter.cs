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
        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            if (typeof(long) == typeToConvert || typeof(long?) == typeToConvert)
            {
                return new Int64Converter();
            }
            else if (typeof(ulong) == typeToConvert || typeof(ulong?) == typeToConvert)
            {
                return new UInt64Converter();
            }
            else if (typeof(int) == typeToConvert || typeof(int?) == typeToConvert)
            {
                return new Int32Converter();
            }
            else if (typeof(uint) == typeToConvert || typeof(uint?) == typeToConvert)
            {
                return new UInt32Converter();
            }
            else if (typeof(short) == typeToConvert || typeof(short?) == typeToConvert)
            {
                return new Int16Converter();
            }
            else if (typeof(ushort) == typeToConvert || typeof(ushort?) == typeToConvert)
            {
                return new UInt16Converter();
            }
            else if (typeof(byte) == typeToConvert || typeof(byte?) == typeToConvert)
            {
                return new Int8Converter();
            }
            else if (typeof(float) == typeToConvert || typeof(float?) == typeToConvert)
            {
                return new FloatConverter();
            }
            else if (typeof(double) == typeToConvert || typeof(double?) == typeToConvert)
            {
                return new DoubleConverter();
            }
            else if (typeof(decimal) == typeToConvert || typeof(decimal?) == typeToConvert)
            {
                return new DecimalConverter();
            }

            return null;
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
                    return 0;
                }
                else if (reader.TokenType == JsonTokenType.String)
                {
                    if (long.TryParse(reader.GetString(), out var value))
                    {
                        return value;
                    }

                    return 0;
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
        /// UInt64序列化
        /// </summary>
        private class UInt64Converter : JsonConverter<ulong>
        {
            /// <inheritdoc />
            public override ulong Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.Null)
                {
                    return 0;
                }
                else if (reader.TokenType == JsonTokenType.String)
                {
                    if (ulong.TryParse(reader.GetString(), out var value))
                    {
                        return value;
                    }

                    return 0;
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
                    return 0;
                }
                else if (reader.TokenType == JsonTokenType.String)
                {
                    if (int.TryParse(reader.GetString(), out var value))
                    {
                        return value;
                    }

                    return 0;
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
        /// UInt32序列化
        /// </summary>
        private class UInt32Converter : JsonConverter<uint>
        {
            /// <inheritdoc />
            public override uint Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.Null)
                {
                    return 0;
                }
                else if (reader.TokenType == JsonTokenType.String)
                {
                    if (uint.TryParse(reader.GetString(), out var value))
                    {
                        return value;
                    }

                    return 0;
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
        /// Int16序列化
        /// </summary>
        private class Int16Converter : JsonConverter<short>
        {
            /// <inheritdoc />
            public override short Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.Null)
                {
                    return 0;
                }
                else if (reader.TokenType == JsonTokenType.String)
                {
                    if (short.TryParse(reader.GetString(), out var value))
                    {
                        return value;
                    }

                    return 0;
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
        /// UInt16序列化
        /// </summary>
        private class UInt16Converter : JsonConverter<ushort>
        {
            /// <inheritdoc />
            public override ushort Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.Null)
                {
                    return 0;
                }
                else if (reader.TokenType == JsonTokenType.String)
                {
                    if (ushort.TryParse(reader.GetString(), out var value))
                    {
                        return value;
                    }

                    return 0;
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
        /// Int8序列化
        /// </summary>
        private class Int8Converter : JsonConverter<byte>
        {
            /// <inheritdoc />
            public override byte Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.Null)
                {
                    return 0;
                }
                else if (reader.TokenType == JsonTokenType.String)
                {
                    if (byte.TryParse(reader.GetString(), out var value))
                    {
                        return value;
                    }

                    return 0;
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
                writer.WriteNumberValue(value);
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
                    return 0;
                }
                else if (reader.TokenType == JsonTokenType.String)
                {
                    if (decimal.TryParse(reader.GetString(), out var value))
                    {
                        return value;
                    }

                    return 0;
                }

                return reader.GetDecimal();
            }

            /// <inheritdoc />
            public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
            {
                writer.WriteNumberValue(value);
            }
        }

        #endregion
    }
}