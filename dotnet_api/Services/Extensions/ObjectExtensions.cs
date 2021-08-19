using System;
using System.Collections.Generic;
namespace dotnetApi.Services.Extensions
{
    public static class ObjectExtentions
    {
        public static T Get<T>(this object obj,string property)
        {
            return (T)obj.GetType().GetProperty(property).GetValue(obj);
        }
    }
}