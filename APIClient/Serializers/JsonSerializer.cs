using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace APIClient.Serializers
{
    public class JsonSerializer : ISerializer
    {
        private JsonSerializerSettings _serializerSettings;
        private bool _throwOnFailure;

        // In real-life scenario just settings could be passed and error handling could be done with it. Here for siplicity it's done with additional flag.
        public JsonSerializer(bool throwOnFailure, JsonSerializerSettings settings = null)
        {
            _throwOnFailure = throwOnFailure;
            _serializerSettings = settings;
        }

        public T Deserialize<T>(string value)
        {
            Console.WriteLine("Attempting to deserialize json to " + (MethodBase.GetCurrentMethod() as MethodInfo)?.ReturnType.ToString());

            T obj = default(T);
            try
            {
                obj = JsonConvert.DeserializeObject<T>(value, _serializerSettings);
                Console.WriteLine("Deserialization success");
            }
            catch(JsonException)
            {
                Console.WriteLine("Exception hit when deserializing");
                if(_throwOnFailure)
                    throw;
                Console.WriteLine("Exception ignored, returning default");
            }
            return obj;
        }

        public string Serialize(object obj)
        {
            Console.WriteLine("Attempting to serialize json from" + obj.GetType());

            string val = null;
            try
            {
                val = JsonConvert.SerializeObject(obj);
                Console.WriteLine("Serialization success");
            }
            catch (JsonException)
            {
                Console.WriteLine("Exception hit when serializing");
                throw;
            }
            return val;
        }
    }
}
