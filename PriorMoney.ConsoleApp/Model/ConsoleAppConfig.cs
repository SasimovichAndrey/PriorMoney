namespace PriorMoney.ConsoleApp.Model
{
    public class ConsoleAppConfig
    {
        public string OperationsReportsDataFolderPath { get; internal set; }
        public string MongoConnectionString { get; internal set; }
        public string MongoDbName { get; internal set; }
    }
}