using System;

namespace PriorMoney.Model
{
    public interface IHasId
    {
        Guid Id { get; set; }
    }
}