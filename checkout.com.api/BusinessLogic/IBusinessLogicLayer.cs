using checkout.com.api.Data;

namespace checkout.com.api.BusinessLogic
{
    public interface IBusinessLogicLayer
    {
        Task<CheckoutResult> Run(Purchase purchase);
    }
}
