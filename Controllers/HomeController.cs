namespace Covid19.Controllers
{
    using Covid19.Services;

    using Microsoft.AspNetCore.Mvc;

    public class HomeController : Controller
    {
        private readonly TimeSeriesReadService timeSeriesReadService;
        private readonly PredictionService predictionService;

        public HomeController()
        {
            this.timeSeriesReadService = new TimeSeriesReadService();
            this.predictionService = new PredictionService();
        }

        public IActionResult Index()
        {
            var timeSeriesDictionary = this.timeSeriesReadService.ReadTimeSeries();
            var predictedTimeSeriesDictionary = this.predictionService.CreatePredictionTimeSeriesDictionary(timeSeriesDictionary);

            return this.View("Graph", predictedTimeSeriesDictionary);
        }
    }
}