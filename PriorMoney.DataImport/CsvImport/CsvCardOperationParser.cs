using PriorMoney.DataImport.Interface;
using PriorMoney.Model;
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
        public CardOperation[] Parse(string csvStringToImportFrom)
        {
            List<CardOperation> operations = new List<CardOperation>();

            // Get string blocks from csv string that contain operations info
            var operationLineBlockRegex = new Regex(@"Операции по.*\n(((?!Всего по контракту).*\n)*)[\n]{0,1}Всего по контракту");
            var operationLineBlockMatches = operationLineBlockRegex.Matches(csvStringToImportFrom);

            if (operationLineBlockMatches.Count == 0)
            {
                throw new ArgumentException("The file doesn't contain operation data in acceptable format");
            }

            // Process each data block
            foreach (Match operationBlockMatch in operationLineBlockMatches)
            {
                var headerRowsToSkip = 1;
                var operationLinesGroupIndex = 2;
                var operationLines = operationBlockMatch
                    .Groups[operationLinesGroupIndex].Captures
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
                        .Select(capt => capt.ToString().Replace(";", ""))
                        .ToArray();

                    // Parse data
                    var newCardOperation = new CardOperation();
                    newCardOperation.DateTime = DateTime.ParseExact(cells[0], "dd.MM.yyyy HH:mm:ss", null);
                    newCardOperation.Name = cells[1];
                    newCardOperation.Amount = decimal.Parse(cells[2].Replace(',', '.'));
                    newCardOperation.Currency = Enum.Parse<Currency>(cells[3]);

                    operations.Add(newCardOperation);
                }
            }

            return operations.ToArray();
        }
    }
}
