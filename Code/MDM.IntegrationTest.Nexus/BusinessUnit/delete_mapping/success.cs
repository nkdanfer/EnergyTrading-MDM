namespace EnergyTrading.MDM.Test
{
    using System.Configuration;
    using System.Linq;
    using System.Net;

    using Microsoft.Http;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using EnergyTrading.Data.EntityFramework;
    using EnergyTrading.MDM.Data.EF.Configuration;

    [TestClass]
    public class when_a_request_is_made_to_delete_a_businessunit_mapping : IntegrationTestBase
    {
        private static HttpResponseMessage response;
        private static HttpClient client;
        private static MDM.BusinessUnit businessunit;

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            Establish_context();
            Because_of();
        }

        protected static void Establish_context()
        {
            businessunit = BusinessUnitData.CreateEntityWithTwoDetailsAndTwoMappings();
        }

        protected static void Because_of()
        {
            client = new HttpClient();
            var uri = ServiceUrl["BusinessUnit"] + businessunit.Id + "/Mapping/" + businessunit.Mappings[0].Id;
            response = client.Delete(uri);
        }

        [TestMethod]
        public void should_delete_the_mapping()
        {
            var dbBusinessUnit =
                new DbSetRepository<MDM.BusinessUnit>(new MappingContext()).FindOne(businessunit.Id);

            Assert.IsTrue(dbBusinessUnit.Mappings.Where(mapping => mapping.Id == businessunit.Mappings[0].Id).Count() == 0);
        }

        [TestMethod]
        public void should_leave_other_mappings_untouched()
        {
            var dbBusinessUnit =
                new DbSetRepository<MDM.BusinessUnit>(new MappingContext()).FindOne(businessunit.Id);

            Assert.AreEqual(1, dbBusinessUnit.Mappings.Count);
        }

        [TestMethod]
        public void should_return_status_ok()
        {
            response.StatusCode = HttpStatusCode.OK;
        }
    }

}
    