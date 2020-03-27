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
        public readonly string PetEndpoint = "pet/";
        public string FindPetsByStatusEndpoint => PetEndpoint + "findByStatus";
        public readonly string StoreOrderEndpoint = "store/order/";
        public readonly string StoreInventoryEndpoint = "store/inventory";

        public PetStoreApi(ClientConfig config) : base(config)
        {
         
        }

        #region Pet endpoints
        public void AddPet(Pet pet) => SendRequest(HttpMethod.Post, PetEndpoint, pet); //Client responds with body containing added pet - this is undocumented and ignored here. Also successful creation of an object that can be retrieved depends on providing id with request, what would be rather expected of PUT request with which this one is actually interchangable. But here it's understandable given the nature of SUT, so it will be ignored.

        public Pet GetPet(long id) => SendRequest<Pet>(HttpMethod.Get, PetEndpoint + id);

        public ICollection<Pet> GetPets(PetStatus status) => SendRequest<ICollection<Pet>>(HttpMethod.Get, FindPetsByStatusEndpoint + $"?status={status.ToString().ToLower()}");

        public void UpdatePet(Pet pet) => SendRequest(HttpMethod.Put, PetEndpoint, pet);

        public void DeletePet(long id) => SendRequest(HttpMethod.Delete, PetEndpoint + id);
        #endregion

        #region Store endpoints
        public Order AddOrder(Order order) => SendRequest<Order>(HttpMethod.Post, StoreOrderEndpoint, order);

        public Order GetOrder(long id) => SendRequest<Order>(HttpMethod.Get, StoreOrderEndpoint + id); // Here have have a problem with response body being order if things go right and api response if not. We're returning just the order, for testing and debuging we can additionaly deserialize body from LastResponse

        public ApiResponse DeleteOrder(long id) => SendRequest<ApiResponse>(HttpMethod.Delete, StoreOrderEndpoint + id);

        public Dictionary<string, int> GetInventory() => SendRequest<Dictionary<string, int>>(HttpMethod.Get, StoreInventoryEndpoint);
        #endregion
    }
}
