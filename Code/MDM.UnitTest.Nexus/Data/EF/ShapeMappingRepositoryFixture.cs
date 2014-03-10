namespace EnergyTrading.MDM.Test.Data.EF
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RWEST.Nexus.MDM;

    [TestClass]
    public class ShapeMappingRepositoryFixture : DbSetRepositoryFixture<ShapeMapping>
    {
        protected override ShapeMapping Default()
        {
            var entity = base.Default();
            entity.Shape = ObjectMother.Create<Shape>();
            entity.System = new SourceSystem { Name = Guid.NewGuid().ToString() };
            entity.MappingValue = "Test";

            return entity;
        }
    }
}