namespace MDM.ServiceHost.Unity.Nexus.Configuration
{
    using System.Collections.Generic;

    using EnergyTrading.Mapping;
    using EnergyTrading.MDM;
    using EnergyTrading.MDM.Contracts.Mappers;
    using EnergyTrading.MDM.Contracts.Validators;
    using EnergyTrading.MDM.Mappers;
    using EnergyTrading.MDM.Services;

    using Microsoft.Practices.Unity;

    using EnergyTrading.Contracts.Atom;

    using Location = EnergyTrading.MDM.Location;

    public class LocationConfiguration : NexusEntityConfiguration<LocationService, Location, OpenNexus.MDM.Contracts.Location, 
		LocationMapping, LocationValidator>
    {
        public LocationConfiguration(IUnityContainer container) : base(container)
        {
        }

        protected override string Name
        {
            get { return "location"; }
        }

        protected override void ContractDomainMapping()
        {
            this.Container.RegisterType<IMapper<OpenNexus.MDM.Contracts.Location, Location>, EnergyTrading.MDM.Contracts.Mappers.LocationMapper>();
            this.Container.RegisterType<IMapper<OpenNexus.MDM.Contracts.LocationDetails, Location>, EnergyTrading.MDM.Contracts.Mappers.LocationDetailsMapper>();
            this.Container.RegisterType<IMapper<EnergyTrading.Mdm.Contracts.MdmId, LocationMapping>, MappingMapper<LocationMapping>>();
        }

        protected override void DomainContractMapping()
        {
            this.MappingEngine.RegisterMap(new EnergyTrading.MDM.Mappers.LocationDetailsMapper());
            this.MappingEngine.RegisterMap(new LocationMappingMapper());      
            this.Container.RegisterType<IMapper<Location, List<Link>>, NullLinksMapper>();
            this.Container.RegisterType<IMapper<Location, OpenNexus.MDM.Contracts.Location>, EnergyTrading.MDM.Mappers.LocationMapper>();
        }
    }
}