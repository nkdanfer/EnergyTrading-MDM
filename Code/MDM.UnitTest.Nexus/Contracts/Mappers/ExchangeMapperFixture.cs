namespace EnergyTrading.MDM.Test.Contracts.Mappers
{
    using System;

    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using EnergyTrading.Data;
    using EnergyTrading.MDM.Contracts.Mappers;
    using EnergyTrading;
    using EnergyTrading.Mapping;

    using RWEST.Nexus.MDM;
    using EnergyTrading.MDM.Extensions;

    [TestClass]
    public class ExchangeMapperFixture : Fixture
    {
        [TestMethod]
        public void Map()
        {
            // Arrange
            var start = new DateTime(2010, 1, 1);
            var end = DateUtility.MaxDate;
            var range = new DateRange(start, end);

            var id = new RWEST.Nexus.MDM.Contracts.NexusId { SystemName = "Test", Identifier = "A" };
            var contractDetails = new RWEST.Nexus.MDM.Contracts.ExchangeDetails();
            var contract = new RWEST.Nexus.MDM.Contracts.Exchange
            {
                Identifiers = new RWEST.Nexus.MDM.Contracts.NexusIdList { id },
                Details = contractDetails,
                Nexus = new RWEST.Nexus.MDM.Contracts.SystemData { StartDate = start, EndDate = end },
                Party = ObjectMother.Create<Party>().CreateNexusEntityId(()=> "")
            };

            // NB Don't assign validity here, want to prove SUT sets it
            var details = new ExchangeDetails();

            var mapping = new PartyRoleMapping();

            var mappingEngine = new Mock<IMappingEngine>();
            var repository = new Mock<IRepository>();
            mappingEngine.Setup(x => x.Map<RWEST.Nexus.MDM.Contracts.NexusId, PartyRoleMapping>(id)).Returns(mapping);
            mappingEngine.Setup(x => x.Map<RWEST.Nexus.MDM.Contracts.NexusId, PartyRoleMapping>(id)).Returns(mapping);
            mappingEngine.Setup(x => x.Map<RWEST.Nexus.MDM.Contracts.ExchangeDetails, ExchangeDetails>(contractDetails)).Returns(details);
            repository.Setup(x => x.FindOne<Party>(int.Parse(contract.Party.Identifier.Identifier))).Returns(ObjectMother.Create<Party>());

            var mapper = new ExchangeMapper(mappingEngine.Object, repository.Object);

            // Act
            var candidate = mapper.Map(contract);

            // Assert
            //Assert.AreEqual(1, candidate.Details.Count, "Detail count differs");
            Assert.AreEqual(1, candidate.Mappings.Count, "Mapping count differs");
            Assert.AreEqual("Exchange", candidate.PartyRoleType);
            Check(range, details.Validity, "Validity differs");
        }
    }
}