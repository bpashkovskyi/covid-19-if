namespace Covid19.Models.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ClinicalStatus
    {
        private ClinicalStatus()
        {
        }

        public ClinicalStatus(string name, DateTime date, List<Case> cases)
        {
            this.Name = name;
            this.Date = date;

            this.Total = cases.Count;

            this.SocialGroups = new List<SocialGroup>
            {
                new("Gender")
                {
                    Values = new List<SocialGroupValue>
                    {
                        new("Female", cases.Count(@case => @case.Gender == "Жіноча")),
                        new("Male", cases.Count(@case => @case.Gender == "Чоловіча"))
                    }
                },
                new("Age")
                {
                    Values = new List<SocialGroupValue>
                    {
                        new("To 5", cases.Count(@case => @case.Age <= 5)),
                        new("From 6 to 11", cases.Count(@case => @case.Age is >= 6 and <= 11)),
                        new("From 12 to 17", cases.Count(@case => @case.Age is >= 12 and <= 17)),
                        new("From 18 to 33", cases.Count(@case => @case.Age is >= 18 and <= 33)),
                        new("From 34 to 50", cases.Count(@case => @case.Age is >= 34 and <= 50)),
                        new("From 51 to 70", cases.Count(@case => @case.Age is >= 51 and <= 70)),
                        new("From 71", cases.Count(@case => @case.Age >= 71))
                    }
                },
                new("Illness")
                {
                    Values = new List<SocialGroupValue>
                    {
                        new("With other illnesses", cases.Count(@case => @case.OtherIllnesses)),
                        new("Without other illnesses", cases.Count(@case => !@case.OtherIllnesses))
                    }
                }
            };
        }

        public string Name { get; private init; }
        public DateTime Date { get; private init; }

        public long Total { get; private init; }

        public List<SocialGroup> SocialGroups { get; private init; }

        public ClinicalStatus GetAverageData(List<ClinicalStatus> previousDays)
        {
            previousDays.Add(this);

            var category = new ClinicalStatus
            {
                Name = this.Name,
                Date = this.Date,
                Total = (long)previousDays.Average(clinicalStatus => clinicalStatus.Total),
                SocialGroups = this.SocialGroups.Select(socialGroup => socialGroup.GetAverageData(previousDays.Select(previousDay => previousDay.GetSocialGroup(socialGroup.Name)).ToList())).ToList()
            };

            return category;
        }

        public SocialGroup GetSocialGroup(string name)
        {
            return this.SocialGroups.SingleOrDefault(socialGroup => socialGroup.Name == name);
        }
    }
}