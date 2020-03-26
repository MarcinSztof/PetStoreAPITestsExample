using APIClient;
using APIClient.Models;
using NUnit.Framework;
using PetStoreAPITests.Data;
using System;
using System.Net;

namespace PetStoreAPITests
{
    [TestFixture]
    public class PetEndpointTests
    {
        private PetStoreApi BasicApi => PetStoreApiClientFactory.GetBasicPetStoreApi();

        [Test]
        public void CreatePetReturns200()
        {
            var api = BasicApi;
            api.CreatePet(PetStoreDataGenerator.GetRandomPet());
            Assert.AreEqual(HttpStatusCode.OK, api.LastResponse.StatusCode, "Status code inconsisten with specification");
        }

        [Test]
        public void CreateAndReadPet()
        {
            var pet = PetStoreDataGenerator.GetRandomPet();
            BasicApi.CreatePet(pet);
            Assert.AreEqual(pet, BasicApi.GetPet((long)pet.Id));
        }
    }
}
