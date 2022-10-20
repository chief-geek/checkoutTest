using checkout.com.api.Data;
using checkout.com.api.Database;
using checkout.com.api.Integrations;
using Microsoft.Extensions.Options;
using System.Net;

namespace checkout.com.api.BusinessLogic
{
    public class BusinessLogicLayer : IBusinessLogicLayer
    {
        private readonly ILogger<BusinessLogicLayer> _logger;
        private readonly IOptions<EndPointConfig> _config;
        private readonly ICardVerification _cardVerification;
        private readonly IRepository _repository;
        private readonly IBankRequest _bankRequest;

        public BusinessLogicLayer(ILogger<BusinessLogicLayer> logger, IOptions<EndPointConfig> config,
            ICardVerification cardVerification, IRepository repository, IBankRequest bankRequest)
        {
            _logger = logger;
            _config = config;
            _cardVerification = cardVerification;
            _repository = repository;
            _bankRequest = bankRequest;
        }

        public async Task<CheckoutResult> Run(Purchase purchase)
        {
            if (purchase == null || purchase.CreditCard == null)
            {
                _logger.LogError($"Invalid or missing card or purchase information");
                return await BuildResult(purchase, "Invalid or missing card or purchase information", HttpStatusCode.BadRequest, null);
            }

            var validCard = await CardIsValid(purchase);
            if (!validCard)
            {
                return await BuildResult(purchase, "Invalid Card Number provided or the bank refused the transaction", HttpStatusCode.BadRequest, null);
            }

            var bankAccepted = await BankAcceptedPayment(purchase);
            if (!bankAccepted)
            {
                return await BuildResult(purchase, "Bank Refused Transaction", HttpStatusCode.BadRequest, null);
            }

            var dbResult = await SaveToDatabase(purchase);
            if (dbResult == string.Empty)
            {
                return await BuildResult(purchase, "Failed to save information to database, please check logs.", HttpStatusCode.BadRequest, null);
            }

            var result = new CheckoutResult()
            {
                StatusCode = HttpStatusCode.OK,
                Message = null,
                Card = purchase.CreditCard,
                TransactionId = dbResult
            };
            return result;
        }

        private async Task<string> SaveToDatabase(Purchase purchase)
        {
            return await _repository.SavePurchase(purchase);
        }

        private async Task<bool> BankAcceptedPayment(Purchase purchase)
        {
            return await _bankRequest.BankAccepts(purchase);
        }

        private async Task<bool> CardIsValid(Purchase purchase)
        {
            return await _cardVerification.CheckCard(purchase);
        }

        private async Task<CheckoutResult> BuildResult(Purchase purchase, string message, HttpStatusCode statusCode, string? transactionID)
        {
            var result = new CheckoutResult
            {
                StatusCode = statusCode,
                Message = message,
                Card = purchase.CreditCard,
                TransactionId = transactionID
            };
            return result;
        }
    }
}
