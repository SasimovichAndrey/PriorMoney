using System;

namespace PriorMoney.ConsoleApp.UserInterface.Tools
{
    public static class ConsoleExtensions
    {
        public static bool AskYesNo(string question)
        {
            Console.Write($"{question} (y/n) ");
            var userInput = Console.ReadKey().KeyChar;

            Console.WriteLine();
            return userInput == 'y';
        }
    }
}