using WalletAPI.Models.DTO;

namespace WalletAPI.Service.Interfaces
{
    public interface IConversionService
    {
        Task<double> ConvertCurrencyAsync(double amount, Currency sourceCurrency, Currency targetCurrency);
    }
}
