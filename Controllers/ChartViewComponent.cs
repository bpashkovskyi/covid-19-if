namespace Covid19.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Covid19.Models.Entities;
    using Covid19.Services;
    using Covid19.ViewModels;

    using Microsoft.AspNetCore.Mvc;

    public class ChartViewComponent : ViewComponent
    {

        private readonly IReadService readService;

        public ChartViewComponent(IReadService readService)
        {
            this.readService = readService;
        }

        public async Task<IViewComponentResult> InvokeAsync(TimeSeriesSettings timeSeriesSettings)
        {
            var cases = await this.readService.ReadAsync().ConfigureAwait(false);
            var timeSeries = new TimeSeries(timeSeriesSettings.GroupType.ToString(), cases, timeSeriesSettings);
            var chart = this.MapTimeSeries(timeSeries);

            return this.View("_Chart", chart);
        }

        private Chart MapTimeSeries(TimeSeries timeSeries)
        {
            var chart = new Chart(timeSeries.Name)
            {
                Title = timeSeries.Name,
                XData = timeSeries.DaysData.Select(dayData => dayData.Date.ToLongDateString()).ToArray(),
                Lines = timeSeries.DaysData.First().GroupValues.Select(
                    groupValue => new ChartLine
                    {
                        Label = groupValue.Name,
                        YData = timeSeries.DaysData.Select(dayData => dayData.GetGroupValue(groupValue.Name).CasesCount.ToString()).ToArray()
                    }).ToList()
            };

            foreach (var chartLine in chart.Lines)
            {
                chartLine.BorderColor = ChartLine.Colors[chart.Lines.IndexOf(chartLine)];
            }

            return chart;
        }
    }
}