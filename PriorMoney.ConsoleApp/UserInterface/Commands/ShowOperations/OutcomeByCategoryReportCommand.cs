using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PriorMoney.ConsoleApp.Model;
using PriorMoney.ConsoleApp.UserInterface.Tools;
using PriorMoney.Model;
using PriorMoney.Storage.Interface;


namespace PriorMoney.ConsoleApp.UserInterface.Commands.ShowOperations
{
    public class OutcomeByCategoryReportCommand : BaseUserInterfaceCommand, IUserInterfaceCommand
    {
        private readonly IDbLogicManager _dbLogicManager;

        public OutcomeByCategoryReportCommand(IDbLogicManager dbLogicManager)
        {
            _dbLogicManager = dbLogicManager;
        }

        public async Task ExecuteAsync()
        {
            var startDate = DateTime.Now.Date.AddMonths(-1);
            var endDate = DateTime.Now.Date.AddDays(1);
            var operations = await _dbLogicManager.GetOperationsByPeriod(startDate, endDate);

            var allAvailableCategories = operations.SelectMany(op => op.Categories).ToHashSet();
            foreach (var cat in allAvailableCategories)
            {
                Console.Write(cat + " ");
            }
            Console.WriteLine();
            var categoriesChosenByUser = ConsoleExtensions.ReadStringListOrRetry().Select(c => c.Trim().ToUpper());

            var categorySums = new Dictionary<string, decimal>();
            foreach (var cat in categoriesChosenByUser)
            {
                var sum = operations.Where(op => op.Categories != null && op.Categories.Contains(cat)).Sum(c => c.Amount);
                categorySums[cat] = sum;
            }

            foreach (var kv in categorySums.OrderBy(kv => kv.Value))
            {
                Console.WriteLine($"{kv.Key}: {kv.Value}");
            }

            var chosenCategoriesSum = operations.Where(op => op.Categories != null && op.Categories.Any(c => categoriesChosenByUser.Contains(c))).Sum(op => op.Amount);
            Console.WriteLine($"Всего для выбранных категорий:{chosenCategoriesSum}");
        }


    }
}