namespace Covid19.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Covid19.Models.Entities;
    using Covid19.Services;
    using Covid19.ViewModels;

    using Microsoft.AspNetCore.Mvc;

    public class HomeController : Controller
    {
        private readonly IReadService readService;

        public HomeController(IReadService readService)
        {
            this.readService = readService;
        }

        public async Task<IActionResult> Index(string clinicalStatus = "Illness", string graphType = "Daily")
        {
            var cases = await this.readService.ReadAsync().ConfigureAwait(false);

            TimeSeries timeSeries = null;

            switch (graphType)
            {
                case "Daily": timeSeries = new TimeSeries(cases, cumulative: false);
                    break;
                case "Weekly":
                    timeSeries = new TimeSeries(cases, cumulative: false).GetWeeklyAverageData();
                    break;
                case "Cumulative":
                    timeSeries = new TimeSeries(cases, cumulative: true);
                    break;
            }

            var clinicalStatuses = timeSeries.DaysData.Select(dayData => dayData.GetClinicalStatus(clinicalStatus)).ToList();

            var chartsViewModel = new ChartsViewModel
            {
                Charts = this.MapClinicalStatuses(clinicalStatuses),
                Settings = new ViewSettings { ClinicalStatus = clinicalStatus, GraphType = graphType }
            };

            return this.View("Graph", chartsViewModel);
        }

        private List<Chart> MapClinicalStatuses(List<ClinicalStatus> clinicalStatuses)
        {
            var charts = clinicalStatuses.First().SocialGroups.Select(socialGroup => new Chart(socialGroup.Name)
            {
                Title = socialGroup.Name,
                XData = clinicalStatuses.Select(clinicalStatus => clinicalStatus.Date.ToLongDateString()).ToArray(),
                Graphs = this.MapSocialGroups(clinicalStatuses.Select(clinicalStatus => clinicalStatus.GetSocialGroup(socialGroup.Name)).ToList())
            }).ToList();

            return charts;
        }

        private List<Graph> MapSocialGroups(List<SocialGroup> socialGroups)
        {
            var graphs = socialGroups.First().Values.Select(
                socialGroupValue => new Graph
                {
                    Label = socialGroupValue.Name,
                    YData = socialGroups.Select(socialGroup => socialGroup.GetSocialGroupValue(socialGroupValue.Name).CasesCount.ToString()).ToArray()
                }).ToList();

            foreach (var graph in graphs)
            {
                graph.BorderColor = Graph.Colors[graphs.IndexOf(graph)];
            }

            return graphs;
        }
    }
}