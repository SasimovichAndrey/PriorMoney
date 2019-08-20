using PriorMoney.Model;

public class CardOperationStringView : IModelStringView<CardOperation>
{
    public string GetView(CardOperation model)
    {
        var categories = model.Categories != null ? string.Join(", ", model.Categories) : string.Empty;
        return $"{model.UserDefinedName ?? model.OriginalName} {model.DateTime.ToLocalTime().ToString("dd/MM/yyyy HH:mm")} {model.Amount} [{categories}]";
    }
}