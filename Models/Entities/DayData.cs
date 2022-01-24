namespace Covid19.Models.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class DayData
    {
        public DayData()
        {
        }

        public DayData(DateTime date, List<Case> cases)
        {
            Date = date;

            var dayCases = cases.Where(@case => @case.InDate < this.Date).ToList();

            this.Illness = new Category(date, dayCases);
            this.Hospitalization = new Category(date, dayCases.Where(@case => @case.Hospitalized).ToList());
            this.IntensiveCare = new Category(date, dayCases.Where(@case => @case.IntensiveCare).ToList());
            this.Ventilated = new Category(date, dayCases.Where(@case => @case.Ventilated).ToList());
            this.Dead = new Category(date, dayCases.Where(@case => @case.Dead).ToList());
        }

        public DateTime Date { get; private init; }

        public Category Illness { get; private init; }

        public Category Hospitalization { get; private init; }

        public Category IntensiveCare { get; private init; }

        public Category Ventilated { get; private init; }

        public Category Dead { get; private init; }

        public DayData GetAverageData(List<DayData> lastWeek)
        {
            var averageDay = new DayData
            {
                Date = this.Date,
                Illness = this.Illness.GetAverageData(lastWeek.Select(day => day.Illness).ToList()),
                Hospitalization = this.Hospitalization.GetAverageData(lastWeek.Select(day => day.Hospitalization).ToList()),
                IntensiveCare = this.IntensiveCare.GetAverageData(lastWeek.Select(day => day.IntensiveCare).ToList()),
                Ventilated = this.Ventilated.GetAverageData(lastWeek.Select(day => day.Ventilated).ToList()),
                Dead = this.Dead.GetAverageData(lastWeek.Select(day => day.Dead).ToList()),
            };

            return averageDay;
        }
    }
}