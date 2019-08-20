using System;
using System.Collections.Generic;
using System.Linq;
using PriorMoney.Model;

namespace PriorMoney.ConsoleApp.Model
{
    public class OperationSetStatistics
    {
        private IEnumerable<CardOperation> _operations;
        private Lazy<decimal> _totalSpent;
        private Lazy<decimal> _totalGot;
        private Lazy<decimal> _saldo;

        public OperationSetStatistics(IEnumerable<CardOperation> operations)
        {
            _operations = operations;
            _totalSpent = new Lazy<decimal>(() => _operations.Sum(op => op.Amount < 0 ? op.Amount : 0));
            _totalGot = new Lazy<decimal>(() => _operations.Sum(op => op.Amount > 0 ? op.Amount : 0));
            _saldo = new Lazy<decimal>(() => _totalSpent.Value + _totalGot.Value);
        }

        public decimal TotalSpent { get { return _totalSpent.Value; } }
        public decimal TotalGot { get { return _totalGot.Value; } }
        public decimal Saldo { get { return _saldo.Value; } }
    }

}