﻿namespace NetRpc.Contract;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = true)]
public class AllowNullValueAttribute : Attribute
{
    public AllowNullValueAttribute()
    {
    }
}