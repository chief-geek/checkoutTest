using checkout.com.api.Data;

namespace checkout.com.api.Integrations
{
    public interface IBankRequest
    {
        Task<bool> BankAccepts(Purchase purchase);
    }
}
