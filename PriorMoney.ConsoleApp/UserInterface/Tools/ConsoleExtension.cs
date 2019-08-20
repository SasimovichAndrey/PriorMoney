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
            string userInput;
            var success = false;
            var result = 0;

            while (!success)
            {
                userInput = Console.ReadLine();
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
                    if (string.IsNullOrWhiteSpace(userInput))
                    {
                        strings = new List<string>();
                    }
                    else
                    {
                        strings = userInput.Trim()
                            .Split(',')
                            .ToList();
                    }

                    success = true;
                }
                catch
                {
                    Console.WriteLine(FAILURE_RETRY_MESSAGE);
                }
            }


            return strings;
        }

        internal static decimal ReadDecimalOrRetry(decimal min = decimal.MinValue, decimal max = decimal.MaxValue, bool allowEmpty = true)
        {
            var userInput = Console.ReadLine();
            var success = false;
            decimal result = 0;

            while (!success)
            {
                if (decimal.TryParse(userInput, out result) && result >= min && result <= max)
                {
                    success = true;
                }
                else
                {
                    if (string.Empty == userInput && allowEmpty)
                    {
                        result = 0;
                        success = true;
                    }
                    else
                    {
                        Console.WriteLine(FAILURE_RETRY_MESSAGE);
                    }
                }
            }

            return result;
        }

        public static List<int> ReadIntRangeListOrRetry(bool allowRange = false)
        {
            bool success = false;
            List<int> numbers = new List<int>();

            Console.WriteLine("Введите номера через запятую");
            while (!success)
            {
                var userInput = Console.ReadLine();

                try
                {
                    var inputSplittedByComma = userInput.Split(',');
                    foreach (var part in inputSplittedByComma)
                    {
                        if (part.Contains('-'))
                        {
                            numbers.AddRange(GetRangeFromUserInput(part));
                        }
                        else
                        {
                            numbers.Add(int.Parse(part.Trim()));
                        }
                    }

                    success = true;
                }
                catch
                {
                    Console.WriteLine(FAILURE_RETRY_MESSAGE);
                }
            }


            return numbers;
        }

        private static IEnumerable<int> GetRangeFromUserInput(string part)
        {
            var indexOfDelimeter = part.IndexOf('-');
            var leftInt = int.Parse(part.Substring(0, indexOfDelimeter).Trim());
            var rightInt = int.Parse(part.Substring(indexOfDelimeter + 1, part.Length - indexOfDelimeter - 1));

            if (leftInt > rightInt)
            {
                throw new Exception($"Invalid int range {part}. The left boundary cannot be less than right one");
            }

            var result = new int[rightInt - leftInt + 1];
            for (var i = 0; i < result.Length; i++)
            {
                result[i] = leftInt + i;
            }

            return result;
        }
    }
}
