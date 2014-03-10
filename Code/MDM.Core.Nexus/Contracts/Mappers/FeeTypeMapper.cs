namespace EnergyTrading.MDM.Contracts.Mappers
{
    using System.Collections.Generic;

    using RWEST.Nexus.MDM.Contracts;
    using EnergyTrading.Mapping;
	
    /// <summary>
    /// Maps a <see cref="SourceSystem" /> to a <see cref="FeeType" />
    /// </summary>
    public class FeeTypeMapper : ContractMapper<FeeType, MDM.FeeType, FeeTypeDetails, MDM.FeeType, FeeTypeMapping>
    {
        public FeeTypeMapper(IMappingEngine mappingEngine) : base(mappingEngine)
        {
        }

        protected override FeeTypeDetails ContractDetails(FeeType contract)
        {
            return contract.Details;
        }

        protected override EnergyTrading.DateRange ContractDetailsValidity(FeeType contract)
        {
            return this.SystemDataValidity(contract.Nexus);
        }

        protected override IEnumerable<RWEST.Nexus.MDM.Contracts.NexusId> Identifiers(FeeType contract)
        {
            return contract.Identifiers;
        }
    }
}