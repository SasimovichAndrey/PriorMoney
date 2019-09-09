using PriorMoney.DesktopApp.Model;
using PriorMoney.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriorMoney.DesktopApp.SampleData
{
    public class DesignTimeData
    {
        public List<CardOperationModel> CardOperations { get; set; } = new List<CardOperationModel>
        {
            new CardOperationModel
                {
                    UserDefinedName = "First operation",
                    Amount = -30.5m,
                    Currency = Currency.BYN,
                    DateTime = DateTime.Now.AddDays(-2),
                    Categories = new HashSet<string>() {"еда", "продукты"}
                },
                new CardOperationModel
                {
                    UserDefinedName = "Second operation",
                    Amount = -35,
                    Currency = Currency.BYN,
                    DateTime = DateTime.Now.AddDays(-1),
                    Categories = new HashSet<string>() {"машина"}
                }
        };

        public CardOperationModel CardOperation { get; set; } = new CardOperationModel
        {
            UserDefinedName = "Продукты",
            Amount = -30.54m,
            Currency = Currency.BYN,
            DateTime = DateTime.Now.AddDays(-2),
            Categories = new HashSet<string>() { "еда", "продукты" }
        };

        public List<string> AvailableCategories { get; set; } = new List<string>
        {
            "ПРОДУКТЫ",
            "ЕДА",
            "МАШИНА"
        };

        public bool IsBeingEdited { get; set; } = false;

        public bool IsNewCardOperationBeingAdded { get; set; } = true;

        public bool DataImported { get; set; } = false;
    }
}
