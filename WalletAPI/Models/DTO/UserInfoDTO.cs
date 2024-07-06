namespace WalletAPI.Models.DTO
{
    public class UserInfoDTO
    {
        public string UserId { get; set; }
       

        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
       
        public string PhoneNumber{ get; set; }

        
        public ICollection<Wallets> wallets { get; set; }


    }
}
