using PaymentGateway.Data;
using PaymentGateway.Processor;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PaymentGateway.Controllers
{
    [Route("api/bank")]
    [ApiController]
    public class BankController : ControllerBase
    {
        private readonly ILogger<BankController> _logger;
        private readonly IBankStub _bankStub;

        public BankController(ILogger<BankController> logger, IBankStub bankStub)
        {
            _logger = logger;
            _bankStub = bankStub;
        }

        [HttpPost]
        [Route("verifytransaction")]
        public async Task<bool> CheckBankTransaction(Purchase purchase)
        {
            return await _bankStub.TransactionIsValid(purchase);
        }
    }
}
