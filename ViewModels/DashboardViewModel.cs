using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace StockBrokerProject.ViewModels
{
    public class DashboardViewModel : INotifyPropertyChanged
    {
        // Portfolio Stats
        private decimal _totalValue = 48750.32m;
        private decimal _totalGainLoss = 6250.78m;
        private decimal _totalGainLossPercent = 14.72m;
        private decimal _cashBalance = 12500.00m;
        private decimal _investedAmount = 36250.32m;

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

        public decimal TotalGainLossPercent
        {
            get => _totalGainLossPercent;
            set { if (value != _totalGainLossPercent) { _totalGainLossPercent = value; OnPropertyChanged(nameof(TotalGainLossPercent)); } }
        }

        public decimal CashBalance
        {
            get => _cashBalance;
            set { if (value != _cashBalance) { _cashBalance = value; OnPropertyChanged(nameof(CashBalance)); } }
        }

        public decimal InvestedAmount
        {
            get => _investedAmount;
            set { if (value != _investedAmount) { _investedAmount = value; OnPropertyChanged(nameof(InvestedAmount)); } }
        }

        // Top Movers - using StockInfoViewModel from OverviewViewModel
        public ObservableCollection<TopMoverItem> TopMovers { get; } = new();

        // News Feed
        public ObservableCollection<NewsFeedItem> NewsFeeds { get; } = new();

        public DashboardViewModel()
        {
            LoadTopMovers();
            LoadNewsFeed();
        }

        private void LoadTopMovers()
        {
            // Top 5 movers based on the sample data
            TopMovers.Add(new TopMoverItem("NVDA", "NVIDIA Corporation", 485.12m, 28.45m, 6.23m, "45.2M"));
            TopMovers.Add(new TopMoverItem("TSLA", "Tesla, Inc.", 242.84m, -15.32m, -5.93m, "98.7M"));
            TopMovers.Add(new TopMoverItem("META", "Meta Platforms, Inc.", 338.56m, 12.34m, 3.78m, "18.5M"));
            TopMovers.Add(new TopMoverItem("AAPL", "Apple Inc.", 178.42m, 8.12m, 4.77m, "52.1M"));
            TopMovers.Add(new TopMoverItem("AMD", "Advanced Micro Devices", 118.92m, -6.78m, -5.40m, "63.2M"));
        }

        private void LoadNewsFeed()
        {
            // Sample news items
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

    // Helper classes for Dashboard items
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
