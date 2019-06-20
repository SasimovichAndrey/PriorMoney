using System;
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
            throw new NotImplementedException();
        }
    }
}