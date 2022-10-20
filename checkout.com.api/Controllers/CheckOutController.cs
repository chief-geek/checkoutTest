using checkout.com.api.Data;
using checkout.com.api.Database;
using checkout.com.api.Integrations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net;

namespace checkout.com.api.Controllers
{
    [ApiController]
    [Route("api/checkout")]
    public class CheckOutController : ControllerBase
    {

        private readonly ILogger<CheckOutController> _logger;
        private readonly IOptions<EndPointConfig> _config;
        private readonly ICardVerification _cardVerification;
        private readonly IRepository _repository;
        private readonly IBankRequest _bankRequest;

        public CheckOutController(ILogger<CheckOutController> logger, IOptions<EndPointConfig> config, 
            ICardVerification cardVerification, IRepository repository, IBankRequest bankRequest)
        {
            _logger = logger;
            _config = config;
            _cardVerification = cardVerification;
            _repository = repository;
            _bankRequest = bankRequest;

        }

        [Route("acceptpayment")]
        [HttpPost]
        public async Task<IActionResult> AcceptCardPayment(Purchase purchase)
        {
            if (purchase == null || purchase.CreditCard == null)
            {
                _logger.LogError($"Invalid or missing card or purchase information");
                return BadRequest(JsonConvert.SerializeObject(BuildResult(purchase, "Invalid or missing card or purchase information", HttpStatusCode.BadRequest, null)));
            }

            var validCard = await CardIsValid(purchase);
            if (!validCard)
            {
                return BadRequest(JsonConvert.SerializeObject(BuildResult(purchase, "Invalid Card Number provided or the bank refused the transaction", HttpStatusCode.BadRequest, null)));
            }

            var bankAccepted = await BankAcceptedPayment(purchase);
            if(!bankAccepted)
            {
                return BadRequest(JsonConvert.SerializeObject(BuildResult(purchase, "Bank Refused Transaction", HttpStatusCode.BadRequest, null)));
            }

            var dbResult = await SaveToDatabase(purchase);
            if (dbResult == string.Empty)
            {
                return BadRequest(JsonConvert.SerializeObject(BuildResult(purchase,"Failed to save information to database, please check logs.", HttpStatusCode.BadRequest, null)));
            }

            var result = new CheckoutResult() 
            { 
                StatusCode = HttpStatusCode.OK, 
                Message = null ,
                Card = purchase.CreditCard,
                TransactionId = dbResult
            };
            return Ok(JsonConvert.SerializeObject(result));
        }

        

        [HttpPost]
        [Route("gettransaction")]
        public async Task<IActionResult> GetTransaction(string transactionId)
        {
            var result = await _repository.GetTransaction(transactionId);
            if (result == null)
            {
                return NotFound(JsonConvert.SerializeObject(result));
            }

            return Ok(JsonConvert.SerializeObject(result));
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

        private CheckoutResult BuildResult(Purchase purchase, string message, HttpStatusCode statusCode, string? transactionID)
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