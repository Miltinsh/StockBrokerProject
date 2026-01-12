using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using StockBrokerProject.Models;

namespace StockBrokerProject.Services
{
    public class DataService
    {
        private const string PortfolioFile = "portfolio.json";
        private const string PricesFile = "prices.json";
        private const string TransactionsFile = "transactions.json";

        private readonly JsonSerializerOptions _options = new()
        {
            WriteIndented = true
        };


        public Portfolio LoadPortfolio()
        {
            try
            {
                if (File.Exists(PortfolioFile))
                {
                    var json = File.ReadAllText(PortfolioFile);
                    var portfolio = JsonSerializer.Deserialize<Portfolio>(json);
                    return portfolio ?? CreateDefaultPortfolio();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading portfolio: {ex.Message}");
            }

            return CreateDefaultPortfolio();
        }

        public void SavePortfolio(Portfolio portfolio)
        {
            try
            {
                var json = JsonSerializer.Serialize(portfolio, _options);
                File.WriteAllText(PortfolioFile, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving portfolio: {ex.Message}");
            }
        }

        private Portfolio CreateDefaultPortfolio()
        {
            return new Portfolio
            {
                CashBalance = 100000m,
                TotalValue = 100000m,
                TotalGainLoss = 0m,
                Positions = new List<Position>(),
                TransactionHistory = new List<Transaction>()
            };
        }



         

        public Dictionary<string, decimal> LoadPrices()
        {
            try
            {
                if (File.Exists(PricesFile))
                {
                    var json = File.ReadAllText(PricesFile);
                    var prices = JsonSerializer.Deserialize<Dictionary<string, decimal>>(json);
                    return prices ?? new Dictionary<string, decimal>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading prices: {ex.Message}");
            }

            return new Dictionary<string, decimal>();
        }

        public void SavePrices(Dictionary<string, decimal> prices)
        {
            try
            {
                var json = JsonSerializer.Serialize(prices, _options);
                File.WriteAllText(PricesFile, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving prices: {ex.Message}");
            }
        }

     

        public List<Transaction> LoadTransactions()
        {
            try
            {
                if (File.Exists(TransactionsFile))
                {
                    var json = File.ReadAllText(TransactionsFile);
                    var transactions = JsonSerializer.Deserialize<List<Transaction>>(json);
                    return transactions ?? new List<Transaction>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading transactions: {ex.Message}");
            }

            return new List<Transaction>();
        }

        public void SaveTransactions(List<Transaction> transactions)
        {
            try
            {
                var json = JsonSerializer.Serialize(transactions, _options);
                File.WriteAllText(TransactionsFile, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving transactions: {ex.Message}");
            }
        }



        public void DeleteAllData()
        {
            try
            {
                if (File.Exists(PortfolioFile)) File.Delete(PortfolioFile);
                if (File.Exists(PricesFile)) File.Delete(PricesFile);
                if (File.Exists(TransactionsFile)) File.Delete(TransactionsFile);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting data: {ex.Message}");
            }
        }

        public bool HasSavedData()
        {
            return File.Exists(PortfolioFile) || File.Exists(PricesFile);
        }

 
    }
}
