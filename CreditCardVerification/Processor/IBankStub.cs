using PaymentGateway.Data;
using System.Threading.Tasks;

namespace PaymentGateway.Processor
{
    public interface IBankStub
    {
        Task<bool> TransactionIsValid(Purchase purchase);
    }
}
