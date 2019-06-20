using NUnit.Framework;
using PriorMoney.DataImport.CsvImport;
using PriorMoney.DataImport.CsvImport.Parsers;
using PriorMoney.DataImport.Interface;
using PriorMoney.Model;
using PriorMoney.Utils.Models;
using System;
using System.IO;
using System.Text;

namespace PriorMoney.DataImport.Tests
{
    [TestFixture]
    public class CsvCardOperationImporterTests
    {
        private string _csvString;
        private ICardOperationParser _parser;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var csvFilePath = @"Data/test.csv";
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            using (var streamReader = new StreamReader(File.Open(csvFilePath, FileMode.Open), Encoding.GetEncoding(1251)))
            {
                _csvString = streamReader.ReadToEnd();
            }
        }

        [SetUp]
        public void SetUp()
        {
            _parser = new CsvCardOperationParser(new DateRangeParserMock(new DateRange() { StartDate = new DateTime(2019, 6, 20), EndDate = new DateTime(2019, 6, 20, 23, 59, 59) }));
        }

        [Test]
        public void ImportTest()
        {
            CardOperation[] expectedData = new CardOperation[]
            {
                new CardOperation
                {
                    Amount = 0.37m,
                    Currency = Currency.BYN,
                    DateTime = new DateTime(2019, 6,20),
                    OriginalName = "Поступление на контракт клиента 749103-00081-044753  "
                },
                new CardOperation
                {
                    Amount = 100m,
                    Currency = Currency.BYN,
                    DateTime = new DateTime(2019, 6,20),
                    OriginalName = "Поступление на контракт клиента 749103-00081-044753  "
                },
                new CardOperation
                {
                    Amount = -2m,
                    Currency = Currency.BYN,
                    DateTime = new DateTime(2019, 6,20),
                    OriginalName = "Retail BLR GRODNO PT MINI KAFE TVISTER  "
                },
                new CardOperation
                {
                    Amount = -33.6m,
                    Currency = Currency.BYN,
                    DateTime = new DateTime(2019, 6,20),
                    OriginalName = "Retail BLR GRODNO AZS N 46  "
                },
                new CardOperation
                {
                    Amount = -4.80m,
                    Currency = Currency.BYN,
                    DateTime = new DateTime(2019, 6,20, 20, 31,20),
                    OriginalName = "Retail NLD AMSTERDAM YANDEX.TAXI"
                },
                new CardOperation
                {
                    Amount = -2.70m,
                    Currency = Currency.BYN,
                    DateTime = new DateTime(2019, 6,20, 0, 59, 27),
                    OriginalName = "Retail NLD AMSTERDAM YANDEX.TAXI"
                },
            };

            CardOperation[] actualData = _parser.Parse(_csvString);

            Func<CardOperation, CardOperation, bool> cardOperationComparer = (op1, op2) =>
            {
                if (op1.Amount != op2.Amount) return false;
                if (op1.Currency != op2.Currency) return false;
                if (op1.DateTime != op2.DateTime) return false;
                if (op1.UserDefinedName != op2.UserDefinedName) return false;
                if (op1.OriginalName != op2.OriginalName) return false;

                return true;
            };
            Assert.That(actualData, Is.EquivalentTo(expectedData).Using(cardOperationComparer));
        }
    }

    public class DateRangeParserMock : IDateRangeParser
    {
        private readonly DateRange dateRange;

        public DateRangeParserMock(DateRange dateRange)
        {
            this.dateRange = dateRange;
        }
        public DateRange Parse(string str)
        {
            return dateRange;
        }
    }
}
