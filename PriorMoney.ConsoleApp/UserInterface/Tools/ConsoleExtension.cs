using System;

namespace PriorMoney.ConsoleApp.UserInterface.Tools
{
    public static class ConsoleExtensions
    {
        public static bool AskYesNo(string question)
        {
            
            var userInput = ' ';
            while(userInput != 'y' && userInput !='n'){

                Console.Write($"{question} (y/n) ");
                userInput = Console.ReadKey().KeyChar;
            }

            Console.WriteLine();
            return userInput == 'y';
        }
    }
}