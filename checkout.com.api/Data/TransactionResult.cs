namespace checkout.com.api.Data
{
    public class TransactionResult
    {
        public string? CardHolder { get; set; } = string.Empty;
        public string? CardNumber { get; set; } = string.Empty;
        public string? ExpiryMonth { get; set; } = string.Empty;
        public string? ExpiryYear { get; set; } = String.Empty;
        public decimal? Amount { get; set; } = null;
        public string? TransactionId { get; set; }
    }
}
