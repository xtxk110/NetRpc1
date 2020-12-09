﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NetRpc.Http.Client
{
    public static class ClientHelper
    {
        private static readonly JsonSerializerOptions JsOptions = new()
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = {new JsonStringEnumConverter(JsonNamingPolicy.CamelCase), new StreamConverter()}
        };

        public static object? ToDtoObject(this string str, Type t)
        {
            if (string.IsNullOrEmpty(str))
                return default;

            return JsonSerializer.Deserialize(str, t, JsOptions);
        }

        public static bool HasStream(this Type? t)
        {
            if (t == null)
                return false;

            if (t.IsStream())
                return true;

            var propertyInfos = t.GetProperties();
            return propertyInfos.Any(i => i.PropertyType.IsStream());
        }

        [return: NotNullIfNotNull("obj")]
        public static string? ToDtoJson<T>(this T obj, bool isRestful = false)
        {
            if (obj == null)
                return null;
            return JsonSerializer.Serialize(obj, JsOptions);
        }

        [return: NotNullIfNotNull("obj")]
        public static string? ToResultJson<T>(this T obj)
        {
            if (obj == null)
                return null;
            return JsonSerializer.Serialize(obj, new JsonSerializerOptions
            {
                WriteIndented = true,
                IgnoreNullValues = true
            });
        }
    }
}