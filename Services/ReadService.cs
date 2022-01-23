namespace Covid19.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Text.RegularExpressions;

    using Covid19.Models.Entities;
    using Covid19.Utilities;

    public class ReadService
    {
        private const string CsvUrl = "https://raw.githubusercontent.com/bpashkovskyi/covid-19-if/main/data2.csv";

        public List<Case> Read()
        {
            var streamReader = this.GetStreamReaderForRemoteUrl(CsvUrl);

            var dataTable = this.ConvertCsvToDataTable(streamReader);
            var cases = this.Read(dataTable);

            return cases;
        }

        private StreamReader GetStreamReaderForRemoteUrl(string url)
        {
            var webRequest = WebRequest.Create(url);

            ////var webResponse = webRequest.GetResponse();
            ////var responseStream = webResponse.GetResponseStream();
            ////var streamReader = new StreamReader(responseStream);

            var filePath = Path.GetFullPath("wwwroot\\data.csv");
            var streamReader = new StreamReader(filePath);

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

        private List<Case> Read(DataTable dataTable)
        {
            var cases = new List<Case>();

            for (var rowIndex = 0; rowIndex < dataTable.Rows.Count; rowIndex++)
            {
                var @case = new Case
                {
                    Id = dataTable.Rows[rowIndex]["Людський випадок - Інд#№"].ToString(),
                    InDate = DateTime.Parse(dataTable.Rows[rowIndex]["Людський випадок - Дата заповнення паперової форми"].ToString()),
                    District = dataTable.Rows[rowIndex]["Людський випадок Поточне місце проживання - Район"].ToString(),
                    City = dataTable.Rows[rowIndex]["Людський випадок Поточне місце проживання - Населений пункт"].ToString(),
                    Gender = dataTable.Rows[rowIndex]["Людський випадок - Стать"].ToString(),
                    Age = int.Parse(dataTable.Rows[rowIndex]["Людський випадок - Вік пацієнта"].ToString()),
                    Illnesses = dataTable.Rows[rowIndex]["Наявність супутніх станів"].ToString() == "Так",
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


            return cases;
        }
    }
}