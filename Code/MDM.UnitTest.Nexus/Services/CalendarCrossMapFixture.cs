namespace EnergyTrading.MDM.Test.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using RWEST.Nexus.MDM.Contracts;
    using EnergyTrading;
    using EnergyTrading.Data;
    using EnergyTrading.Mapping;
    using EnergyTrading.Validation;
    using EnergyTrading.Search;
    using RWEST.Nexus.MDM;
    using EnergyTrading.MDM.Messages;
    using EnergyTrading.MDM.Services;

    using SourceSystem = EnergyTrading.MDM.SourceSystem;
	
    [TestClass]
    public class CalendarCrossMapFixture
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullRequestErrors()
        {
            // Arrange
            var validatorFactory = new Mock<IValidatorEngine>();
            var mappingEngine = new Mock<IMappingEngine>();
            var repository = new Mock<IRepository>();
			var searchCache = new Mock<ISearchCache>();

            var service = new CalendarService(validatorFactory.Object, mappingEngine.Object, repository.Object, searchCache.Object);

            // Act
            service.CrossMap(null);
        }

        [TestMethod]
        public void UnsuccessfulMatchReturnsNotFound()
        {
            // Arrange
            var validatorFactory = new Mock<IValidatorEngine>();
            var mappingEngine = new Mock<IMappingEngine>();
            var repository = new Mock<IRepository>();
			var searchCache = new Mock<ISearchCache>();

            var service = new CalendarService(validatorFactory.Object, mappingEngine.Object, repository.Object, searchCache.Object);

            var list = new List<CalendarMapping>();
            repository.Setup(x => x.Queryable<CalendarMapping>()).Returns(list.AsQueryable());

            var request = new CrossMappingRequest { SystemName = "Endur", Identifier = "A", ValidAt = SystemTime.UtcNow(), TargetSystemName = "Trayport" };

            // Act
            var contract = service.CrossMap(request);

            // Assert
            Assert.IsNotNull(contract, "Contract null");
            Assert.IsFalse(contract.IsValid, "Contract valid");
            Assert.AreEqual(ErrorType.NotFound, contract.Error.Type, "ErrorType difers");
        }

        [TestMethod]
        public void SuccessMatch()
        {
            // Arrange
            var validatorFactory = new Mock<IValidatorEngine>();
            var mappingEngine = new Mock<IMappingEngine>();
            var repository = new Mock<IRepository>();
			var searchCache = new Mock<ISearchCache>();

            var service = new CalendarService(validatorFactory.Object, mappingEngine.Object, repository.Object, searchCache.Object);

            // Domain details
            var system = new MDM.SourceSystem { Name = "Endur" };
            var mapping = new CalendarMapping
            {
                System = system,
                MappingValue = "A"
            };
            var targetSystem = new MDM.SourceSystem { Name = "Trayport" };
            var targetMapping = new CalendarMapping
            {
                System = targetSystem,
                MappingValue = "B",
                IsDefault = true
            };
            var calendar = new MDM.Calendar
            {
                Id = 1,
                Timestamp = new byte[] { 1, 0, 0, 0, 0, 0, 0, 0 }
            };
            //Calendar.AddDetails(details);
            calendar.ProcessMapping(mapping);
            calendar.ProcessMapping(targetMapping);

            // Contract details
            var targetIdentifier = new NexusId
            {
                SystemName = "Trayport",
                Identifier = "B"
            };

            mappingEngine.Setup(x => x.Map<CalendarMapping, NexusId>(targetMapping)).Returns(targetIdentifier);

            var list = new List<CalendarMapping> { mapping };
            repository.Setup(x => x.Queryable<CalendarMapping>()).Returns(list.AsQueryable());

            var request = new CrossMappingRequest
            {
                SystemName = "Endur", 
                Identifier = "A", 
                TargetSystemName = "trayport",
                ValidAt = SystemTime.UtcNow(),
                Version = 0
            };

            // Act
            var response = service.CrossMap(request);
            var candidate = response.Contract;

            // Assert
            Assert.IsNotNull(response, "Contract null");
            Assert.IsNotNull(candidate, "Mapping null");
            Assert.AreEqual(1, candidate.Mappings.Count, "Identifier count incorrect");
            Assert.AreSame(targetIdentifier, candidate.Mappings[0], "Different identifier assigned");
        }

        [TestMethod]
        public void SuccessMatchSameVersion()
        {
            // Arrange
            var validatorFactory = new Mock<IValidatorEngine>();
            var mappingEngine = new Mock<IMappingEngine>();
            var repository = new Mock<IRepository>();
			var searchCache = new Mock<ISearchCache>();

            var service = new CalendarService(validatorFactory.Object, mappingEngine.Object, repository.Object, searchCache.Object);

            // Domain details
            var system = new MDM.SourceSystem { Name = "Endur" };
            var mapping = new CalendarMapping
            {
                System = system,
                MappingValue = "A"
            };
            var targetSystem = new MDM.SourceSystem { Name = "Trayport" };
            var targetMapping = new CalendarMapping
            {
                System = targetSystem,
                MappingValue = "B",
                IsDefault = true
            };
            var calendar = new MDM.Calendar
            {
                Id = 1,
            };
            //Calendar.AddDetails(details);
            calendar.ProcessMapping(mapping);
            calendar.ProcessMapping(targetMapping);

            // Contract details
            var targetIdentifier = new NexusId
            {
                SystemName = "Trayport",
                Identifier = "B"
            };

            mappingEngine.Setup(x => x.Map<CalendarMapping, NexusId>(targetMapping)).Returns(targetIdentifier);

            var list = new List<CalendarMapping> { mapping };
            repository.Setup(x => x.Queryable<CalendarMapping>()).Returns(list.AsQueryable());

            var request = new CrossMappingRequest
            {
                SystemName = "Endur", 
                Identifier = "A", 
                TargetSystemName = "trayport",
                ValidAt = SystemTime.UtcNow(),
                Version = 0
            };

            // Act
            var response = service.CrossMap(request);
            var candidate = response.Contract;

            // Assert
            Assert.IsNotNull(response, "Contract null");
            Assert.IsTrue(response.IsValid, "Contract not valid");
            Assert.IsNull(candidate, "Mapping null");
        }
    }
}