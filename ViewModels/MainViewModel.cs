 using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Threading;
using StockBrokerProject.Models;
using StockBrokerProject.Services;

namespace StockBrokerProject.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly DataService _dataService;
        private readonly PriceUpdateService _priceUpdateService;
        private readonly DispatcherTimer _priceUpdateTimer;
        
        private object? _currentViewModel;
        private Portfolio _portfolio;

        public object? CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                if (_currentViewModel != value)
                {
                    _currentViewModel = value;
                    OnPropertyChanged(nameof(CurrentViewModel));
                    OnPropertyChanged(nameof(IsDashboardActive));
                    OnPropertyChanged(nameof(IsMarketsActive));
                }
            }
        }

        public Portfolio Portfolio
        {
            get => _portfolio;
            set
            {
                if (_portfolio != value)
                {
                    _portfolio = value;
                    OnPropertyChanged(nameof(Portfolio));
                }
            }
        }

        public DashboardViewModel DashboardVM { get; }
        public OverviewViewModel OverviewVM { get; }
        
        public ICommand ShowDashboardCommand { get; }
        public ICommand ShowOverviewCommand { get; }
        public ICommand ShowStockInfoCommand { get; }
        public ICommand SaveDataCommand { get; }
        public ICommand ResetPortfolioCommand { get; }

        public bool IsDashboardActive => CurrentViewModel is DashboardViewModel;
        public bool IsMarketsActive => CurrentViewModel is OverviewViewModel;

        public MainViewModel()
        {
            _dataService = new DataService();
            _priceUpdateService = new PriceUpdateService();
            
            _portfolio = _dataService.LoadPortfolio();
            
            DashboardVM = new DashboardViewModel(_portfolio, _dataService);
            OverviewVM = new OverviewViewModel(_dataService);
            
            OverviewVM.MainViewModel = this;
            
            CurrentViewModel = DashboardVM;

            ShowDashboardCommand = new RelayCommand(_ => CurrentViewModel = DashboardVM);
            ShowOverviewCommand = new RelayCommand(_ => CurrentViewModel = OverviewVM);
            ShowStockInfoCommand = new RelayCommand(param =>
            {
                if (param is StockInfoViewModel stockVM)
                {
                    var tradeableStockVM = new StockInfoViewModel(
                        new Stock(
                            stockVM.Symbol,
                            stockVM.Name,
                            stockVM.Price,
                            stockVM.Change,
                            stockVM.ChangePercent,
                            stockVM.Sector,
                            stockVM.MarketCap,
                            stockVM.Volume,
                            stockVM.Details
                        ),
                        _portfolio,
                        this
                    );
                    CurrentViewModel = tradeableStockVM;
                }
                else if (OverviewVM.SelectedStock != null)
                {
                    CurrentViewModel = new StockInfoViewModel(
                        new Stock(
                            OverviewVM.SelectedStock.Symbol,
                            OverviewVM.SelectedStock.Name,
                            OverviewVM.SelectedStock.Price,
                            OverviewVM.SelectedStock.Change,
                            OverviewVM.SelectedStock.ChangePercent,
                            OverviewVM.SelectedStock.Sector,
                            OverviewVM.SelectedStock.MarketCap,
                            OverviewVM.SelectedStock.Volume,
                            OverviewVM.SelectedStock.Details
                        ),
                        _portfolio,
                        this
                    );
                }
            });
            SaveDataCommand = new RelayCommand(_ => SaveAllData());
            ResetPortfolioCommand = new RelayCommand(_ => ResetPortfolio());


            _priceUpdateTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(5)
            };
            _priceUpdateTimer.Tick += OnPriceUpdateTick;
            _priceUpdateTimer.Start();
        }

        private void OnPriceUpdateTick(object? sender, EventArgs e)
        {
            foreach (var stock in OverviewVM.Stocks)
            {
                decimal oldPrice = stock.Price;
                decimal newPrice = _priceUpdateService.UpdatePrice(oldPrice);
                
                decimal change = newPrice - oldPrice;
                decimal changePercent = oldPrice > 0 ? (change / oldPrice) * 100 : 0;
                
                stock.Price = newPrice;
                stock.Change = change;
                stock.ChangePercent = changePercent;
            }
            
            UpdatePortfolioValues();
            
            DashboardVM.RefreshData(OverviewVM.Stocks);
            
            SavePrices();
        }

        private void UpdatePortfolioValues()
        {
            decimal totalPositionValue = 0m;
            decimal totalCost = 0m;
            
            foreach (var position in _portfolio.Positions)
            {
                var stock = OverviewVM.Stocks.FirstOrDefault(s => s.Symbol == position.Symbol);
                if (stock != null)
                {
                    position.CurrentPrice = stock.Price;
                    totalPositionValue += position.CurrentValue;
                    totalCost += position.TotalCost;
                }
            }
            
            _portfolio.TotalValue = _portfolio.CashBalance + totalPositionValue;
            _portfolio.TotalGainLoss = totalPositionValue - totalCost;
        }

        public void ExecuteTrade(string symbol, int shares, decimal price, bool isBuy)
        {
            if (isBuy)
            {
                ExecuteBuy(symbol, shares, price);
            }
            else
            {
                ExecuteSell(symbol, shares, price);
            }
            
            UpdatePortfolioValues();
            DashboardVM.RefreshData(OverviewVM.Stocks);
            SaveAllData();
        }

        private void ExecuteBuy(string symbol, int shares, decimal price)
        {
            decimal total = shares * price;
            
            if (_portfolio.CashBalance < total)
            {
                System.Windows.MessageBox.Show(
                    $"Insufficient funds! You need ${total:N2} but only have ${_portfolio.CashBalance:N2}",
                    "Cannot Complete Purchase",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Warning
                );
                return;
            }
            
            _portfolio.CashBalance -= total;
            
            var position = _portfolio.Positions.FirstOrDefault(p => p.Symbol == symbol);
            if (position != null)
            {
                decimal totalShares = position.Shares + shares;
                decimal totalCost = (position.Shares * position.AverageCost) + (shares * price);
                position.AverageCost = totalCost / totalShares;
                position.Shares = (int)totalShares;
            }
            else
            {
                _portfolio.Positions.Add(new Position
                {
                    Symbol = symbol,
                    Shares = shares,
                    AverageCost = price,
                    CurrentPrice = price
                });
            }
            
            _portfolio.TransactionHistory.Add(new Transaction
            {
                DateTime = DateTime.Now,
                Type = "BUY",
                Symbol = symbol,
                Shares = shares,
                Price = price
            });
            
            System.Windows.MessageBox.Show(
                $"Successfully purchased {shares} shares of {symbol} at ${price:N2}\nTotal: ${total:N2}",
                "Trade Executed",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Information
            );
        }

        private void ExecuteSell(string symbol, int shares, decimal price)
        {
            var position = _portfolio.Positions.FirstOrDefault(p => p.Symbol == symbol);
            
            if (position == null || position.Shares < shares)
            {
                System.Windows.MessageBox.Show(
                    $"You don't own enough shares of {symbol}!\nYou own: {position?.Shares ?? 0}, trying to sell: {shares}",
                    "Cannot Complete Sale",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Warning
                );
                return;
            }
            
            decimal total = shares * price;
            
            _portfolio.CashBalance += total;
            
            position.Shares -= shares;
            
            if (position.Shares == 0)
            {
                _portfolio.Positions.Remove(position);
            }
            
            _portfolio.TransactionHistory.Add(new Transaction
            {
                DateTime = DateTime.Now,
                Type = "SELL",
                Symbol = symbol,
                Shares = shares,
                Price = price
            });
            
            System.Windows.MessageBox.Show(
                $"Successfully sold {shares} shares of {symbol} at ${price:N2}\nTotal: ${total:N2}",
                "Trade Executed",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Information
            );
        }

        private void SaveAllData()
        {
            _dataService.SavePortfolio(_portfolio);
            SavePrices();
            _dataService.SaveTransactions(_portfolio.TransactionHistory);
        }

        private void SavePrices()
        {
            var prices = OverviewVM.Stocks.ToDictionary(s => s.Symbol, s => s.Price);
            _dataService.SavePrices(prices);
        }

        private void ResetPortfolio()
        {
            var result = System.Windows.MessageBox.Show(
                "This will reset your portfolio to $100,000 and delete all positions and history. Continue?",
                "Reset Portfolio",
                System.Windows.MessageBoxButton.YesNo,
                System.Windows.MessageBoxImage.Warning
            );
            
            if (result == System.Windows.MessageBoxResult.Yes)
            {
                _dataService.DeleteAllData();
                _portfolio = new Portfolio();
                DashboardVM.RefreshData(OverviewVM.Stocks);
                SaveAllData();
                
                System.Windows.MessageBox.Show(
                    "Portfolio has been reset to $100,000",
                    "Reset Complete",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Information
                );
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
