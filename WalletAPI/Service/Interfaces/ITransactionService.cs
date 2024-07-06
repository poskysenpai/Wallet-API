using WalletAPI.Models.DTO.Request;
using WalletAPI.Models.DTO;
using WalletAPI.Models.Response;

namespace WalletAPI.Service.Interfaces
{
    public interface ITransactionService
    {
        Task<APIresponse<ConversionResponseDTO>> FundWallet(string WalletId, Currency SourceCurrency, double Amount, HttpContext httpContext);
        Task<APIresponse<ConversionResponseDTO>> WithdrawWallet(string UserId, Currency SourceCurrency, double Amount, HttpContext httpContext);
    }
}
