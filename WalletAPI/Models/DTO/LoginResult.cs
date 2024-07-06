using System.Data;
using System.Security.Claims;
using WalletAPI.Models.Entities;

namespace WalletAPI.Models.DTO
{
    public class LoginResult
    {
      public bool IsSuccess { get; set; }
        public string Message {  get; set; }
        public string Token {  get; set; }
        public string UserId { get; set; }
        public List<string> Roles {  get; set; }
    }
}
