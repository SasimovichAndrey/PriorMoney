using System;
using System.Collections.Generic;
using System.Linq;

namespace PriorMoney.ConsoleApp.UserInterface.Tools
{
    public static class ConsoleExtensions
    {
        private const string FAILURE_RETRY_MESSAGE = "Шота не так. Попробуй ещё раз";

        public static bool AskYesNo(string question)
        {

            var userInput = ' ';
            while (userInput != 'y' && userInput != 'n')
            {

                Console.Write($"{question} (y/n) ");
                userInput = Console.ReadKey().KeyChar;
            }

            Console.WriteLine();
            return userInput == 'y';
        }

        public static int ReadIntOrRetry(int min = 0, int max = int.MaxValue)
        {
            var userInput = Console.ReadLine();
            var success = false;
            var result = 0;

            while (!success)
            {
                if (int.TryParse(userInput, out result) && result >= min && result <= max)
                {
                    success = true;
                }
                else
                {
                    Console.WriteLine(FAILURE_RETRY_MESSAGE);
                }
            }

            return result;
        }

        internal static List<string> ReadStringListOrRetry()
        {
            bool success = false;
            List<string> strings = null;

            Console.WriteLine("Введите имена через запятую");
            while (!success)
            {
                var userInput = Console.ReadLine();

                try
                {
                    strings = userInput.Trim()
                        .Split(',')
                        .ToList();

                    success = true;
                }
                catch
                {
                    Console.WriteLine(FAILURE_RETRY_MESSAGE);
                }
            }


            return strings;
        }

        public static List<int> ReadIntListOrRetry()
        {
            bool success = false;
            List<int> numbers = null;

            Console.WriteLine("Введите номера через запятую");
            while (!success)
            {
                var userInput = Console.ReadLine();

                try
                {
                    numbers = userInput.Trim()
                        .Split(',')
                        .Select(str => int.Parse(str.Trim()))
                        .ToList();

                    success = true;
                }
                catch
                {
                    Console.WriteLine(FAILURE_RETRY_MESSAGE);
                }
            }


            return numbers;
        }
    }
}
