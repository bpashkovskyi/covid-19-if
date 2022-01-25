namespace Covid19.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Covid19.Models.Entities;
    using Covid19.Services;

    using Microsoft.AspNetCore.Mvc;

    public class HomeController : Controller
    {
        private readonly IReadService readService;

        public HomeController(IReadService readService)
        {
            this.readService = readService;
        }

        public async Task<IActionResult> Index(string timeSeriesType = "Illnesses")
        {
            var cases = await this.readService.ReadAsync().ConfigureAwait(false);

            var timeSeries = new TimeSeries(cases);


            var weeklyAverageTimeSeries = timeSeries.GetWeeklyAverageData();

            var categories = new List<Category>();

            switch (timeSeriesType)
            {
                case "Illnesses":
                    categories = weeklyAverageTimeSeries.DaysData.Select(dayData => dayData.Illness).ToList();
                    break;
                case "Hospitalized":
                    categories = weeklyAverageTimeSeries.DaysData.Select(dayData => dayData.Hospitalization).ToList();
                    break;
                case "IntensiveCare":
                    categories = weeklyAverageTimeSeries.DaysData.Select(dayData => dayData.IntensiveCare).ToList();
                    break;
                case "Ventilated":
                    categories = weeklyAverageTimeSeries.DaysData.Select(dayData => dayData.Ventilated).ToList();
                    break;
                case "Dead":
                    categories = weeklyAverageTimeSeries.DaysData.Select(dayData => dayData.Dead).ToList();
                    break;
            }
            

            return this.View("Graph", categories);
        }
    }
}