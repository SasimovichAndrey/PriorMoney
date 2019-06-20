using PriorMoney.Model;

public class CardOperationStringView : IModelStringView<CardOperation>
{
    public string GetView(CardOperation model)
    {
        return $"{model.UserDefinedName ?? model.OriginalName} {model.DateTime} {model.Amount}";
    }
}