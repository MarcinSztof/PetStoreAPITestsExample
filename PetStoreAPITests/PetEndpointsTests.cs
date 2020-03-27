using APIClient;
using APIClient.Auth;
using APIClient.Models;
using NUnit.Framework;
using PetStoreAPITests.Data;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace PetStoreAPITests
{
    [Parallelizable(ParallelScope.Fixtures)]
    [TestFixture]
    public class PetEndpointsTests : PetStoreApiTestsBase
    {
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
            BasicApi.GetPet((long)pet.Id); // Implicitly verifying if returns 2**

            BasicApi.DeletePet((long)pet.Id);

            var api = ApiIgnoring400AndDeserializingErrors;
            Assert.AreEqual(null, api.GetPet((long)pet.Id), "Service returned pet, when it should be deleted");
            Assert.AreEqual(HttpStatusCode.NotFound, api.LastResponse.StatusCode, "Getting deleted pet didn't return 404");
        }

        #endregion

        #region Business logic, authentication and bug hunting

        //TODO: Here it would be nice to have a way to dynamically create collection of object based on given model but each with one of required fields missing, so when in real-life scenario testing with huge number of models with lots of required fileds, this kkind of collection doesn't have to be created by hand
        public static IEnumerable<object> GetPetsWithMissingRequiredFields
        {
            get
            {
                //Nunit random has to be used in data source
                yield return new { id = TestContext.CurrentContext.Random.Next(10000, 99999), category = new { id = 1234, name = "qwe" }, photoUrls = new List<string>(), tags = new List<Tag>(), status = "sold" };
                yield return new { id = TestContext.CurrentContext.Random.Next(10000, 99999), category = new { id = 1234, name = "qwe" }, name = "Hamster", tags = new List<Tag>(), status = "sold" };
            }
        }

        // Bug - object with missing required fields is accepted
        [TestCaseSource(nameof(GetPetsWithMissingRequiredFields))]
        public void AddingPetWithMissingRequireFieldIsNotAcceted(object obj)
        {
            var api = ApiIgnoring400AndDeserializingErrors;

            api.SendRequest(HttpMethod.Post, api.PetEndpoint, obj);

            Assert.AreEqual(HttpStatusCode.MethodNotAllowed, api.LastResponse.StatusCode);
        }

        //Bug - Should return 405 implying invalid input, returns 500
        [Test]
        public void AddingPetWithStringIdIsNotAccepted() // TODO: Again, with these kind of tests, in real-life scenario it would be good to cover it with tests using generated data, writing multiple tests like that checking multiple fields with multiple incorrect values isn't really feasible 
        {
            var pet = new { id = DateGenerationUtils.GetRandomAlphabeticString(7), category = new { id = 1234, name = "qwe" }, name = "Rat", photoUrls = new List<string>(), tags = new List<Tag>(), status = "sold" };
            var api = new PetStoreApi(ClientConfig.GetConfigIgnoringStatusAndDeserializerError(Protocol, BaseApiPath));

            api.SendRequest(HttpMethod.Post, api.PetEndpoint, pet);

            Assert.AreEqual(HttpStatusCode.MethodNotAllowed, api.LastResponse.StatusCode);
        }

        [Test]
        public void AddPetAuthenticatingWithApiKey() // Here as an example. Using differnt auth methods, to verify auth for each endpoint could be parametrized for all tests
        {
            var config = ClientConfig.GetBasicConfig(Protocol, BaseApiPath);
            config.AuthModule = new ApiKeyAuth(ApiKey);
            var api = new PetStoreApi(config);
            var pet = PetStoreDataGenerator.GetBasicPet();

            api.AddPet(pet);

            Assert.AreEqual(pet, api.GetPet((long)pet.Id), "Pet read is not equal to pet created");
        }

        // Hard to say if that's a bug - no auth is needed at all. But when providing invalid api key header, I'd expect a service to return error
        [Test]
        public void CannotAddPetAuthenticatingWithInvalidApiKey()
        {
            var config = ClientConfig.GetBasicConfig(Protocol, BaseApiPath);
            config.AuthModule = new ApiKeyAuth("invalidKey");
            config.AcceptedResponseStatuses = new[] { ResponseStatusCategory.Status2xx, ResponseStatusCategory.Status4xx };
            var api = new PetStoreApi(config);
            var pet = PetStoreDataGenerator.GetBasicPet();

            api.AddPet(pet);

            Assert.AreEqual(HttpStatusCode.Unauthorized, api.LastResponse.StatusCode, "Auth with invalid key succeeded");
        }

        #endregion
    }
}
