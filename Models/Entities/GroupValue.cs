namespace IfCovid.Models.Entities
{
    public class GroupValue
    {
        public GroupValue(string name, long casesCount)
        {
            this.Name = name;
            this.CasesCount = casesCount;
        }

        public string Name { get; }
        public long CasesCount { get; }
    }
}