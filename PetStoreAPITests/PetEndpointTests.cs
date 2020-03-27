using APIClient;
using APIClient.Models;
using NUnit.Framework;
using PetStoreAPITests.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;

namespace PetStoreAPITests
{
    [Parallelizable(ParallelScope.Fixtures)]
    [TestFixture]
    public class PetEndpointTests : TestBase
    {
        private PetStoreApi BasicApi => new PetStoreApi(ClientConfig.GetBasicConfig(Protocol, BaseApiPath));
        private PetStoreApi ApiIgnoring400AndDeserializingErrors => new PetStoreApi(ClientConfig.GetConfigIgnoring4xxAndDeserializerError(Protocol, BaseApiPath));

        #region Basic CRUD - happy path
        [Test]
        public void AddPetReturns200()
        {
            var api = BasicApi;

            api.AddPet(PetStoreDataGenerator.GetBasicPet());

            Assert.AreEqual(HttpStatusCode.OK, api.LastResponse.StatusCode, "Status code inconsistent with specification");
        }

        [Test]
        public void GetdPet()
        {
            var pet = PetStoreDataGenerator.GetBasicPet();

            BasicApi.AddPet(pet);

            Assert.AreEqual(pet, BasicApi.GetPet((long)pet.Id), "Pet read is not equal to pet created");
        }

        [Test]
        public void GetdPetsByStatus()
        {
            var petsWithDifferentStatuses = new[] { PetStatus.Available, PetStatus.Pending, PetStatus.Sold}.Select(x => PetStoreDataGenerator.GetBasicPet(x)); // To make sure at least one of each status is present in DB
            foreach (var pet in petsWithDifferentStatuses)
                BasicApi.AddPet(pet);

            foreach(var status in petsWithDifferentStatuses.Select(x => x.Status))
            {
                var apiIgnoringErrors = new PetStoreApi(ClientConfig.GetConfigIgnoringMissingRequiredFields(Protocol, BaseApiPath)); // Due to bug service accepts and then returns pet objects with required fields missing, but that's other kind of problem so we're ignoring it here and just checking statuses
                var returnedPets = apiIgnoringErrors.GetPets((PetStatus)status);

                Assert.That(returnedPets.Any());
                Assert.That(returnedPets.All(x => x.Status == status));
            }
        }

        [Test]
        public void UpdatePet()
        {
            var pet = PetStoreDataGenerator.GetBasicPet();
            pet.PhotoUrls = new List<string>() { "url1, url2, url3" };
            BasicApi.AddPet(pet);
            var updatedPet = new Pet()
            {
                Id = pet.Id,
                Name = DateGenerationUtils.GetRandomAlphabeticString(7),
                Category = PetStoreDataGenerator.GetCategory(2),
                PhotoUrls = new List<string>() { "url4, url5" },
                Tags = new List<Tag>() { new Tag() { Id = 2, Name = "Herbivorous" } },
                Status = PetStatus.Sold
            };

            BasicApi.UpdatePet(updatedPet);

            Assert.AreEqual(updatedPet, BasicApi.GetPet((long)pet.Id), "Pet read is not equal to pet created");
        }

        [Test]
        public void DeletePet()
        {
            var pet = PetStoreDataGenerator.GetBasicPet();
            BasicApi.AddPet(pet);
            BasicApi.GetPet((long)pet.Id); // Implicitly verifying it returns 2**

            BasicApi.DeletePet((long)pet.Id);

            var api = ApiIgnoring400AndDeserializingErrors;
            Assert.AreEqual(null, api.GetPet((long)pet.Id), "Service returned pet, when it should be deleted");
            Assert.AreEqual(HttpStatusCode.NotFound, api.LastResponse.StatusCode, "Getting deleted pet didn't return 404");
        }

        #endregion
    }
}
