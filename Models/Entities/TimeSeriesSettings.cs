namespace IfCovid.Models.Entities
{
    using IfCovid.Models.Enums;

    public class TimeSeriesSettings
    {
        public ClinicalStatusType ClinicalStatusType { get; init; }
        public GroupType GroupType { get; init; }
        public AggregationType AggregationType { get; init; }
    }
}