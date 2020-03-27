using APIClient.Models;
using NUnit.Framework;
using PetStoreAPITests.Data;
using System.Linq;
using System.Net;

namespace PetStoreAPITests
{
    [Parallelizable(ParallelScope.Fixtures)]
    [TestFixture]
    public class StoreEndpointsTests : PetStoreApiTestsBase
    {

        #region Basic CRUD - happy path
        // Adding order with pet id of non-existing pet is possible here. Without context of specific use case hard it's to tell whether that's a desired behaviour, so it will be accepted in following tests
        [Test]
        public void AddOrderReturns200()
        {
            var api = BasicApi;

            api.AddOrder(PetStoreDataGenerator.GetBasicOrder());

            Assert.AreEqual(HttpStatusCode.OK, api.LastResponse.StatusCode, "Status code inconsistent with specification");
        }

        [Test]
        public void GetdOrder()
        {
            var order = PetStoreDataGenerator.GetBasicOrder();

            BasicApi.AddOrder(order);

            var a = BasicApi.GetOrder((long)order.Id);
            Assert.AreEqual(order, a, "Order read is not equal to order created");
        }

        [Test]
        public void DeleteOrder()
        {
            var order = PetStoreDataGenerator.GetBasicOrder();
            BasicApi.AddOrder(order);
            BasicApi.GetOrder((long)order.Id); // Implicitly verifying if returns 2**

            BasicApi.DeleteOrder((long)order.Id);

            var api = ApiIgnoring400AndDeserializingErrors;
            Assert.That(api.GetOrder((long)order.Id).IsNullOrEmpty(), "Service returned pet, when it should be deleted");
            Assert.AreEqual(HttpStatusCode.NotFound, api.LastResponse.StatusCode, "Getting deleted pet didn't return 404");
        }

        [Test]
        public void GetInventoryByStatus()
        {
            var inventoriesDict = BasicApi.GetInventory(); // Implicit validation of contract through deserialization and checking if not null in the next step

            Assert.That(new[] { PetStatus.Available, PetStatus.Pending, PetStatus.Sold }.All(x => inventoriesDict.ContainsKey(x.ToString().ToLower())),
                        "Inventories for basic statuses weren't returned"); // Docs don't specify this, but make sense that it should return them
        }

        #endregion
    }

}
