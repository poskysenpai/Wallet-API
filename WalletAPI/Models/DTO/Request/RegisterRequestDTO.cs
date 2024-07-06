using Microsoft.SqlServer.Server;
using System.ComponentModel.DataAnnotations;
using WalletAPI.Models.Response;

namespace WalletAPI.Models.DTO.Request
{
    public class RegisterRequestDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [RegularExpression(@"^(\+234)?\d{10}$")]
        public string PhoneNumber { get; set; }
        [Required]
        public string  UserType { get; set; }

        [Required]
        public List<CurrencyAttribute> Currencies { get; set; }

        //public static implicit operator RegisterRequestDTO(RegisterResponse v)
        //{
        //    throw new NotImplementedException();
        //}


    }

    public class CurrencyAttribute
    {
        public bool IsMain { get; set; }

        public Currency Type { get; set; }
    }

    }
