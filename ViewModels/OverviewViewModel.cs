using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using StockBrokerProject.Models;
using StockBrokerProject.Services;

namespace StockBrokerProject.ViewModels
{
    public class OverviewViewModel : INotifyPropertyChanged
    {
        private readonly DataService _dataService;
        
        public ObservableCollection<StockInfoViewModel> Stocks { get; } = new();

        private StockInfoViewModel? _selectedStock;
        public StockInfoViewModel? SelectedStock
        {
            get => _selectedStock;
            set
            {
                if (_selectedStock != value)
                {
                    _selectedStock = value;
                    OnPropertyChanged(nameof(SelectedStock));
                }
            }
        }

        // Reference to MainViewModel for executing trades
        public MainViewModel? MainViewModel { get; set; }

        public OverviewViewModel(DataService dataService)
        {
            _dataService = dataService;
            
            // Load saved prices or create initial stock data
            var savedPrices = _dataService.LoadPrices();
            
            // Sample stock market data
            var sampleStocks = new[]
            {
                new Stock("AAPL", "Apple Inc.", 178.42m, 0m, 0m, "Technology", "2.78T", "52.1M", 
                    "Apple Inc. designs, manufactures, and markets smartphones, personal computers, tablets, wearables, and accessories worldwide."),
                new Stock("MSFT", "Microsoft Corporation", 374.23m, 0m, 0m, "Technology", "2.81T", "23.5M",
                    "Microsoft Corporation develops, licenses, and supports software, services, devices, and solutions worldwide."),
                new Stock("GOOGL", "Alphabet Inc.", 139.67m, 0m, 0m, "Technology", "1.75T", "28.7M",
                    "Alphabet Inc. provides various products and services in the United States, Europe, the Middle East, Africa, the Asia-Pacific, Canada, and Latin America."),
                new Stock("AMZN", "Amazon.com, Inc.", 145.82m, 0m, 0m, "Consumer Cyclical", "1.51T", "45.8M",
                    "Amazon.com, Inc. engages in the retail sale of consumer products and subscriptions in North America and internationally."),
                new Stock("NVDA", "NVIDIA Corporation", 485.12m, 0m, 0m, "Technology", "1.19T", "45.2M",
                    "NVIDIA Corporation provides graphics, and compute and networking solutions in the United States, Taiwan, China, and internationally."),
                new Stock("TSLA", "Tesla, Inc.", 242.84m, 0m, 0m, "Consumer Cyclical", "771.2B", "98.7M",
                    "Tesla, Inc. designs, develops, manufactures, leases, and sells electric vehicles, and energy generation and storage systems."),
                new Stock("META", "Meta Platforms, Inc.", 338.56m, 0m, 0m, "Technology", "861.5B", "18.5M",
                    "Meta Platforms, Inc. engages in the development of products that enable people to connect and share with friends and family."),
                new Stock("BRK.B", "Berkshire Hathaway Inc.", 367.45m, 0m, 0m, "Financial", "794.3B", "3.2M",
                    "Berkshire Hathaway Inc., through its subsidiaries, engages in the insurance, freight rail transportation, and utility businesses worldwide."),
                new Stock("V", "Visa Inc.", 258.32m, 0m, 0m, "Financial", "531.2B", "6.8M",
                    "Visa Inc. operates as a payments technology company worldwide."),
                new Stock("JPM", "JPMorgan Chase & Co.", 156.78m, 0m, 0m, "Financial", "455.8B", "12.4M",
                    "JPMorgan Chase & Co. operates as a financial services company worldwide."),
                new Stock("JNJ", "Johnson & Johnson", 162.45m, 0m, 0m, "Healthcare", "402.1B", "8.9M",
                    "Johnson & Johnson researches and develops, manufactures, and sells various products in the healthcare field worldwide."),
                new Stock("WMT", "Walmart Inc.", 158.92m, 0m, 0m, "Consumer Defensive", "433.2B", "11.2M",
                    "Walmart Inc. engages in the operation of retail, wholesale, and other units worldwide.")
            };

            foreach (var stock in sampleStocks)
            {
                Stock stockToAdd = stock;
                // Use saved price if available, otherwise use default
                if (savedPrices.ContainsKey(stock.Symbol))
                {
                    stockToAdd = new Stock(
                        stock.Symbol,
                        stock.Name,
                        savedPrices[stock.Symbol],
                        0m,
                        0m,
                        stock.Sector,
                        stock.MarketCap,
                        stock.Volume,
                        stock.Details
                    );
                }
                
                Stocks.Add(new StockInfoViewModel(stockToAdd));
            }

            SelectedStock = Stocks.FirstOrDefault();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
