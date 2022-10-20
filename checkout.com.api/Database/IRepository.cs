using checkout.com.api.Data;

namespace checkout.com.api.Database
{
    public interface IRepository
    {
        Task<string> SavePurchase(Purchase purchase);
        Task<TransactionResult> GetTransaction(string transactionId);
    }
}
