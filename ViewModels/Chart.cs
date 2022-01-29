namespace Covid19.ViewModels
{
    using System.Collections.Generic;

    public class Chart
    {
        public Chart(string name)
        {
            this.Name = name;
        }

        public string Name { get; }
        public string Title { get; init; }
        public string XLabel { get; } = "Date";
        public string YLabel { get; } = "Cases";

        public string[] XData { get; init; }

        public List<Graph> Graphs { get; init; }
    }
}
