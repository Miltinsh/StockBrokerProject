using System;
using System.Collections.Generic;

namespace StockBrokerProject.Services
{
    public class PriceUpdateService
    {
        private readonly Random _random = new();

        /// <summary>
        /// Updates stock price with tiered random volatility system.
        /// Uses 3 nested loops (tiers) where deeper tiers = bigger changes.
        /// 
        /// Tier 1 (Always): 1-10 roll → Small change (0.05% - 0.50%)
        /// Tier 2 (If Tier1 >= 7): 1-10 roll → Medium change (adds 0.2% - 2.0%)
        /// Tier 3 (If Tier2 >= 8): 1-10 roll → Large change (adds 0.5% - 5.0%)
        /// </summary>
        public decimal UpdatePrice(decimal currentPrice)
        {
            // Tier 1: Always happens - determines base volatility (1-10)
            int tier1 = _random.Next(1, 11);
            
            // Tier 2: Only if tier1 >= 7 - amplifies the change (30% chance)
            int tier2 = 0;
            if (tier1 >= 7)
            {
                tier2 = _random.Next(1, 11);
            }
            
            // Tier 3: Only if tier2 >= 8 - rare, massive changes (20% of tier 2)
            int tier3 = 0;
            if (tier2 >= 8)
            {
                tier3 = _random.Next(1, 11);
            }

            // Calculate total volatility multiplier
            decimal volatilityMultiplier = CalculateVolatilityMultiplier(tier1, tier2, tier3);
            
            // Random direction (up or down)
            bool isPositive = _random.NextDouble() > 0.5;
            
            // Calculate price change
            decimal changePercent = volatilityMultiplier * (isPositive ? 1 : -1);
            decimal newPrice = currentPrice * (1 + changePercent / 100);
            
            // Ensure price doesn't go below $1
            if (newPrice < 1m)
                newPrice = 1m;
            
            return Math.Round(newPrice, 2);
        }

        /// <summary>
        /// Calculates volatility multiplier based on tier values.
        /// Tier 1: 0.05% - 0.50% change (common, small moves)
        /// Tier 2: 0.20% - 2.00% change (uncommon, medium moves)  
        /// Tier 3: 0.50% - 5.00% change (rare, large moves)
        /// </summary>
        private decimal CalculateVolatilityMultiplier(int tier1, int tier2, int tier3)
        {
            decimal multiplier = 0m;

            // Tier 1: Base change (0.05% to 0.50%)
            multiplier += tier1 * 0.05m;

            // Tier 2: Medium amplification (adds 0.2% to 2.0%)
            if (tier2 > 0)
            {
                multiplier += tier2 * 0.2m;
            }

            // Tier 3: Large amplification (adds 0.5% to 5.0%)
            if (tier3 > 0)
            {
                multiplier += tier3 * 0.5m;
            }

            return multiplier;
        }

        /// <summary>
        /// Gets a description of the volatility for debugging/logging
        /// </summary>
        public string GetVolatilityDescription(int tier1, int tier2, int tier3)
        {
            if (tier3 > 0)
                return $"🔥 RARE VOLATILITY (Tier 3: {tier1}-{tier2}-{tier3})";
            else if (tier2 > 0)
                return $"⚡ Medium Volatility (Tier 2: {tier1}-{tier2})";
            else
                return $"📊 Normal Volatility (Tier 1: {tier1})";
        }

        /// <summary>
        /// Batch update multiple stock prices
        /// </summary>
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
