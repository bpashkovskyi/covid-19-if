namespace Covid19.Controllers
{
    using Covid19.Services;

    using Microsoft.AspNetCore.Mvc;

    public class HomeController : Controller
    {
        private readonly TimeSeriesReadService timeSeriesReadService;

        public HomeController()
        {
            this.timeSeriesReadService = new TimeSeriesReadService();
        }

        public IActionResult Index()
        {
            var timeSeriesDictionary = this.timeSeriesReadService.ReadTimeSeries();

            return this.View("Graph", timeSeriesDictionary);
        }
    }
}