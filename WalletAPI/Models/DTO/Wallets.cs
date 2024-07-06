using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WalletAPI.Models.Entities;

namespace WalletAPI.Models.DTO
{
    public class Wallets
    {
        [Key]
        public string Id { get; set; }

        public double Balance { get; set; }

        public bool IsMain { get; set; }    
        public Currency Currency { get; set; }

       

        public string UsersId { get; set; }

        
        
    }
}
