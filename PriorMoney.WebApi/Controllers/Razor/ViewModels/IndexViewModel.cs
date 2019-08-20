using System.Collections.Generic;
using System.Threading.Tasks;
using PriorMoney.Model;

namespace PriorMoney.WebApp.Controllers.Razor.Models
{
    public class IndexViewModel
    {
        public List<CardOperation> LastOperations { get; set; }

        public IndexViewModel(List<CardOperation> lastOperations)
        {
            this.LastOperations = lastOperations;
        }
    }
}