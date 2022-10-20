using System.ComponentModel.DataAnnotations;

namespace checkout.com.api.Data
{
    public class CreditCard
    {
        [Required]
        public string? CardHolder { get; set; } = string.Empty;
        [Required, MaxLength(19)]
        public string? CardNumber { get; set; } = string.Empty;
        [Required, MaxLength(2)]
        public string? Month { get; set; } = string.Empty;
        [Required, MaxLength(2)]
        public string? Year { get; set; } = String.Empty;
        [Required, MaxLength(3)]
        public string? CVV { get; set; } = String.Empty;
    }
}