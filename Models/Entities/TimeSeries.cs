namespace Covid19.Models.Entities
{
    using System.Collections.Generic;
    using System.Linq;

    using Covid19.Utilities;

    public class TimeSeries
    {
        private TimeSeries()
        {
        }

        public TimeSeries(List<Case> cases)
        {
            var casesDates = cases.Select(@case => @case.InDate).Distinct().OrderBy(date => date);

            foreach (var caseDate in casesDates)
            {
                var dayData = new DayData(caseDate, cases);
                this.DaysData.Add(dayData);
            }
        }

        public List<DayData> DaysData { get; } = new List<DayData>();

        public TimeSeries GetWeeklyAverageData()
        {
            var timeSeries = new TimeSeries();
            foreach (var day in this.DaysData)
            {
                var lastWeek = new List<DayData>
                {
                    this.DaysData.GetPreviousElement(day, 7),
                    this.DaysData.GetPreviousElement(day, 6),
                    this.DaysData.GetPreviousElement(day, 5),
                    this.DaysData.GetPreviousElement(day, 4),
                    this.DaysData.GetPreviousElement(day, 3),
                    this.DaysData.GetPreviousElement(day, 2),
                    this.DaysData.GetPreviousElement(day, 1)
                };

                lastWeek = lastWeek.Where(currentDay => currentDay != null).ToList();

                var averageDay = day.GetAverageData(lastWeek);

                timeSeries.DaysData.Add(averageDay);
            }

            return timeSeries;
        }
    }
}