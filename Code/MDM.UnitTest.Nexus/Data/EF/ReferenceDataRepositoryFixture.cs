namespace EnergyTrading.MDM.Test.Data.EF
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RWEST.Nexus.MDM;

    [TestClass]
    public class ReferenceDataRepositoryFixture : DbSetRepositoryFixture<ReferenceData>
    {        
        protected override ReferenceData Default()
        {
            return new ReferenceData { Key = Guid.NewGuid().ToString(), Value = Guid.NewGuid().ToString() };
        }
    }
}