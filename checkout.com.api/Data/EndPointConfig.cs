namespace checkout.com.api.Data
{
    public interface EndpointConfig
    {
        public string? CardVerificationEndPoint { get; set; }
    }
    public class EndPointConfig
    {
        public string? CardVerificationEndPoint { get; set; }
        public string? LuhnApi { get; set; }
        public string? BankApi { get; set; }
    }
}
