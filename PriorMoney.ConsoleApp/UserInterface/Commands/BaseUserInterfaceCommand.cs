using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PriorMoney.ConsoleApp.UserInterface.Commands.Model;

namespace PriorMoney.ConsoleApp.UserInterface.Commands
{
    public abstract class BaseUserInterfaceCommand
    {
        protected void RenderMenu(IEnumerable<MenuCommandItem> menuItems)
        {
            var itemCount = menuItems.Count();
            for (int itemIndex = 0; itemIndex < itemCount; itemIndex++)
            {
                Console.WriteLine($"{itemIndex + 1}. {menuItems.ElementAt(itemIndex).MenuItemLabel}");
            }
        }

        protected object GetCommandFromUserInput(IEnumerable<MenuCommandItem> menuItems)
        {
            var userInput = Console.ReadLine();

            int parsedUserInput;
            if (int.TryParse(userInput, out parsedUserInput))
            {
                return menuItems.ElementAt(parsedUserInput - 1).Command;
            }
            else
            {
                return null;
            }
        }
    }
}