using APIClient;
using NUnit.Framework;

namespace PetStoreAPITests
{
    // In real-life scenario here probably also things like clearing DB or seeding default data would be done, any setup required for tests for this specific API 
    public class PetStoreApiTestsBase
    {
        protected Protocol Protocol { get; private set; }
        protected string BaseApiPath { get; private set; }
        protected string ApiKey { get; private set; }

        protected PetStoreApi BasicApi => new PetStoreApi(ClientConfig.GetBasicConfig(Protocol, BaseApiPath));
        protected PetStoreApi ApiIgnoring400AndDeserializingErrors => new PetStoreApi(ClientConfig.GetConfigIgnoring4xxAndDeserializerError(Protocol, BaseApiPath));

        [OneTimeSetUp]
        public void SetUp()
        {
            // In real life scenario, this would be passed here from local config file and api path possibly from some remote parameter store
            Protocol = Protocol.Http;
            BaseApiPath = "petstore.swagger.io/v2/";
            ApiKey = "special-key";
        }
    }
}
