using NUnit.Framework;
using PriorMoney.DataImport.CsvImport;
using PriorMoney.DataImport.CsvImport.Parsers;
using PriorMoney.DataImport.Interface;
using PriorMoney.Model;
using PriorMoney.Utils.Models;
using System;
using System.IO;
using System.Text;
using PriorMoney.Storage.Interface;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PriorMoney.Storage.Managers;

namespace PriorMoney.Tests
{
    [TestFixture]
    public class DbLogicManagerTests
    {
        private CardOperationStorageMock _cardOperationStorage;
        private DbLogicManager _dbLogicManager;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
        }

        [SetUp]
        public void SetUp()
        {
            _cardOperationStorage = new CardOperationStorageMock();
            _dbLogicManager = new DbLogicManager(_cardOperationStorage);
        }

        [Test]
        public async Task TestGetNameSuggestions()
        {
            var stubData = new List<CardOperation>()
            {
                new CardOperation{
                    OriginalName = "Some original name",
                    UserDefinedName = "Продукты"
                },
                new CardOperation{
                    OriginalName = "Some original name",
                    UserDefinedName = "Продукты"
                },
                new CardOperation{
                    OriginalName = "Some original name",
                    UserDefinedName = "Машина"
                },
            };
            _cardOperationStorage.SetData(stubData);

            var expectedSuggestions = new List<string>() { "Продукты", "Машина" };

            var actualSuggestions = await _dbLogicManager.GetOperationNameSuggestions("Some original name");

            Assert.That(expectedSuggestions, Is.EquivalentTo(actualSuggestions));
        }
    }

    public class CardOperationStorageMock : IStorage<CardOperation>
    {
        private List<CardOperation> _data;

        public CardOperationStorageMock()
        {
        }

        public Task Add(CardOperation entity)
        {
            throw new NotImplementedException();
        }

        public Task AddMany(IEnumerable<CardOperation> entities)
        {
            throw new NotImplementedException();
        }

        public Task<CardOperation> Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<CardOperation>> Get(Expression<Func<CardOperation, bool>> where)
        {
            return Task.FromResult<List<CardOperation>>(_data);
        }

        public Task<List<CardOperation>> Get(Expression<Func<CardOperation, bool>> where, int take, Expression<Func<CardOperation, object>> sortBy)
        {
            return Task.FromResult<List<CardOperation>>(_data);
        }

        public Task<List<CardOperation>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task UpdateManyAsync<TField>(List<CardOperation> operations, Expression<Func<CardOperation, TField>> field)
        {
            throw new NotImplementedException();
        }

        internal void SetData(List<CardOperation> stubData)
        {
            this._data = stubData;
        }
    }
}
