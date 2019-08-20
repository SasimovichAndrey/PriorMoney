using System.Text;
using PriorMoney.ConsoleApp.Model;
using PriorMoney.Model;

public class CardOperationSetStatisticsStringView : IModelStringView<OperationSetStatistics>
{
    public string GetView(OperationSetStatistics operationsStatistics)
    {
        var strBuilder = new StringBuilder();
        strBuilder.AppendLine($"Всего потрачено: {operationsStatistics.TotalSpent}");
        strBuilder.AppendLine($"Всего получено: {operationsStatistics.TotalGot}");
        strBuilder.AppendLine($"Итого: {operationsStatistics.Saldo}");

        return strBuilder.ToString();
    }
}