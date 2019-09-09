using PriorMoney.DataImport.CsvImport.Parsers;
using PriorMoney.DataImport.Interface;
using PriorMoney.Model;
using PriorMoney.Utils.ExceptionHandling;
using PriorMoney.Utils.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PriorMoney.DataImport.CsvImport
{
    public class CsvCardOperationParser : ICardOperationParser
    {
        private readonly IDateRangeParser _dateRangeParser;

        public CsvCardOperationParser(IDateRangeParser dateRangeParser)
        {
            _dateRangeParser = dateRangeParser;
        }

        public CardOperation[] Parse(string csvStringToImportFrom)
        {
            Throw.If.String.IsNullOrWhiteSpace(csvStringToImportFrom, nameof(CsvCardOperationParser), nameof(Parse), nameof(csvStringToImportFrom));

            List<CardOperation> operations = new List<CardOperation>();

            GetReleasedOperations(operations, csvStringToImportFrom);
            GetBlockedOperations(operations, csvStringToImportFrom);

            var reportPeriod = GetReportPeriod(csvStringToImportFrom);
            operations = operations.Where(op => op.DateTime >= reportPeriod.StartDate && op.DateTime <= reportPeriod.EndDate).ToList();

            return operations.ToArray();
        }

        private void ValidateParameter(string csvStringToImportFrom)
        {
            throw new NotImplementedException();
        }

        private DateRange GetReportPeriod(string csvStringToImportFrom)
        {
            return _dateRangeParser.Parse(csvStringToImportFrom);
        }

        private void GetBlockedOperations(List<CardOperation> operations, string csvStringToImportFrom)
        {
            // Get string blocks from csv string that contain operations info
            var operationLineBlockRegex = new Regex(@"Заблокированные суммы по.*\n((.*\n)*)");
            var operationLineBlockMatches = operationLineBlockRegex.Matches(csvStringToImportFrom);

            if (operationLineBlockMatches.Count != 0)
            {
                // Process each data block
                foreach (Match operationBlockMatch in operationLineBlockMatches)
                {
                    var headerRowsToSkip = 1;
                    var operationLinesGroupIndex = 2;
                    var operationLines = operationBlockMatch
                        .Groups[operationLinesGroupIndex].Captures
                        .Cast<Capture>()
                        .Skip(headerRowsToSkip)
                        .Select(capture => capture.ToString())
                        .Where(l => !string.IsNullOrWhiteSpace(l));

                    // Single operation line regex
                    var operationLineRegex = new Regex("(.*;){8}");

                    // Populate result array with CardOperation objects created based on captured string lines
                    foreach (var operationLine in operationLines)
                    {
                        int cellGroupIndex = 1;
                        var cells = operationLineRegex.Match(operationLine)
                            .Groups[cellGroupIndex].Captures
                            .Cast<Capture>()
                            .Select(capt => capt.ToString().Replace(";", ""))
                            .ToArray();

                        // Parse data
                        var newCardOperation = new CardOperation();
                        newCardOperation.DateTime = DateTime.ParseExact(cells[0], "dd.MM.yyyy HH:mm:ss", null, DateTimeStyles.AssumeLocal);
                        newCardOperation.OriginalName = cells[1];
                        newCardOperation.Amount = -ParseDecimal(cells[2]);
                        newCardOperation.Currency = (Currency)Enum.Parse(typeof(Currency), cells[3]);

                        operations.Add(newCardOperation);
                    }
                }
            }
        }

        private decimal ParseDecimal(string str)
        {
            try
            {
                return decimal.Parse(str.Replace(',', '.').Replace(" ", ""));
            }
            catch
            {
                Console.WriteLine($"Can't parse decimal value '{str}'");
                throw;
            }
        }

        private void GetReleasedOperations(List<CardOperation> operations, string csvStringToImportFrom)
        {
            // Get string blocks from csv string that contain operations info
            var operationLineBlockRegex = new Regex(@"Операции по.*\n(((?!Всего по контракту).*\n)*)[\n]{0,1}Всего по контракту");
            var operationLineBlockMatches = operationLineBlockRegex.Matches(csvStringToImportFrom);

            if (operationLineBlockMatches.Count != 0)
            {
                // Process each data block
                foreach (Match operationBlockMatch in operationLineBlockMatches)
                {
                    var headerRowsToSkip = 1;
                    var operationLinesGroupIndex = 2;
                    var operationLines = operationBlockMatch
                        .Groups[operationLinesGroupIndex].Captures
                        .Cast<Capture>()
                        .Skip(headerRowsToSkip)
                        .Select(capture => capture.ToString())
                        .Where(l => !string.IsNullOrWhiteSpace(l));

                    // Single operation line regex
                    var operationLineRegex = new Regex("(.*;){9}");

                    // Populate result array with CardOperation objects created based on captured string lines
                    foreach (var operationLine in operationLines)
                    {
                        int cellGroupIndex = 1;
                        var cells = operationLineRegex.Match(operationLine)
                            .Groups[cellGroupIndex].Captures
                            .Cast<Capture>()
                            .Select(capt => capt.ToString().Replace(";", ""))
                            .ToArray();

                        // Parse data
                        var newCardOperation = new CardOperation();
                        newCardOperation.DateTime = DateTime.ParseExact(cells[0], "dd.MM.yyyy HH:mm:ss", null, DateTimeStyles.AssumeLocal);
                        newCardOperation.OriginalName = cells[1];
                        newCardOperation.Amount = ParseDecimal(cells[2]);
                        newCardOperation.Currency = (Currency)Enum.Parse(typeof(Currency),cells[3]);

                        operations.Add(newCardOperation);
                    }
                }
            }
        }
    }
}
