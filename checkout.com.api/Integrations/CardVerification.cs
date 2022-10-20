using checkout.com.api.Data;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;

namespace checkout.com.api.Integrations
{
    public class CardVerification : ICardVerification
    {
        private readonly ILogger<CardVerification> _logger;
        private readonly IOptions<EndPointConfig> _options;

        public CardVerification(ILogger<CardVerification> logger, IOptions<EndPointConfig> options)
        {
            _logger = logger;
            _options = options;
        }

        public async Task<bool> CheckCard(Purchase purchase)
        {
            try
            {
                var content = JsonConvert.SerializeObject(purchase);
                var webRequest = new WebExtension<bool>();
                var result = await webRequest.MakeRequest(_options.Value.CardVerificationEndPoint, _options.Value.LuhnApi, Method.Post, content);
                return result;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message, ex.InnerException);
                return false;
            }
        }
    }
}
