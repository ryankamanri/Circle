using System.Collections.Generic;
namespace dotnetApi.Services.Self
{
    public static class JsonDynamic
    {
        public static void FillSelections(Dictionary<string, object> options,dynamic selections)
        {
            foreach(var option in options)
            {
                if(option.Value.GetType() == typeof(Newtonsoft.Json.Linq.JArray))
                    ((IDictionary<string,object>)selections)[option.Key] = ((Newtonsoft.Json.Linq.JArray)option.Value).ToObject<IEnumerable<object>>();
                else ((IDictionary<string,object>)selections)[option.Key] = option.Value;
            }
        }
    }
}