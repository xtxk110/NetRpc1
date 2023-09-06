using System.ComponentModel;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Xml.Linq;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using NetRpc.Contract;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NetRpc.Http;

/// <summary>
/// EnumSchemaFilter
/// </summary>
public class EnumSchemaFilter : ISchemaFilter
{
    /// <summary>
    /// Apply
    /// </summary>
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type.IsEnum)
        {
            schema.Enum.Clear();
            Enum.GetNames(context.Type).ToList()
                .ForEach(m =>
                {
                    var e = (Enum)Enum.Parse(context.Type, m);
                    schema.Enum.Add(new OpenApiString($"{Convert.ToInt64(e)}({e})：{GetDescription(context.Type, e)}"));
                });
        }
    }

    #region private methods
    /// <summary>
    /// 获取枚举描述
    /// </summary>
    /// <param name="t"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    private string GetDescription(Type t, object value)
    {
        foreach (MemberInfo mInfo in t.GetMembers())
        {
            if (mInfo.Name == t.GetEnumName(value))
                foreach (Attribute attr in Attribute.GetCustomAttributes(mInfo))
                    if (attr.GetType() == typeof(DescriptionAttribute))
                        return ((DescriptionAttribute)attr).Description;
        }

        return string.Empty;
    }
    #endregion
}