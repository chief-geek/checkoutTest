using checkout.com.api.Data;

namespace checkout.com.api.Integrations
{
    public interface ICardVerification
    {
        Task<bool> CheckCard(Purchase purchase);   
    }
}
