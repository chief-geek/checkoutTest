using checkout.com.api.BusinessLogic;
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
        private readonly IBusinessLogicLayer _logic;
        private readonly IRepository _repository;

        public CheckOutController(ILogger<CheckOutController> logger, IBusinessLogicLayer businessLogicLayer, IRepository repository)
        {
            _logger = logger;
            _logic = businessLogicLayer;
            _repository = repository;

        }

        [Route("acceptpayment")]
        [HttpPost]
        public async Task<IActionResult> AcceptCardPayment(Purchase purchase)
        {
            var result = await _logic.Run(purchase);
            if (result.StatusCode == HttpStatusCode.BadRequest)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("gettransaction")]
        public async Task<IActionResult> GetTransaction(string transactionId)
        {
            var result = await _repository.GetTransaction(transactionId);
            if (result == null)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        
    }
}