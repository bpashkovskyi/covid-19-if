namespace Covid19.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Covid19.Models.Entities;
    using Covid19.Utilities;

    using MathNet.Numerics;

    public class PredictionService
    {
        public Dictionary<string, PredictionTimeSeries> CreatePredictionTimeSeriesDictionary(Dictionary<string, TimeSeries> timeSeriesDictionary)
        {
            var predictionTimeSeriesDictionary = new Dictionary<string, PredictionTimeSeries>();

            foreach (var (timeSeriesName, timeSeries) in timeSeriesDictionary)
            {
                this.AdjustRealDaysData(timeSeries);
                var predictionTimeSeries = this.CreatePredictionTimeSeries(timeSeries);

                predictionTimeSeriesDictionary.Add(timeSeriesName, predictionTimeSeries);
            }

            return predictionTimeSeriesDictionary;
        }

        private Func<double, double> CalculateRegressionFunction(TimeSeries timeSeries, out int optimalDaysToSkip)
        {
            optimalDaysToSkip = 0;

            var optimalDaysDataForPrediction = timeSeries.DaysData
                .Skip(optimalDaysToSkip).ToArray();

            var optimalXData = optimalDaysDataForPrediction.Select(dayData => (double)dayData.DayNumber).ToArray();
            var optimalYData = optimalDaysDataForPrediction.Select(dayData => (double)dayData.NewCases).ToArray();

            var optimalRegressionFunction = Fit.PolynomialFunc(optimalXData, optimalYData, 1);
            return optimalRegressionFunction;
        }

        private PredictionTimeSeries CreatePredictionTimeSeries(TimeSeries timeSeries)
        {
            var regressionFunction = this.CalculateRegressionFunction(timeSeries, out var daysToSkip);
            var dayWithLocationsData = timeSeries.DaysData.Skip(daysToSkip).ToList();

            var firstDay = dayWithLocationsData.First();

            var predictionDaysData = new List<PredictionDayData>
            {
                new PredictionDayData
                {
                    DayNumber = firstDay.DayNumber,
                    Date = firstDay.Date,
                    TotalCases = firstDay.TotalCases,
                    NewCases = firstDay.NewCases,
                    PredictionTotalCases = firstDay.TotalCases
                }
            };

            while (predictionDaysData.Count < (dayWithLocationsData.Count + 60))
            {
                var lastPredictionDay = predictionDaysData.Last();

                var realDay = dayWithLocationsData.FirstOrDefault(day => day.DayNumber == (lastPredictionDay.DayNumber + 1));

                var newPredictionDay = new PredictionDayData
                {
                    Date = realDay?.Date ?? lastPredictionDay.Date.AddDays(1),
                    DayNumber = lastPredictionDay.DayNumber + 1,
                    TotalCases = realDay?.TotalCases,
                    NewCases = realDay?.NewCases,
                    WeeklyNewCases = realDay?.WeeklyNewCases
                };

                newPredictionDay.PredictionNewCases = (long)regressionFunction(newPredictionDay.DayNumber);
                newPredictionDay.PredictionTotalCases = lastPredictionDay.PredictionTotalCases + newPredictionDay.PredictionNewCases;

                predictionDaysData.Add(newPredictionDay);
            }

            predictionDaysData.Remove(predictionDaysData.Last());

            return new PredictionTimeSeries
            {
                DaysData = predictionDaysData.ToList()
            };
        }

        private void AdjustRealDaysData(TimeSeries timeSeries)
        {
            foreach (var day in timeSeries.DaysData)
            {
                var previousDay = timeSeries.DaysData.GetPreviousElement(day, 1);

                day.NewCases = previousDay == null ? day.NewCases : day.TotalCases - previousDay.TotalCases;

                var previousWeekDay = timeSeries.DaysData.GetPreviousElement(day, 7);
                day.WeeklyNewCases = previousWeekDay == null ? 0 : (day.TotalCases - previousWeekDay.TotalCases) / 7;
            }
        }
    }
}