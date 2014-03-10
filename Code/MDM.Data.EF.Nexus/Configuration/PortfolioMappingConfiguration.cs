namespace EnergyTrading.MDM.Data.EF.Configuration
{
    using System.Data.Entity.ModelConfiguration;

    using RWEST.Nexus.MDM;

    public class PortfolioMappingConfiguration : EntityTypeConfiguration<PortfolioMapping>
    {
        public PortfolioMappingConfiguration()
        {
            this.ToTable("PortfolioMapping");

            this.Property(x => x.Id).HasColumnName("PortfolioMappingId");
            this.HasRequired(x => x.System).WithMany().Map(x => x.MapKey("SourceSystemId"));
            this.HasRequired(x => x.Portfolio).WithMany(y => y.Mappings).Map(x => x.MapKey("PortfolioId"));
            this.Property(x => x.Validity.Start).HasColumnName("Start");
            this.Property(x => x.Validity.Finish).HasColumnName("Finish");
            this.Property(x => x.Version).IsRowVersion();
        }
    }
}