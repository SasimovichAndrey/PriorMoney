using System;
using System.Text.RegularExpressions;
using PriorMoney.Utils.Models;

namespace PriorMoney.DataImport.CsvImport.Parsers
{
    public interface IDateRangeParser
    {
        DateRange Parse(string str);
    }

    public class DateRangeParser : IDateRangeParser
    {
        public DateRange Parse(string str)
        {
            var rangeRegex = new Regex(@"Период выписки:;(\d{2}.\d{2}.\d{4})-(\d{2}.\d{2}.\d{4})");
            var match = rangeRegex.Match(str);

            if (match.Success)
            {
                var dateFormat = "dd.MM.yyyy";
                var startDate = DateTime.ParseExact(match.Groups[1].Value, dateFormat, null);
                var endDate = DateTime.ParseExact(match.Groups[2].Value, dateFormat, null);
                endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);

                return new DateRange
                {
                    StartDate = startDate,
                    EndDate = endDate
                };
            }
            else
            {
                throw new Exception("Can't parse date range");
            }
        }
    }
}