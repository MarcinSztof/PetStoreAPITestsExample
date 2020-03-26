using APIClient.Auth;
using APIClient.Models;
using APIClient.Serializers;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace APIClient
{
    public class PetStoreApi : ApiClientBase
    {
        private const string petEndpoint = "pet/";
        public PetStoreApi(string url, IAuthModule auth, ISerializer serializer, RequestBuilder requestBuilder, ResponseStatusCategory[] acceptedResponseStatuses ) : base(url, auth, serializer, requestBuilder, acceptedResponseStatuses)
        {
         
        }

        public void CreatePet(Pet pet) => SendRequest(HttpMethod.Post, petEndpoint, pet);

        public Pet GetPet(long id) => SendRequest<Pet>(HttpMethod.Get, petEndpoint + id.ToString());
    }
}
