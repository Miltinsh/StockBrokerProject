using System;
using System.ComponentModel;
using StockBrokerProject.Models;

namespace StockBrokerProject.ViewModels
{
    public class StockInfoViewModel : INotifyPropertyChanged
    {
        private string _symbol = string.Empty;
        private string _name = string.Empty;
        private decimal _price;
        private decimal _change;
        private decimal _changePercent;
        private string _sector = string.Empty;
        private string _marketCap = string.Empty;
        private string _volume = string.Empty;
        private string _details = string.Empty;

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
            set { if (value != _price) { _price = value; OnPropertyChanged(nameof(Price)); } }
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

        public StockInfoViewModel() { }

        public StockInfoViewModel(Stock stock)
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

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
