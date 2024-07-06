namespace WalletAPI.Models.DTO
{
    public class APIresponse
    
        {
        public int StatusCode { get; set; }
        public string Message { get; set; }


    }
    public class APIresponse<T> : APIresponse
    {

        public T Data { get; set; }


    }
}

