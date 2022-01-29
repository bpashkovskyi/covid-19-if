namespace Covid19.Models.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class DayData
    {
        private DayData()
        {
        }

        public DayData(DateTime date, List<Case> cases, bool cumulative = false)
        {
            this.Date = date;

            var dayCases = cumulative 
                ? cases.Where(@case => @case.InDate < this.Date).ToList()
                : cases.Where(@case => @case.InDate == this.Date).ToList();

            this.ClinicalStatuses = new List<ClinicalStatus>
            {
                new ClinicalStatus("Illness", date, dayCases),
                new ClinicalStatus("Hospitalization", date, dayCases.Where(@case => @case.Hospitalized).ToList()),
                new ClinicalStatus("IntensiveCare", date, dayCases.Where(@case => @case.IntensiveCare).ToList()),
                new ClinicalStatus("Ventilated", date, dayCases.Where(@case => @case.Ventilated).ToList()),
                new ClinicalStatus("Dead", date, dayCases.Where(@case => @case.Dead).ToList()),
            };
        }

        private DateTime Date { get; init; }

        private List<ClinicalStatus> ClinicalStatuses { get; init; }

        public DayData GetAverageData(List<DayData> previousDays)
        {
            var averageDay = new DayData
            {
                Date = this.Date,
                ClinicalStatuses = this.ClinicalStatuses.Select(clinicalStatus => clinicalStatus.GetAverageData(previousDays.Select(previousDay => previousDay.GetClinicalStatus(clinicalStatus.Name)).ToList())).ToList()
            };

            return averageDay;
        }

        public ClinicalStatus GetClinicalStatus(string name)
        {
            return this.ClinicalStatuses.SingleOrDefault(clinicalStatus => clinicalStatus.Name == name);
        }
    }
}