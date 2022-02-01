namespace Covid19.ViewModels
{
    using Covid19.Models.Enums;

    public class PageSettings
    {
        public ClinicalStatusType ClinicalStatusType { get; init; }

        public AggregationType AggregationType { get; init; }
    }
}