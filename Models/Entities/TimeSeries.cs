namespace Covid19.Models.Entities
{
    using System.Collections.Generic;
    using System.Linq;

    using Covid19.Models.Enums;
    using Covid19.Utilities;

    public class TimeSeries
    {
        public string Name { get; }

        public TimeSeries(string name, List<Case> cases, TimeSeriesSettings timeSeriesSettings)
        {
            this.Name = name;

            var casesDates = cases.Select(@case => @case.InDate).Distinct().OrderBy(date => date);

            switch (timeSeriesSettings.ClinicalStatusType)
            {
                case ClinicalStatusType.Hospitalization: cases = cases.Where(@case => @case.Hospitalized).ToList();
                    break;
                case ClinicalStatusType.IntensiveCare:
                    cases = cases.Where(@case => @case.IntensiveCare).ToList();
                    break;
                case ClinicalStatusType.Ventilated:
                    cases = cases.Where(@case => @case.Ventilated).ToList();
                    break;
                case ClinicalStatusType.Dead:
                    cases = cases.Where(@case => @case.Dead).ToList();
                    break;
            }

            var daysData = casesDates.Select(caseDate => new DayData(caseDate, cases, timeSeriesSettings)).ToList();

            this.DaysData = timeSeriesSettings.AggregationType == AggregationType.Weekly 
                ? this.GetWeeklyAverageData(daysData) 
                : daysData;
        }

        public List<DayData> DaysData { get; }

        private List<DayData> GetWeeklyAverageData(List<DayData> daysData)
        {
            var averageDaysData = new List<DayData>();
            foreach (var day in daysData)
            {
                var lastWeek = new List<DayData>
                {
                    daysData.GetPreviousElement(day, 7),
                    daysData.GetPreviousElement(day, 6),
                    daysData.GetPreviousElement(day, 5),
                    daysData.GetPreviousElement(day, 4),
                    daysData.GetPreviousElement(day, 3),
                    daysData.GetPreviousElement(day, 2),
                    daysData.GetPreviousElement(day, 1)
                };

                lastWeek = lastWeek.Where(currentDay => currentDay != null).ToList();

                var averageDay = day.GetAverageData(lastWeek);
                averageDaysData.Add(averageDay);
            }

            return averageDaysData;
        }
    }
}