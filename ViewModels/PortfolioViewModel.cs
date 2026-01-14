using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using StockBrokerProject.Models;
using StockBrokerProject.Services;

namespace StockBrokerProject.ViewModels
{
    public class PortfolioViewModel : INotifyPropertyChanged
    {
        private readonly Portfolio _portfolio;
        private readonly MainViewModel _mainViewModel;

        public ObservableCollection<PositionViewModel> Positions { get; } = new();
        public ObservableCollection<Transaction> Transactions { get; } = new();

        public decimal TotalValue => _portfolio.TotalValue;
        public decimal TotalGainLoss => _portfolio.TotalGainLoss;
        public decimal TotalGainLossPercent => _portfolio.TotalValue > 0
            ? (_portfolio.TotalGainLoss / (_portfolio.TotalValue - _portfolio.TotalGainLoss)) * 100
            : 0;
        public decimal CashBalance => _portfolio.CashBalance;
        public decimal InvestedAmount => _portfolio.TotalValue - _portfolio.CashBalance;

        public ICommand ViewStockCommand { get; }

        public PortfolioViewModel(Portfolio portfolio, MainViewModel mainViewModel)
        {
            _portfolio = portfolio;
            _mainViewModel = mainViewModel;

            ViewStockCommand = new RelayCommand(param =>
            {
                if (param is PositionViewModel positionVM)
                {
                    _mainViewModel.NavigateToStock(positionVM.Symbol);
                }
            });

            _portfolio.PropertyChanged += (s, e) => RefreshData();
        }

        public void RefreshData()
        {
            if (_mainViewModel?.OverviewVM?.Stocks == null)
                return;

            Positions.Clear();
            foreach (var position in _portfolio.Positions)
            {
                var stock = _mainViewModel.OverviewVM.Stocks.FirstOrDefault(s => s.Symbol == position.Symbol);
                if (stock != null)
                {
                    position.CurrentPrice = stock.Price;
                }

                Positions.Add(new PositionViewModel(position));
            }

            Transactions.Clear();
            foreach (var transaction in _portfolio.TransactionHistory.OrderByDescending(t => t.DateTime))
            {
                Transactions.Add(transaction);
            }

            // Refresh summary stats
            OnPropertyChanged(nameof(TotalValue));
            OnPropertyChanged(nameof(TotalGainLoss));
            OnPropertyChanged(nameof(TotalGainLossPercent));
            OnPropertyChanged(nameof(CashBalance));
            OnPropertyChanged(nameof(InvestedAmount));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public class PositionViewModel : INotifyPropertyChanged
    {
        private readonly Position _position;

        public string Symbol => _position.Symbol;
        public int Shares => _position.Shares;
        public decimal AverageCost => _position.AverageCost;
        public decimal CurrentPrice => _position.CurrentPrice;
        public decimal TotalCost => _position.TotalCost;
        public decimal CurrentValue => _position.CurrentValue;
        public decimal GainLoss => _position.GainLoss;
        public decimal GainLossPercent => _position.GainLossPercent;

        public PositionViewModel(Position position)
        {
            _position = position;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}