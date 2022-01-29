namespace Covid19.Models.Entities
{
    public class SocialGroupValue
    {
        public SocialGroupValue(string name, long casesCount)
        {
            this.Name = name;
            this.CasesCount = casesCount;
        }

        public string Name { get; }
        public long CasesCount { get; }
    }
}