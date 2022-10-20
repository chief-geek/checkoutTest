using System.Net;

namespace checkout.com.api.Data
{
    public class CheckoutResult
    {
        public HttpStatusCode StatusCode { get; set; }
        public string? Message { get; set; }
        public CreditCard? Card { get; set; }
        public string? TransactionId { get; set; }
    }
}