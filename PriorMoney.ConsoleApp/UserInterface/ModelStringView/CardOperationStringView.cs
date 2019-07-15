using PriorMoney.Model;

public class CardOperationStringView : IModelStringView<CardOperation>
{
    public string GetView(CardOperation model)
    {
        var categories = string.Join(", ", model.Categories);
        return $"{model.UserDefinedName ?? model.OriginalName} {model.DateTime.ToString("dd/MM/yyyy HH:mm")} {model.Amount} [{categories}]";
    }
}