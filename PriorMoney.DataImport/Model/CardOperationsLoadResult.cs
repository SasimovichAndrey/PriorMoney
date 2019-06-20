using System.Collections.Generic;
using PriorMoney.Model;

namespace PriorMoney.DataImport.Model
{
    public class CardOperationsLoadResult
    {
        public List<CardOperation> Operations { get; set; }
        public string FileName { get; set; }
    }
}