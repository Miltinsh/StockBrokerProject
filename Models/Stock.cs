using System;

namespace StockBrokerProject.Models
{
    public sealed class Stock
    {
        public string Symbol { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public decimal Price { get; init; }
        public decimal Change { get; init; }
        public decimal ChangePercent { get; init; }
        public string Sector { get; init; } = string.Empty;
        public string MarketCap { get; init; } = string.Empty;
        public string Volume { get; init; } = string.Empty;
        public string Details { get; init; } = string.Empty;

        public Stock() { }

        public Stock(string symbol, string name, decimal price, decimal change, decimal changePercent, string sector = "", string marketCap = "", string volume = "", string details = "")
        {
            Symbol = symbol ?? string.Empty;
            Name = name ?? string.Empty;
            Price = price;
            Change = change;
            ChangePercent = changePercent;
            Sector = sector ?? string.Empty;
            MarketCap = marketCap ?? string.Empty;
            Volume = volume ?? string.Empty;
            Details = details ?? string.Empty;
        }
    }
}
