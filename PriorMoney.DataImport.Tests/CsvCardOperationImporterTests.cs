using NUnit.Framework;
using PriorMoney.DataImport.CsvImport;
using PriorMoney.DataImport.Interface;
using PriorMoney.Model;
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
            _parser = new CsvCardOperationParser();
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
                    DateTime = new DateTime(2019, 5,31),
                    UserDefinedName = "Поступление на контракт клиента 749103-00081-044753  "
                },
                new CardOperation
                {
                    Amount = 100m,
                    Currency = Currency.BYN,
                    DateTime = new DateTime(2019, 5,23),
                    UserDefinedName = "Поступление на контракт клиента 749103-00081-044753  "
                },
                new CardOperation
                {
                    Amount = -2m,
                    Currency = Currency.BYN,
                    DateTime = new DateTime(2019, 5,29),
                    UserDefinedName = "Retail BLR GRODNO PT MINI KAFE TVISTER  "
                },
                new CardOperation
                {
                    Amount = -33.6m,
                    Currency = Currency.BYN,
                    DateTime = new DateTime(2019, 5,29),
                    UserDefinedName = "Retail BLR GRODNO AZS N 46  "
                },
                new CardOperation
                {
                    Amount = -4.26m,
                    Currency = Currency.BYN,
                    DateTime = new DateTime(2019, 5,28),
                    UserDefinedName = "Retail BLR GRODNO SHOP \"LIMOZH\"  "
                },
                new CardOperation
                {
                    Amount = -5.58m,
                    Currency = Currency.BYN,
                    DateTime = new DateTime(2019, 5,28),
                    UserDefinedName = "Retail BLR GRODNO SHOP \"LIMOZH\"  "
                },
            };

            CardOperation[] actualData = _parser.Parse(_csvString);

            Func<CardOperation, CardOperation, bool> cardOperationComparer = (op1, op2) =>
            {
                if (op1.Amount != op2.Amount) return false;
                if (op1.Currency != op2.Currency) return false;
                if (op1.DateTime != op2.DateTime) return false;
                if (op1.UserDefinedName != op2.UserDefinedName) return false;

                return true;
            };
            Assert.That(actualData, Is.EquivalentTo(expectedData).Using(cardOperationComparer));
        }

        [Test]
        public void BadDataTest()
        {
            var badCsvString = "bad string";

            TestDelegate importAction = () => _parser.Parse(badCsvString);

            Assert.Throws<ArgumentException>(importAction);
        }
    }
}
