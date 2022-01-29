namespace Covid19.ViewModels
{
    public class Graph
    {
        public string Label { get; init; }
        public string BorderColor { get; set; }
        public string[] YData { get; init; }

        public static readonly string[] Colors = {
            "#FF0000",
            "#0000FF",
            "#00FF00",
            "#00FFFF",
            "#FF00FF",
            "#FFBF00",
            "#FF7F50",
            "#DE3163",
            "#9FE2BF",
            "#40E0D0",
            "#6495ED",
            "#CCCCFF"
        };
    }
}