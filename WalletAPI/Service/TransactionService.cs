using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WalletAPI.Data;
using WalletAPI.Helper;
using WalletAPI.Migrations;
using WalletAPI.Models.DTO;
using WalletAPI.Models.Entities;
using WalletAPI.Service.Interfaces;

namespace WalletAPI.Service;

public class TransactionService : ITransactionService
{
    private readonly AppDbContext _context;
    private readonly IConversionService _conversionService;
    private readonly UserManager<Users> _userManager;
    private readonly IRepository<Wallets> _repository;
    public TransactionService(AppDbContext context, IConversionService conversionService, UserManager<Users> userManager, IRepository<Wallets> repository)
    {
        _context = context;
        _conversionService = conversionService;
        _userManager = userManager;
        _repository = repository;
    }
    public async Task<APIresponse<ConversionResponseDTO>> FundWallet(string UserId, Currency SourceCurrency, double Amount, HttpContext httpContext)
    {
        var User = String.IsNullOrEmpty(UserId)
        ? await _userManager.GetUserAsync(httpContext.User)
        : await _userManager.FindByIdAsync(UserId);

        if (User == null)
        {
            throw new Exception("User does not exist");
        }

        if (SourceCurrency == null || Amount <= 0)
        {
            throw new Exception("Invalid Request");
        }

        var Noob = await _userManager.IsInRoleAsync(User, "noob");

        if (Noob)
        {


            var noobwallet = await _repository.FindByCondition(x => (x.UsersId == User.Id)).FirstOrDefaultAsync();
            if (noobwallet == null)
            {
                throw new Exception("could not get users wallet");
            }

            if (noobwallet.Currency == SourceCurrency)
            {
                noobwallet.Balance = noobwallet.Balance + Amount;

                _context.SaveChanges();
            }
            else
            {
                double convertedAmount = await _conversionService.ConvertCurrencyAsync(Amount, SourceCurrency, noobwallet.Currency);

                noobwallet.Balance = noobwallet.Balance + convertedAmount;
                _context.SaveChanges();

            }

        }
        else
        {
            var wallet = _repository.FindByCondition(x => (x.UsersId == User.Id) && (x.Currency == SourceCurrency)).FirstOrDefault();
            if (wallet == null)
            {
                var walletId = Guid.NewGuid().ToString();
                Wallets wallets = new Wallets
                {
                    Id = walletId,
                    Currency = SourceCurrency,
                    Balance = Amount,
                    IsMain = false,
                    UsersId = UserId,

                };
                _context.Wallets.Add(wallets);
                _context.SaveChanges();
            }

            else
            {
                wallet.Balance += Amount;
                _context.SaveChanges();

            }
        }
        return new APIresponse<ConversionResponseDTO>() { Message = "Funded Successfully", StatusCode = 200 };
    }

    public async Task<APIresponse<ConversionResponseDTO>> WithdrawWallet(string UserId, Currency SourceCurrency, double Amount, HttpContext httpContext)
    {
        var user = await _userManager.GetUserAsync(httpContext.User);
        var wallet = _repository.FindByCondition(x => (x.UsersId == user.Id) && (x.Currency == SourceCurrency)).FirstOrDefault();

        if (wallet == null || wallet.Balance < Amount)
        {
            double remainingAmount = 0;

            if (wallet != null)
            {
                // Check if the amount requested less than main wallet + currency wallet
                //decimal totalBalance = _cacheService.Get<decimal>(user.Email);
                remainingAmount = Amount - wallet.Balance;
                wallet.Balance = 0;
            }

            
                var wallets = _repository.FindByCondition(x => (x.UsersId == user.Id)).ToList();
                // Go through each wallet and subtract the currency equivalent
                foreach (var currWallet in wallets)
                {
                    //if (remainingAmount == 0) { break; }
                    // 5000 = $5
                    var equivalentAmount = await _conversionService.ConvertCurrencyAsync(Amount, SourceCurrency, currWallet.Currency);
                    // 5000 >= 4000
                    if (equivalentAmount >= currWallet.Balance) //remainingAmount)
                    {
                        // $5 - conv(4000, $)
                        //currWallet.Balance -= await _conversionService.ConvertCurrencyAsync(remainingAmount, SourceCurrency, currWallet.Currency);
                        
                       
                        currWallet.Balance = 0;
                        remainingAmount = equivalentAmount - currWallet.Balance ;
                    _context.SaveChanges();
                }
                    else
                    {
                        
                        currWallet.Balance -= equivalentAmount ;
                    remainingAmount = 0;
                    _context.SaveChanges();
                    break;
                    }
                }
                // if the money is not enough return insufficient funds.
                if (remainingAmount > 0)
                {
                    throw new Exception("Insufficient Funds");
                }
                
            
        }
        else // Sufficient balance in the selected wallet
        {
            wallet.Balance -= Amount;
            _context.SaveChanges();
        }
        
        return new APIresponse<ConversionResponseDTO>() { Message = "Withdrawn Successfully", StatusCode = 200 };
    }


}
