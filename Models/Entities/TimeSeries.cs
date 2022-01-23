namespace Covid19.Models.Entities
{
    using System.Collections.Generic;
    using System.Linq;

    using Covid19.Utilities;

    public class TimeSeries
    {
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