using APIClient;
using NUnit.Framework;

namespace PetStoreAPITests
{
    public class TestBase
    {
        protected Protocol Protocol { get; private set; }
        protected string BaseApiPath { get; private set; }

        [OneTimeSetUp]
        public void SetUp()
        {
            // In real life scenario, this would be passed here from local config file and api path possibly from some remote parameter store
            Protocol = Protocol.Http;
            BaseApiPath = "petstore.swagger.io/v2/";
        }
    }
}
