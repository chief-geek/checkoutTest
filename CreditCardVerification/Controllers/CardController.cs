using PaymentGateway.Data;
using PaymentGateway.Processor;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace PaymentGateway.Controllers
{
    [ApiController]
    [Route("api/cardverification")]
    public class CardController : Controller
    {
        private readonly ILogger<CardController> _logger;
        private readonly ILuhnAlgorithm _luhnAlgorithm;

        public CardController(ILogger<CardController> logger, ILuhnAlgorithm luhnAlgoRithm)
        {
            _logger = logger;
            _luhnAlgorithm = luhnAlgoRithm;
        }

        [HttpPost]
        [Route("verifycard")]
        public async Task<IActionResult> ValidateCard([FromBody] Purchase? purchase)
        {
            if (purchase == null)
            {
                return BadRequest();
            }

            var result = await _luhnAlgorithm.Check(purchase.CreditCard.CardNumber);
            if (!result)
            {
                return BadRequest(purchase.CreditCard);
            }
            else
            {
                return Ok(result);
            }

        }
    }
}
