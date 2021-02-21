namespace Covid19.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Net;
    using System.Text.RegularExpressions;

    using Covid19.Models.Entities;

    public class TimeSeriesReadService
    {
        private const string TimeSeriesCsvUrl = "https://raw.githubusercontent.com/bpashkovskyi/covid-19-if/main/data.csv";

        public Dictionary<string, TimeSeries> ReadTimeSeries()
        {
            var streamReader = this.GetStreamReaderForRemoteUrl(TimeSeriesCsvUrl);

            var timeSeriesDataTable = this.ConvertCsvToDataTable(streamReader);
            var timeSeries = this.ReadTimeSeries(timeSeriesDataTable);

            return timeSeries;
        }

        private StreamReader GetStreamReaderForRemoteUrl(string url)
        {
            var webRequest = WebRequest.Create(url);

            var webResponse = webRequest.GetResponse();
            var responseStream = webResponse.GetResponseStream();
            var streamReader = new StreamReader(responseStream);

            return streamReader;
        }

        private DataTable ConvertCsvToDataTable(StreamReader streamReader)
        {
            var headers = streamReader.ReadLine().Split(',');
            var dataTable = new DataTable();

            foreach (var header in headers)
            {
                dataTable.Columns.Add(header);
            }

            while (!streamReader.EndOfStream)
            {
                var rows = Regex.Split(streamReader.ReadLine(), ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                var dataRow = dataTable.NewRow();

                for (var i = 0; i < headers.Length; i++)
                {
                    dataRow[i] = rows[i];
                }

                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }

        private Dictionary<string, TimeSeries> ReadTimeSeries(DataTable timeSeriesDataTable)
        {
            var timeSeriesDictionary = new Dictionary<string, TimeSeries>();

            for (var columnIndex = 1; columnIndex < timeSeriesDataTable.Columns.Count; columnIndex++)
            {
                var timeSeries = new TimeSeries
                {
                    DaysData = new List<DayData>()
                };

                var timeSeriesName = timeSeriesDataTable.Columns[columnIndex].ColumnName;

                for (var rowIndex = 0; rowIndex < timeSeriesDataTable.Rows.Count; rowIndex++)
                {
                    var dayData = new DayData
                    {
                        Date = DateTime.Parse(timeSeriesDataTable.Rows[rowIndex][0].ToString()),
                        DayNumber = rowIndex,
                        TotalCases = int.Parse(timeSeriesDataTable.Rows[rowIndex][columnIndex].ToString())
                    };

                    timeSeries.DaysData.Add(dayData);
                }

                timeSeriesDictionary.Add(timeSeriesName, timeSeries);
            }

            return timeSeriesDictionary;
        }
    }
}