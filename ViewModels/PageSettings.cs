namespace IfCovid.ViewModels
{
    using IfCovid.Models.Enums;

    public class PageSettings
    {
        public ClinicalStatusType ClinicalStatusType { get; init; }

        public AggregationType AggregationType { get; init; }
    }
}