using System;
using System.Collections.Generic;
using System.Text;

namespace APIClient.Models
{
    public static class ModelExtensions
    {
        public static bool IsNullOrEmpty(this ModelBase obj) //here "empty" means all fields have default values
        {
            if (obj == null)
                return true;
            foreach (var prop in obj.GetType().GetProperties()) // TODO: Kinda hacky and not to pretty - I know. Every time we use new struct in Models this has to be updated. Need to find way to dynamically check if value type has its default value
            {
                var val = prop.GetValue(obj);
                if (val is ValueType)
                {
                    var isDefaultValueType = val.GetType().Name switch
                    {
                        "Int32" => (int)val == 0,
                        "Int64" => (long)val == 0,
                        // other needed here
                        "Boolean" => (bool)val == false,
                        "DateTime" => (DateTime)val == default(DateTime),
                        "DateTimeOffset" => (DateTimeOffset)val == default(DateTimeOffset),
                        _ => throw new Exception("Type not implemented")
                    };
                    if (!isDefaultValueType)
                        return false;
                }
                else
                {
                    if (val != null)
                        return false;
                }
            }
            return true;
        }
    }
}
