using System;

namespace PriorMoney.Model
{
    public class CardOperation
    {
        public DateTime DateTime { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public Currency Currency { get; set; }

        public override string ToString()
        {
            return $"{DateTime} {Name} {Amount} {Currency}";
        }
    }
}
