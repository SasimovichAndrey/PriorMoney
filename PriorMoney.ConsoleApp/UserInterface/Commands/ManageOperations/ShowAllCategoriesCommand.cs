using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PriorMoney.Storage.Interface;

namespace PriorMoney.ConsoleApp.UserInterface.Commands.ManageOperations
{
    class ShowAllCategoriesCommand : BaseUserInterfaceCommand, IUserInterfaceCommand
    {
        private readonly IDbLogicManager _dbLogicManager;

        public ShowAllCategoriesCommand(IDbLogicManager dbLogicManager)
        {
            this._dbLogicManager = dbLogicManager;
        }

        public async Task ExecuteAsync()
        {
            var categories = await _dbLogicManager.GetAllCategories();

            var tmp = categories[0] == categories[5];

            RenderCategories(categories);
        }

        private void RenderCategories(List<string> categories)
        {
            foreach (var cat in categories)
            {
                Console.Write($"\"{cat}\" | ");
            }

            Console.WriteLine();
        }
    }
}