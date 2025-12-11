using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace StockBrokerProject.Models
{
    public class Portfolio : INotifyPropertyChanged
    {
        private decimal _cashBalance;
        private decimal _totalValue;
        private decimal _totalGainLoss;

        public decimal CashBalance
        {
            get => _cashBalance;
            set { if (value != _cashBalance) { _cashBalance = value; OnPropertyChanged(nameof(CashBalance)); } }
        }

        public decimal TotalValue
        {
            get => _totalValue;
            set { if (value != _totalValue) { _totalValue = value; OnPropertyChanged(nameof(TotalValue)); } }
        }

        public decimal TotalGainLoss
        {
            get => _totalGainLoss;
            set { if (value != _totalGainLoss) { _totalGainLoss = value; OnPropertyChanged(nameof(TotalGainLoss)); } }
        }

        public List<Position> Positions { get; set; } = new();
        public List<Transaction> TransactionHistory { get; set; } = new();

        public Portfolio()
        {
            CashBalance = 100000m; 
            TotalValue = 100000m;
            TotalGainLoss = 0m;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public class Position
    {
        public string Symbol { get; set; } = string.Empty;
        public int Shares { get; set; }
        public decimal AverageCost { get; set; }
        public decimal CurrentPrice { get; set; }
        
        public decimal TotalCost => Shares * AverageCost;
        public decimal CurrentValue => Shares * CurrentPrice;
        public decimal GainLoss => CurrentValue - TotalCost;
        public decimal GainLossPercent => TotalCost > 0 ? (GainLoss / TotalCost) * 100 : 0;
    }

    public class Transaction
    {
        public DateTime DateTime { get; set; }
        public string Type { get; set; } = string.Empty; 
        public string Symbol { get; set; } = string.Empty;
        public int Shares { get; set; }
        public decimal Price { get; set; }
        public decimal Total => Shares * Price;
    }
}
