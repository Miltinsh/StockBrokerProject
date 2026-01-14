using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using StockBrokerProject.Models;

namespace StockBrokerProject.ViewModels
{
    public class StockInfoViewModel : INotifyPropertyChanged
    {
        private readonly Portfolio? _portfolio;
        private readonly MainViewModel? _mainViewModel;
        
        private string _symbol = string.Empty;
        private string _name = string.Empty;
        private decimal _price;
        private decimal _change;
        private decimal _changePercent;
        private string _sector = string.Empty;
        private string _marketCap = string.Empty;
        private string _volume = string.Empty;
        private string _details = string.Empty;
        
        private int _sharesToTrade = 1;
        private bool _isBuyMode = true;

        public string Symbol
        {
            get => _symbol;
            set { if (value != _symbol) { _symbol = value; OnPropertyChanged(nameof(Symbol)); } }
        }

        public string Name
        {
            get => _name;
            set { if (value != _name) { _name = value; OnPropertyChanged(nameof(Name)); } }
        }

        public decimal Price
        {
            get => _price;
            set 
            { 
                if (value != _price) 
                { 
                    _price = value; 
                    OnPropertyChanged(nameof(Price));
                    OnPropertyChanged(nameof(TotalCost));
                } 
            }
        }

        public decimal Change
        {
            get => _change;
            set { if (value != _change) { _change = value; OnPropertyChanged(nameof(Change)); } }
        }

        public decimal ChangePercent
        {
            get => _changePercent;
            set { if (value != _changePercent) { _changePercent = value; OnPropertyChanged(nameof(ChangePercent)); } }
        }

        public string Sector
        {
            get => _sector;
            set { if (value != _sector) { _sector = value; OnPropertyChanged(nameof(Sector)); } }
        }

        public string MarketCap
        {
            get => _marketCap;
            set { if (value != _marketCap) { _marketCap = value; OnPropertyChanged(nameof(MarketCap)); } }
        }

        public string Volume
        {
            get => _volume;
            set { if (value != _volume) { _volume = value; OnPropertyChanged(nameof(Volume)); } }
        }

        public string Details
        {
            get => _details;
            set { if (value != _details) { _details = value; OnPropertyChanged(nameof(Details)); } }
        }

        public int SharesToTrade
        {
            get => _sharesToTrade;
            set 
            { 
                if (value != _sharesToTrade && value > 0) 
                { 
                    _sharesToTrade = value; 
                    OnPropertyChanged(nameof(SharesToTrade));
                    OnPropertyChanged(nameof(TotalCost));
                    OnPropertyChanged(nameof(TradeButtonText));
                } 
            }
        }

        public bool IsBuyMode
        {
            get => _isBuyMode;
            set 
            { 
                if (value != _isBuyMode) 
                { 
                    _isBuyMode = value; 
                    OnPropertyChanged(nameof(IsBuyMode));
                    OnPropertyChanged(nameof(IsSellMode));
                    OnPropertyChanged(nameof(TradeButtonText));
                } 
            }
        }

        public bool IsSellMode => !IsBuyMode;

        public decimal TotalCost => SharesToTrade * Price;

        public string TradeButtonText => IsBuyMode 
            ? $"Buy {SharesToTrade} Share{(SharesToTrade > 1 ? "s" : "")}" 
            : $"Sell {SharesToTrade} Share{(SharesToTrade > 1 ? "s" : "")}";

        public int SharesOwned
        {
            get
            {
                if (_portfolio == null) return 0;
                var position = _portfolio.Positions.FirstOrDefault(p => p.Symbol == Symbol);
                return position?.Shares ?? 0;
            }
        }

        public decimal AveragePrice
        {
            get
            {
                if (_portfolio == null) return 0;
                var position = _portfolio.Positions.FirstOrDefault(p => p.Symbol == Symbol);
                return position?.AverageCost ?? 0;
            }
        }

        public decimal PositionValue => SharesOwned * Price;

        public decimal PositionGainLoss
        {
            get
            {
                if (_portfolio == null || SharesOwned == 0) return 0;
                var position = _portfolio.Positions.FirstOrDefault(p => p.Symbol == Symbol);
                return position?.GainLoss ?? 0;
            }
        }

        public decimal PositionGainLossPercent
        {
            get
            {
                if (_portfolio == null || SharesOwned == 0) return 0;
                var position = _portfolio.Positions.FirstOrDefault(p => p.Symbol == Symbol);
                return position?.GainLossPercent ?? 0;
            }
        }

        public ICommand ExecuteTradeCommand { get; }
        public ICommand SetBuyModeCommand { get; }
        public ICommand SetSellModeCommand { get; }

        public StockInfoViewModel() 
        {
            ExecuteTradeCommand = new RelayCommand(_ => { }, _ => false);
            SetBuyModeCommand = new RelayCommand(_ => IsBuyMode = true);
            SetSellModeCommand = new RelayCommand(_ => IsBuyMode = false);
        }

        public StockInfoViewModel(Stock stock) : this()
        {
            if (stock is null) throw new ArgumentNullException(nameof(stock));
            _symbol = stock.Symbol;
            _name = stock.Name;
            _price = stock.Price;
            _change = stock.Change;
            _changePercent = stock.ChangePercent;
            _sector = stock.Sector;
            _marketCap = stock.MarketCap;
            _volume = stock.Volume;
            _details = stock.Details;
        }

        public StockInfoViewModel(Stock stock, Portfolio portfolio, MainViewModel mainViewModel) : this(stock)
        {
            _portfolio = portfolio;
            _mainViewModel = mainViewModel;
            
            ExecuteTradeCommand = new RelayCommand(_ => ExecuteTrade());
            
            if (_portfolio != null)
            {
                _portfolio.PropertyChanged += (s, e) => RefreshPositionInfo();
            }
        }

        private void ExecuteTrade()
        {
            if (_mainViewModel == null) return;
            
            var result = System.Windows.MessageBox.Show(
                $"{(IsBuyMode ? "Buy" : "Sell")} {SharesToTrade} share{(SharesToTrade > 1 ? "s" : "")} of {Symbol} at ${Price:N2}?\n\n" + $"Total: ${TotalCost:N2}", "Confirm Trade",
                System.Windows.MessageBoxButton.YesNo,
                System.Windows.MessageBoxImage.Question
            );
            
            if (result == System.Windows.MessageBoxResult.Yes)
            {
                _mainViewModel.ExecuteTrade(Symbol, SharesToTrade, Price, IsBuyMode);
                RefreshPositionInfo();
            }
        }

        private void RefreshPositionInfo()
        {
            OnPropertyChanged(nameof(SharesOwned));
            OnPropertyChanged(nameof(AveragePrice));
            OnPropertyChanged(nameof(PositionValue));
            OnPropertyChanged(nameof(PositionGainLoss));
            OnPropertyChanged(nameof(PositionGainLossPercent));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
