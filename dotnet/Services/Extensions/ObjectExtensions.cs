using System;
using System.Collections.Generic;
namespace dotnet.Services.Extensions
{
    public static class ObjectExtentions
    {
        public static T Get<T>(this object obj,string property)
        {
            return (T)obj.GetType().GetProperty(property).GetValue(obj);
        }
    }
}