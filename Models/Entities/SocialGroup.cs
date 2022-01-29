namespace Covid19.Models.Entities
{
    using System.Collections.Generic;
    using System.Linq;

    public class SocialGroup
    {
        public SocialGroup(string name)
        {
            this.Name = name;
        }

        public string Name { get; }

        public List<SocialGroupValue> Values { get; init; } = new List<SocialGroupValue>();

        public SocialGroup GetAverageData(List<SocialGroup> lastPeriodData)
        {
            var socialGroup = new SocialGroup(this.Name)
            {
                Values = this.Values.Select(socialGroupValue => new SocialGroupValue(socialGroupValue.Name, (long)lastPeriodData.Average(previousData => previousData.GetSocialGroupValue(socialGroupValue.Name).CasesCount))).ToList()
            };

            return socialGroup;
        }

        public SocialGroupValue GetSocialGroupValue(string name)
        {
            return this.Values.SingleOrDefault(socialGroupValue => socialGroupValue.Name == name);
        }
    }
}