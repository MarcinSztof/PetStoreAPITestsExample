using Newtonsoft.Json;
using System;

namespace APIClient.Serializers
{
    public static class JsonSerializerSettingsFactory
    {
        public static JsonSerializerSettings GetSerializerIgnoringMissingRequiredFields() =>
            new JsonSerializerSettings
            {
                Error = (sender, errorEventArgs) =>
                {
                    if(errorEventArgs.ErrorContext.Error.Message.StartsWith("Required property"))
                    {
                        Console.WriteLine("Missing property exception hit - ignoring");
                        errorEventArgs.ErrorContext.Handled = true;
                    }
                }
            };
    }
}
