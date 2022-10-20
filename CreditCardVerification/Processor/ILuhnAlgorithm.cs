using System.Threading.Tasks;

namespace PaymentGateway.Processor
{
    public interface ILuhnAlgorithm
    {
        Task<bool> Check(string? cardNumber);
    }
}
