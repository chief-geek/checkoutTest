using checkout.com.api.Data;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace checkout.com.api.Integrations
{
    public class BankRequest : IBankRequest
    {
        private readonly ILogger<BankRequest> _logger;
        private readonly IOptions<EndPointConfig> _options;

        public BankRequest(ILogger<BankRequest> logger, IOptions<EndPointConfig> options)
        {
            _logger = logger;
            _options = options;
        }

        public async Task<bool> BankAccepts(Purchase purchase)
        {
            var webRequest = new WebExtension<TransactionResult>();
            var content = JsonConvert.SerializeObject(purchase);
            return await webRequest.MakeRequest(_options.Value.CardVerificationEndPoint, _options.Value.BankApi, RestSharp.Method.Post, content);
        }
    }
}
