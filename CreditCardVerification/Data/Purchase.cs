using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.Data
{
    public class Purchase
    {
        [Required]
        public CreditCard? CreditCard { get; set; }

        [Required]
        public decimal Amount { get; set; }
    }
}
