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
        private string findPetsByStatusEndpoint => petEndpoint + "findByStatus";
        public PetStoreApi(string url, IAuthModule auth, ISerializer serializer, RequestBuilder requestBuilder, ResponseStatusCategory[] acceptedResponseStatuses ) : base(url, auth, serializer, requestBuilder, acceptedResponseStatuses)
        {
         
        }

        public void AddPet(Pet pet) => SendRequest(HttpMethod.Post, petEndpoint, pet);

        public Pet GetPet(long id) => SendRequest<Pet>(HttpMethod.Get, petEndpoint + id);

        public ICollection<Pet> GetPets(PetStatus status) => SendRequest<ICollection<Pet>>(HttpMethod.Get, findPetsByStatusEndpoint + $"?status={status.ToString().ToLower()}");

        public void UpdatePet(Pet pet) => SendRequest(HttpMethod.Put, petEndpoint, pet);

        public void DeletePet(long id) => SendRequest(HttpMethod.Delete, petEndpoint + id);
    }
}
