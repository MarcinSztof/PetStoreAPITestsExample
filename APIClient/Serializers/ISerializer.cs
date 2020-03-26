using System;
using System.Collections.Generic;
using System.Text;

namespace APIClient.Serializers
{
    public interface ISerializer
    {
        T Deserialize<T>(string body);
        string Serialize(object body);
    }
}
