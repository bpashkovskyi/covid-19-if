namespace Covid19.Models.Entities
{
    using Covid19.Models.Enums;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class DayData
    {
        private DayData()
        {
        }

        public DayData(DateTime caseDate, List<Case> cases, TimeSeriesSettings timeSeriesSettings)
        {
            this.Date = caseDate;

            var dayCases = timeSeriesSettings.AggregationType == AggregationType.Cumulative 
                ? cases.Where(@case => @case.InDate < this.Date).ToList() 
                : cases.Where(@case => @case.InDate == this.Date).ToList();

            this.ClinicalStatusType = timeSeriesSettings.ClinicalStatusType;
            this.GroupType = timeSeriesSettings.GroupType;

            switch (timeSeriesSettings.GroupType)
            {
                case GroupType.Gender:
                    this.GroupValues = new List<GroupValue>
                    {
                        new("Total", dayCases.Count),
                        new("Female", dayCases.Count(@case => @case.Gender == "Жіноча")),
                        new("Male", dayCases.Count(@case => @case.Gender == "Чоловіча"))
                    };
                    break;
                case GroupType.Age:
                    this.GroupValues = new List<GroupValue>
                    {
                        new("To 5", dayCases.Count(@case => @case.Age <= 5)),
                        new("From 6 to 11", dayCases.Count(@case => @case.Age is >= 6 and <= 11)),
                        new("From 12 to 17", dayCases.Count(@case => @case.Age is >= 12 and <= 17)),
                        new("From 18 to 33", dayCases.Count(@case => @case.Age is >= 18 and <= 33)),
                        new("From 34 to 50", dayCases.Count(@case => @case.Age is >= 34 and <= 50)),
                        new("From 51 to 70", dayCases.Count(@case => @case.Age is >= 51 and <= 70)),
                        new("From 71", dayCases.Count(@case => @case.Age >= 71))
                    };
                    break;
                case GroupType.Illness:
                    this.GroupValues = new List<GroupValue>
                    {
                        new("With other illnesses", dayCases.Count(@case => @case.OtherIllnesses)),
                        new("Without other illnesses", dayCases.Count(@case => !@case.OtherIllnesses))
                    };
                    break;
            }
        }

        public ClinicalStatusType ClinicalStatusType { get; set; }
        public GroupType GroupType { get; set; }

        public DateTime Date { get; init; }

        public List<GroupValue> GroupValues { get; init; } = new List<GroupValue>();

        public DayData GetAverageData(List<DayData> lastPeriodData)
        {
            lastPeriodData.Add(this);

            var dayData = new DayData
            {
                GroupValues = this.GroupValues.Select(groupValue => new GroupValue(groupValue.Name, (long)lastPeriodData.Average(previousData => previousData.GetGroupValue(groupValue.Name).CasesCount))).ToList()
            };

            return dayData;
        }

        public GroupValue GetGroupValue(string name)
        {
            return this.GroupValues.SingleOrDefault(groupValue => groupValue.Name == name);
        }
    }
}