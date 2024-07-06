using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using WalletAPI.Service;
using WalletAPI.Service.Interfaces;
using System.Net.Http;
using System.Threading.Tasks;


using WalletAPI.Models.DTO.Request;
using WalletAPI.Models.DTO;
using Microsoft.EntityFrameworkCore;
using WalletAPI.Data;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WalletAPI.Controllers
{
    public class TransactionController : Controller
    {
        private readonly IAuthService _authService;
        private readonly AppDbContext _context;
        private readonly ITransactionService _transactionService;
        //private const string FixerApiKey = "fcc88ce9aa618718823880801b3d7dc4";
        private readonly HttpClient _httpClient;
        private readonly IConversionService _conversionService;
        public TransactionController( IAuthService authService, IHttpClientFactory httpClientFactory,AppDbContext context, IConversionService conversionService, ITransactionService transactionService)
        {
            _httpClient = httpClientFactory.CreateClient();
            _transactionService = transactionService;
            _authService = authService;
            _context = context;
            _conversionService = conversionService;

        }
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "noob,elite")]
        [HttpPost("FundWallet")]
            public async Task<IActionResult> deposit(string UserId, Currency SourceCurrency,  double Amount)
            {

            if (!ModelState.IsValid) // checks if the view model properties are valid
            {
                return BadRequest();

            }
            var response = await _transactionService.FundWallet( UserId,  SourceCurrency, Amount, HttpContext);
            if (response.StatusCode != 200)
            {
                return BadRequest(response);
            }

            return Ok(response);


            // Process the response
            // Parse JSON response and extract converted amount
            // Example: decimal convertedAmount = ParseAndExtractAmount(responseBody);

            // You can further process the converted amount as needed

            return Ok("Wallet funded successfully.");
            }

        static decimal ParseAndExtractAmount(string responseBody)
        { 
            return 1;
        }
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "noob,elite")]
        [HttpPost("Withdrawal")]
        public async Task<IActionResult> Withdraw(string UserId, Currency SourceCurrency, double Amount)
        {
                var apiResponse = await _transactionService.WithdrawWallet(UserId,SourceCurrency, Amount,  HttpContext);
                return Ok(apiResponse);
            
        }


    }

       
    }

//For fixer Api
//string apiUrl = $"http://data.fixer.io/api/convert?access_key={FixerApiKey}&from={SourceCurrency}&to={Wallet.Currency}&amount={Amount}";
//HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
//response.EnsureSuccessStatusCode();
//string responseBody = await response.Content.ReadAsStringAsync();


