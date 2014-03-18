namespace EnergyTrading.MDM.Contracts.Mappers
{
    using EnergyTrading.Mapping;

    public class CalendarDetailsMapper : Mapper<OpenNexus.MDM.Contracts.CalendarDetails, MDM.Calendar>
    {
        public override void Map(OpenNexus.MDM.Contracts.CalendarDetails source, MDM.Calendar destination)
        {
            destination.Name = source.Name;

            foreach(var cd in source.CalendayDays)
            {
                destination.Days.Add(
                    new MDM.CalendarDay() { Date = cd.CalendarDate, DayType = (int)cd.CalendarDayType });
            }

            // destination.Days = source.CalendayDays;
        }
    }
}
