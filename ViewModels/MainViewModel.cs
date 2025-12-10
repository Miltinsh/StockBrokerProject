using System;
using System.ComponentModel;
using System.Windows.Input;
using StockBrokerProject.Models;

namespace StockBrokerProject.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private object? _currentViewModel;
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

        public OverviewViewModel OverviewVM { get; }
        public ICommand ShowOverviewCommand { get; }
        public ICommand ShowStockInfoCommand { get; }

        // Properties for active state tracking
        public bool IsDashboardActive => CurrentViewModel is OverviewViewModel;
        public bool IsMarketsActive => CurrentViewModel is StockInfoViewModel;

        public MainViewModel()
        {
            OverviewVM = new OverviewViewModel();
            CurrentViewModel = OverviewVM;

            ShowOverviewCommand = new RelayCommand(_ => CurrentViewModel = OverviewVM);
            ShowStockInfoCommand = new RelayCommand(param =>
            {
                // If a StockInfoViewModel is passed, show it
                if (param is StockInfoViewModel vm)
                {
                    CurrentViewModel = vm;
                }
                // Otherwise show selected stock from overview
                else if (OverviewVM.SelectedStock != null)
                {
                    CurrentViewModel = new StockInfoViewModel(new Stock(
                        OverviewVM.SelectedStock.Symbol,
                        OverviewVM.SelectedStock.Name,
                        OverviewVM.SelectedStock.Price,
                        OverviewVM.SelectedStock.Change,
                        OverviewVM.SelectedStock.ChangePercent,
                        OverviewVM.SelectedStock.Sector,
                        OverviewVM.SelectedStock.MarketCap,
                        OverviewVM.SelectedStock.Volume,
                        OverviewVM.SelectedStock.Details
                    ));
                }
            });
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
