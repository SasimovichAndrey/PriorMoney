using PriorMoney.Model;

public class CardOperationStringView : IModelStringView<CardOperation>
{
    public string GetView(CardOperation model)
    {
        return $"{model.Name} {model.DateTime} {model.Amount}";
    }
}