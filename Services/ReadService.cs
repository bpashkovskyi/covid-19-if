namespace IfCovid.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Net.Http;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using IfCovid.Models.Entities;

    using Microsoft.Extensions.Caching.Memory;

    public class ReadService : IReadService
    {
        private const string CsvUrl = "https://raw.githubusercontent.com/bpashkovskyi/covid-19-if/version2/data2.csv";
        private const string CacheKey = "Cases";
        private readonly MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromDays(1));

        private readonly IMemoryCache memoryCache;
        private readonly HttpClient httpClient;
        

        public ReadService(IMemoryCache memoryCache, HttpClient httpClient)
        {
            this.memoryCache = memoryCache;
            this.httpClient = httpClient;
        }

        public async Task<List<Case>> ReadAsync()
        {
            if (!this.memoryCache.TryGetValue(CacheKey, out List<Case> cases))
            {
                var dataTable = await this.ReadCsvAsync(CsvUrl).ConfigureAwait(false);

                cases = this.Parse(dataTable);

                this.memoryCache.Set(CacheKey, cases, this.cacheOptions);
            }

            return cases;
        }

        private async Task<DataTable> ReadCsvAsync(string url)
        {
            ////string filePath = Path.GetFullPath("data2.csv");
            ////StreamReader streamReader = new StreamReader(filePath);

            using var response = await this.httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
            await using var streamToReadFrom = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

            var streamReader = new StreamReader(streamToReadFrom);

            var headersLine = await streamReader.ReadLineAsync().ConfigureAwait(false);
            var dataTable = new DataTable();

            var headers = headersLine.Split(',');
            foreach (var header in headers)
            {
                dataTable.Columns.Add(header);
            }

            while (!streamReader.EndOfStream)
            {
                try
                {
                    var nextLine = await streamReader.ReadLineAsync().ConfigureAwait(false);
                    var rows = this.SplitCsvString(nextLine);
                    var dataRow = dataTable.NewRow();

                    for (var i = 0; i < headers.Length; i++)
                    {
                        try
                        {
                            dataRow[i] = rows[i];
                        }
                        catch
                        {
                        }
                    }

                    dataTable.Rows.Add(dataRow);
                }
                catch
                {

                }
                
            }

            return dataTable;
        }

        private List<Case> Parse(DataTable dataTable)
        {
            var cases = new List<Case>();

            for (var rowIndex = 0; rowIndex < dataTable.Rows.Count; rowIndex++)
            {
                try
                {
                    var @case = new Case
                    {
                        Id = dataTable.Rows[rowIndex]["Людський випадок - Інд#№"].ToString(),
                        InDate = DateTime.Parse(dataTable.Rows[rowIndex]["Людський випадок - Дата заповнення паперової форми"].ToString()),
                        District = dataTable.Rows[rowIndex]["Людський випадок Поточне місце проживання - Район"].ToString(),
                        City = dataTable.Rows[rowIndex]["Людський випадок Поточне місце проживання - Населений пункт"].ToString(),
                        Gender = dataTable.Rows[rowIndex]["Людський випадок - Стать"].ToString(),
                        Age = int.Parse(dataTable.Rows[rowIndex]["Людський випадок - Вік пацієнта"].ToString()),
                        OtherIllnesses = dataTable.Rows[rowIndex]["Наявність супутніх станів"].ToString() == "Так",
                        Hospitalized = dataTable.Rows[rowIndex]["Людський випадок - Госпіталізація"].ToString() == "Так",
                        IntensiveCare = dataTable.Rows[rowIndex]["Перебування у відділенні інтенсивної терапії"].ToString() == "Так",
                        Ventilated = dataTable.Rows[rowIndex]["Штучна вентиляція легень"].ToString() == "Так",
                        Dead = dataTable.Rows[rowIndex]["Людський випадок - Результат"].ToString() == "Пацієнт помер"
                    };

                    if (@case.City == "Івано-Франківськ")
                    {
                        cases.Add(@case);
                    }
                }
                catch
                {
                    // Ignore failed case
                }
                
            }

            return cases;
        }

        private string[] SplitCsvString(string csvString)
        {
            var splitData = Regex.Split(csvString, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
            return splitData;
        }
    }
}