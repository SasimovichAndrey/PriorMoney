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
        private ICarOperationImporter<string> _importer;

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
            _importer = new CsvCardOperationImporter();
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
                    Name = "Поступление на контракт клиента 749103-00081-044753  "
                },
                new CardOperation
                {
                    Amount = 100m,
                    Currency = Currency.BYN,
                    DateTime = new DateTime(2019, 5,23),
                    Name = "Поступление на контракт клиента 749103-00081-044753  "
                },
                new CardOperation
                {
                    Amount = -2m,
                    Currency = Currency.BYN,
                    DateTime = new DateTime(2019, 5,29),
                    Name = "Retail BLR GRODNO PT MINI KAFE TVISTER  "
                },
                new CardOperation
                {
                    Amount = -33.6m,
                    Currency = Currency.BYN,
                    DateTime = new DateTime(2019, 5,29),
                    Name = "Retail BLR GRODNO AZS N 46  "
                },
                new CardOperation
                {
                    Amount = -4.26m,
                    Currency = Currency.BYN,
                    DateTime = new DateTime(2019, 5,28),
                    Name = "Retail BLR GRODNO SHOP \"LIMOZH\"  "
                },
                new CardOperation
                {
                    Amount = -5.58m,
                    Currency = Currency.BYN,
                    DateTime = new DateTime(2019, 5,28),
                    Name = "Retail BLR GRODNO SHOP \"LIMOZH\"  "
                },
            };

            CardOperation[] actualData = _importer.Import(_csvString);

            Func<CardOperation, CardOperation, bool> cardOperationComparer = (op1, op2) =>
            {
                if (op1.Amount != op2.Amount) return false;
                if (op1.Currency != op2.Currency) return false;
                if (op1.DateTime != op2.DateTime) return false;
                if (op1.Name != op2.Name) return false;

                return true;
            };
            Assert.That(actualData, Is.EquivalentTo(expectedData).Using(cardOperationComparer));
        }

        [Test]
        public void BadDataTest()
        {
            var badCsvString = "bad string";

            TestDelegate importAction = () => _importer.Import(badCsvString);

            Assert.Throws<ArgumentException>(importAction);
        }
    }
}
