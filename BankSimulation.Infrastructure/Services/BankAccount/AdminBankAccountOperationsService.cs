using BankSimulation.Application.Exceptions.BankAccount;
using BankSimulation.Application.Exceptions.User;
using BankSimulation.Application.Interfaces.Repositories;
using BankSimulation.Application.Interfaces.Services;
using BankSimulation.Domain.Entities;
using BankSimulation.Domain.Enums;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BankSimulation.Infrastructure.Services
{
    public class AdminBankAccountOperationsService : IAdminBankAccountOperationsService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly IUserRepository _userRepository;
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly IBankAccountOperationsRepository _bankAccountOperationsRepository;
        private readonly string _exchangeCurrenciesApiKey;
        private const string _exchangeCurrenciesApiUrl = "https://v6.exchangerate-api.com/v6/";

        public AdminBankAccountOperationsService(
            HttpClient httpClient,
            IConfiguration configuration, 
            IUserRepository userRepository, 
            IBankAccountRepository bankAccountRepository, 
            IBankAccountOperationsRepository bankAccountOperationsRepository)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _bankAccountRepository = bankAccountRepository ?? throw new ArgumentNullException(nameof(bankAccountRepository));
            _bankAccountOperationsRepository = bankAccountOperationsRepository 
                ?? throw new ArgumentNullException(nameof(bankAccountOperationsRepository));
            _exchangeCurrenciesApiKey = _configuration["MySettings:ExchangeCurrencyApiKey"] 
                ?? throw new ArgumentNullException("The ExchangeCurrencyApiKey cannot be null.");
        }

        public async Task DepositUserMoneyAsync(Guid userId, string bankAccountNumber, decimal amount, Currency currency)
        {
            if (amount <= 0)
            {
                throw new IncorrectAmountToDepositException($"{userId} - {amount}");
            }

            if (!await _userRepository.AlreadyExistsAsync(userId))
            {
                throw new UserNotFoundException(userId.ToString());
            }

            if (await _bankAccountRepository.AlreadyDeletedAsync(bankAccountNumber))
            {
                throw new UserBankAccountAlreadyDeletedException($"{userId} - {bankAccountNumber}");
            }

            var bankAccountCurrencyInDb = await _bankAccountRepository.GetCurrencyAsync(userId, bankAccountNumber)
                ?? throw new UserBankAccountDoesNotExistException($"{userId} - {bankAccountNumber}");

            if (currency != bankAccountCurrencyInDb)
            {
                amount = await ExchangeCurrencyAsync(amount, currency, bankAccountCurrencyInDb);
            }
            
            await _bankAccountRepository.DepositMoneyAsync(amount, bankAccountNumber);
            var deposit = new Deposit 
            {
                Amount = amount, 
                BankAccountNumber = bankAccountNumber 
            };

            await _bankAccountOperationsRepository.AddDepositAsync(deposit);
            await _userRepository.SaveChangesAsync();    
        }

        private async Task<decimal> ExchangeCurrencyAsync(decimal amount, Currency currentCurrency, Currency requiredCurrency)
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
