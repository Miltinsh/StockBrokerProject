using System;
using System.Collections.Generic;

namespace StockBrokerProject.Services
{
    public class PriceUpdateService
    {
        private readonly Random _random = new();

        public decimal UpdatePrice(decimal currentPrice)
        {
            int tier1 = _random.Next(1, 11);
            
            int tier2 = 0;
            if (tier1 >= 7)
            {
                tier2 = _random.Next(1, 11);
            }
            
            int tier3 = 0;
            if (tier2 >= 8)
            {
                tier3 = _random.Next(1, 11);
            }

            decimal volatilityMultiplier = CalculateVolatilityMultiplier(tier1, tier2, tier3);
            
            bool isPositive = _random.NextDouble() > 0.5;
            
            decimal changePercent = volatilityMultiplier * (isPositive ? 1 : -1);
            decimal newPrice = currentPrice * (1 + changePercent / 100);
            
            if (newPrice < 1m)
                newPrice = 1m;
            
            return Math.Round(newPrice, 2);
        }

        private decimal CalculateVolatilityMultiplier(int tier1, int tier2, int tier3)
        {
            decimal multiplier = 0m;

            multiplier += tier1 * 0.05m;

            if (tier2 > 0)
            {
                multiplier += tier2 * 0.2m;
            }

            if (tier3 > 0)
            {
                multiplier += tier3 * 0.5m;
            }

            return multiplier;
        }

        public string GetVolatilityDescription(int tier1, int tier2, int tier3)
        {
            if (tier3 > 0)
                return $"🔥 RARE VOLATILITY (Tier 3: {tier1}-{tier2}-{tier3})";
            else if (tier2 > 0)
                return $"⚡ Medium Volatility (Tier 2: {tier1}-{tier2})";
            else
                return $"📊 Normal Volatility (Tier 1: {tier1})";
        }

        public Dictionary<string, decimal> UpdatePrices(Dictionary<string, decimal> currentPrices)
        {
            var updatedPrices = new Dictionary<string, decimal>();
            
            foreach (var kvp in currentPrices)
            {
                updatedPrices[kvp.Key] = UpdatePrice(kvp.Value);
            }
            
            return updatedPrices;
        }
    }
}
