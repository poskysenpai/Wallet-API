using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using WalletAPI.Models.DTO;

namespace WalletAPI.Models.Entities
{
    public class Users : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
