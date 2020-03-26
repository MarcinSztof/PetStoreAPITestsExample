using APIClient.Auth;
using APIClient.Serializers;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIClient
{
    public static class PetStoreApiClientFactory
    {
        private const string baseUrl = "http://petstore.swagger.io/v2/"; // Here hardcoded as const, but in real life scenario would be passed here
        public static PetStoreApi GetBasicPetStoreApi() =>
            new PetStoreApi(baseUrl, new NoAuthModule(), new JsonSerializer(true), new RequestBuilder("application/json", Encoding.UTF8), new ResponseStatusCategory[] { ResponseStatusCategory.Status2xx });
    }
}
