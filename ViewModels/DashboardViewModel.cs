using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using StockBrokerProject.Models;
using StockBrokerProject.Services;

namespace StockBrokerProject.ViewModels
{
    public class DashboardViewModel : INotifyPropertyChanged
    {
        private readonly Portfolio _portfolio;
        private readonly DataService _dataService;

        public decimal TotalValue => _portfolio.TotalValue;
        public decimal TotalGainLoss => _portfolio.TotalGainLoss;
        public decimal TotalGainLossPercent => _portfolio.TotalValue > 0 
            ? (_portfolio.TotalGainLoss / (_portfolio.TotalValue - _portfolio.TotalGainLoss)) * 100 
            : 0;
        public decimal CashBalance => _portfolio.CashBalance;
        public decimal InvestedAmount => _portfolio.TotalValue - _portfolio.CashBalance;

        public ObservableCollection<TopMoverItem> TopMovers { get; } = new();

        public ObservableCollection<NewsFeedItem> NewsFeeds { get; } = new();

        public DashboardViewModel(Portfolio portfolio, DataService dataService)
        {
            _portfolio = portfolio;
            _dataService = dataService;
            
            _portfolio.PropertyChanged += (s, e) => RefreshStats();
            
            LoadNewsFeed();
        }

        public void RefreshData(ObservableCollection<StockInfoViewModel> stocks)
        {
            RefreshStats();
            RefreshTopMovers(stocks);
        }

        private void RefreshStats()
        {
            OnPropertyChanged(nameof(TotalValue));
            OnPropertyChanged(nameof(TotalGainLoss));
            OnPropertyChanged(nameof(TotalGainLossPercent));
            OnPropertyChanged(nameof(CashBalance));
            OnPropertyChanged(nameof(InvestedAmount));
        }

        private void RefreshTopMovers(ObservableCollection<StockInfoViewModel> stocks)
        {
            var topMovers = stocks
                .OrderByDescending(s => Math.Abs(s.ChangePercent))
                .Take(5)
                .ToList();

            TopMovers.Clear();
            foreach (var stock in topMovers)
            {
                TopMovers.Add(new TopMoverItem(
                    stock.Symbol,
                    stock.Name,
                    stock.Price,
                    stock.Change,
                    stock.ChangePercent,
                    stock.Volume
                ));
            }
        }

        private void LoadNewsFeed()
        {
            NewsFeeds.Add(new NewsFeedItem(
                "Michael Chen",
                "$NVDA",
                "15 min ago",
                "Just increased my position in NVDA. AI chip demand is through the roof. Long-term play here.",
                24
            ));

            NewsFeeds.Add(new NewsFeedItem(
                "$AAPL",
                "$AAPL",
                "1 hour ago",
                "Apple announces Q4 earnings beat expectations. Revenue up 8% YoY with strong iPhone sales in emerging markets.",
                156
            ));

            NewsFeeds.Add(new NewsFeedItem(
                "Sarah Williams",
                "$TSLA",
                "2 hours ago",
                "Selling half my TSLA position. Taking profits after this rally. Will reassess at lower levels.",
                18
            ));

            NewsFeeds.Add(new NewsFeedItem(
                "$META",
                "$META",
                "3 hours ago",
                "Meta Platforms announces new AI features for Instagram and WhatsApp. Stock up 4% in after-hours trading.",
                203
            ));

            NewsFeeds.Add(new NewsFeedItem(
                "David Park",
                "$AMD",
                "4 hours ago",
                "AMD's data center business is seriously undervalued. Market share gains from Intel continue.",
                42
            ));

            NewsFeeds.Add(new NewsFeedItem(
                "$TSLA",
                "$TSLA",
                "5 hours ago",
                "Tesla reports record deliveries in Q4. Production ramp continues across all factories.",
                187
            ));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public class TopMoverItem : INotifyPropertyChanged
    {
        public string Symbol { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal Change { get; set; }
        public decimal ChangePercent { get; set; }
        public string Volume { get; set; } = string.Empty;

        public TopMoverItem() { }

        public TopMoverItem(string symbol, string name, decimal price, decimal change, decimal changePercent, string volume)
        {
            Symbol = symbol;
            Name = name;
            Price = price;
            Change = change;
            ChangePercent = changePercent;
            Volume = volume;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public class NewsFeedItem : INotifyPropertyChanged
    {
        public string Author { get; set; } = string.Empty;
        public string Ticker { get; set; } = string.Empty;
        public string TimeAgo { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int Reactions { get; set; }

        public NewsFeedItem() { }

        public NewsFeedItem(string author, string ticker, string timeAgo, string content, int reactions)
        {
            Author = author;
            Ticker = ticker;
            TimeAgo = timeAgo;
            Content = content;
            Reactions = reactions;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
