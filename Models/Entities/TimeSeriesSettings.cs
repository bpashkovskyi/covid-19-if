namespace Covid19.Models.Entities
{
    using Covid19.Models.Enums;

    public class TimeSeriesSettings
    {
        public ClinicalStatusType ClinicalStatusType { get; init; }
        public GroupType GroupType { get; init; }
        public AggregationType AggregationType { get; init; }
    }
}