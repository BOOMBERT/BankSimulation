using BankSimulation.Domain.Enums;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace BankSimulation.Infrastructure.Services.Utils
{
    internal class MoneyOperations
    {
        private readonly HttpClient _httpClient;
        private readonly string _exchangeCurrenciesApiKey;
        private readonly string _exchangeCurrenciesApiUrl;

        public MoneyOperations(IConfiguration configuration, HttpClient httpClient)
        {
            _httpClient = httpClient;
            _exchangeCurrenciesApiKey = configuration["ExchangeCurrenciesSettings:ApiKey"] 
                ?? throw new ArgumentNullException("The ExchangeCurrenciesApiKey cannot be null.");
            _exchangeCurrenciesApiUrl = configuration["ExchangeCurrenciesSettings:ApiUrl"] 
                ?? throw new ArgumentNullException("The ExchangeCurrenciesApiUrl cannot be null.");
        }

        public async Task<decimal> ExchangeCurrencyAsync(decimal amount, Currency currentCurrency, Currency requiredCurrency)
        {
            try
            {
                var response = await _httpClient.GetStringAsync(
                    $"{_exchangeCurrenciesApiUrl}{_exchangeCurrenciesApiKey}/latest/{currentCurrency}");

                var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(response);

                if (data == null || !data.ContainsKey("conversion_rates"))
                {
                    throw new KeyNotFoundException("The 'conversion_rates' section not found.");
                }

                var conversionRatesJObject = data["conversion_rates"] as JObject;
                var conversionRates = conversionRatesJObject!.ToObject<Dictionary<string, decimal>>();

                if (conversionRates == null || !conversionRates.TryGetValue(requiredCurrency.ToString(), out var exchangeRate))
                {
                    throw new KeyNotFoundException($"Currency '{requiredCurrency}' not found.");
                }

                return amount * exchangeRate;
            }
            catch (HttpRequestException)
            {
                throw new HttpRequestException("Error accessing the currency exchange API.");
            }
        }
    }
}
