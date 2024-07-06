using System.ComponentModel.DataAnnotations;

namespace WalletAPI.Models.DTO.Request
{
    public class ConversionRequestDTO
    {
       
            public string UserId { get; set; }

            [Required]
            public Currency SourceCurrency { get; set; }
            public Currency TargetCurrency { get; set; }
            public decimal Amount { get; set; }
        }
    
}
