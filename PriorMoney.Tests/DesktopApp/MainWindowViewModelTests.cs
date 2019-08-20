using AutoMapper;
using Moq;
using NUnit.Framework;
using PriorMoney.DesktopApp.Model;
using PriorMoney.DesktopApp.ViewModel;
using PriorMoney.Model;
using PriorMoney.Storage.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PriorMoney.Tests.DesktopApp
{
    [TestFixture]
    public class MainWindowViewModelTests
    {
        [SetUp]
        public void SetUp()
        {
            
            
        }

        [Test]
        public async Task LoadDataTest()
        {
            var dateTimeNow = DateTime.Now;
            var hardCodedOperations = new List<CardOperation>()
            {
                new CardOperation
                {
                    UserDefinedName = "First operation",
                    Amount = -30,
                    Currency = Currency.BYN,
                    DateTime = dateTimeNow.AddDays(-2),
                    Categories = new HashSet<string>() {"еда", "продукты"}
                },
                new CardOperation
                {
                    UserDefinedName = "Second operation",
                    Amount = -35,
                    Currency = Currency.BYN,
                    DateTime = dateTimeNow.AddDays(-1),
                    Categories = new HashSet<string>() {"машина"}
                }
            };

            var dbLogicManagerMock = new Mock<IDbLogicManager>();
            dbLogicManagerMock.Setup(o => o.GetLastOperations(It.IsAny<int>())).Returns(Task.FromResult<List<CardOperation>>(hardCodedOperations));

            var dbLogicManager = dbLogicManagerMock.Object;

            var mapperMock = new Mock<IMapper>();
            List<CardOperationModel> expectedCardOperations = new List<CardOperationModel>()
            {
                new CardOperationModel
                {
                    UserDefinedName = "First operation",
                    Amount = -30,
                    Currency = Currency.BYN,
                    DateTime = dateTimeNow.AddDays(-2),
                    Categories = new HashSet<string>() {"еда", "продукты"}
                },
                new CardOperationModel
                {
                    UserDefinedName = "Second operation",
                    Amount = -35,
                    Currency = Currency.BYN,
                    DateTime = dateTimeNow.AddDays(-1),
                    Categories = new HashSet<string>() {"машина"}
                }
            };
            mapperMock.Setup(m => m.Map<CardOperationModel>(hardCodedOperations[0])).Returns(expectedCardOperations[0]);
            mapperMock.Setup(m => m.Map<CardOperationModel>(hardCodedOperations[1])).Returns(expectedCardOperations[1]);

            // todo mock mapper logic

            var mapper = mapperMock.Object;

            var mainWindowVM = new MainWindowViewModel(dbLogicManager, mapper);

            await mainWindowVM.LoadData();

            Func<CardOperationModel, CardOperationModel, bool> comparer = (CardOperationModel m1, CardOperationModel m2) =>
            {
                if(m1.UserDefinedName != m2.UserDefinedName)
                {
                    return false;
                }
                if(m1.Amount != m2.Amount)
                {
                    return false;
                }
                if(m1.Currency != m2.Currency)
                {
                    return false;
                }
                if(m1.DateTime != m2.DateTime)
                {
                    return false;
                }
                if(Is.EquivalentTo(m1.Categories).ApplyTo(m2).IsSuccess != true)
                {
                    return false;
                }

                return true;
            };

            Assert.That(mainWindowVM.CardOperations, Is.EquivalentTo(expectedCardOperations).Using(comparer));
            //throw new NotImplementedException();
        }
    }
}
