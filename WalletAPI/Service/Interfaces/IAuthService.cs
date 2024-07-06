using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using System.Security.Claims;
using WalletAPI.Models.DTO;
using WalletAPI.Models.DTO.Request;
using WalletAPI.Models.Entities;
using WalletAPI.Models.Response;

namespace WalletAPI.Service.Interfaces
{
    public interface IAuthService
    {
        //Task<string> Generate(LoginDTO user);

        // Task<APIresponse<string>> LoginAsync(LoginDTO requestDTO);

        //Task<Dictionary<string, string>> ValidateLoggedInUser(ClaimsPrincipal user, string userId);

        string Generate(Users user, List<string> roles, List<Claim> claims);

        Task<LoginResult> Login(LoginDTO model);

        Task<APIresponse<RegisterResponse>> Register(RegisterRequestDTO request);

    }
}
