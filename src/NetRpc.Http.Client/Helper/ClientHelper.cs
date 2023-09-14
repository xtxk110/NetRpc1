﻿using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NetRpc.Http.Client;

public static class ClientHelper
{
    private static readonly JsonSerializerOptions JsOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase), new StreamConverter(), new NumberConverter() }
    };

    private static readonly JsonSerializerOptions JsOptionsNotIndented = new()
    {
        WriteIndented = false,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase), new StreamConverter(), new NumberConverter() }
    };

    public static object? ToDtoObject(this string? str, Type t)
    {
        if (string.IsNullOrEmpty(str))
            return default;

        return JsonSerializer.Deserialize(str, t, JsOptions);
    }

    public static object? ToDtoObjectByNumber(this string? str, Type t)
    {
        if (string.IsNullOrEmpty(str))
            return default;

        return JsonSerializer.Deserialize(str, t, new JsonSerializerOptions
        {
            Converters =
            {
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase),
                new NumberConverter()
            }
        });
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
    public static string? ToDtoJson<T>(this T obj)
    {
        if (obj == null)
            return null;
        return JsonSerializer.Serialize(obj, JsOptions);
    }

    [return: NotNullIfNotNull("obj")]
    public static string? ToDtoJsonNotIndented<T>(this T obj)
    {
        if (obj == null)
            return null;
        return JsonSerializer.Serialize(obj, JsOptionsNotIndented);
    }
}