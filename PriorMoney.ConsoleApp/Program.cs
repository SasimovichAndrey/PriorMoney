using System;
using System.Threading.Tasks;
using PriorMoney.ConsoleApp.Model;
using PriorMoney.ConsoleApp.UserInterface;

namespace PriorMoney.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var cfg = ReadConfig();

            InitApp(cfg);

            StartUserInterfaceAsync(cfg).Wait();
        }

        private async static Task StartUserInterfaceAsync(ConsoleAppConfig cfg)
        {
            IConsoleAppUserInterface userInterface = new DefaultUserInterface();
            await userInterface.StartAsync();
        }

        private static void InitApp(ConsoleAppConfig cfg)
        {
            // init app
        }

        private static ConsoleAppConfig ReadConfig()
        {
            return new ConsoleAppConfig();
        }
    }
}
