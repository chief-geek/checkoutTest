using checkout.com.api;
using checkout.com.api.Controllers;
using checkout.com.api.Data;
using checkout.com.api.Database;
using checkout.com.api.Integrations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using System.Net;

namespace CheckoutTest
{
    public class CheckoutApiCreditCardTests
    {

        [Fact]
        public async void Pass_Model_to_Controller_And_It_Fails()
        {
            var purchase = new Purchase
            {
                CreditCard = null,
                Amount = 100
            };

            var logger = new Mock<ILogger<CheckOutController>>();

            var endPointConfig = new EndPointConfig
            {
                CardVerificationEndPoint = "https://localhost:7050"
            };
            var options = new Mock<IOptions<EndPointConfig>>();
            options.Setup(x => x.Value).Returns(endPointConfig);

            var cardVerifier = new Mock<ICardVerification>();
            cardVerifier.Setup(x=>x.CheckCard(It.IsAny<Purchase>()))
                .Returns(() => Task.FromResult(false));

            var repository = new Mock<IRepository>();
            var guid = Guid.NewGuid();
            repository.Setup(x => x.SavePurchase(It.IsAny<Purchase>()))
                .Returns(() => Task.FromResult("123456"));

            var bank = new Mock<IBankRequest>();
            bank.Setup(x => x.BankAccepts(It.IsAny<Purchase>()))
                .Returns(() => Task.FromResult(false));

            var controller = new CheckOutController(logger.Object, options.Object, cardVerifier.Object, repository.Object, bank.Object);

            var expectedResult = new CheckoutResult
            {
                StatusCode = HttpStatusCode.BadRequest,
                Message = "Missing card details provided, please ensure you provide all the require information!",
                Card = null,
                TransactionId = null
            };

            var result = await controller.AcceptCardPayment(purchase);

            Assert.Equal(HttpStatusCode.BadRequest, expectedResult.StatusCode);
        }

        [Fact]
        public async void Pass_Model_to_Controller_And_It_Passes()
        {
            var purchase = new Purchase
            {
                CreditCard = new CreditCard
                {
                    CardHolder = "Mr Jai Holloway",
                    CardNumber = "378 7344 9367 1000",
                    CVV = "423",
                    Month = "09",
                    Year = "25"
                },
                Amount = 100
            };

            var logger = new Mock<ILogger<CheckOutController>>();
            var endPointConfig = new EndPointConfig
            {
                CardVerificationEndPoint = "https://localhost:7050"
            };

            var options = new Mock<IOptions<EndPointConfig>>();
            options.Setup(x => x.Value).Returns(endPointConfig);

            var cardVerifier = new Mock<ICardVerification>();
            cardVerifier.Setup(x => x.CheckCard(It.IsAny<Purchase>()))
                .Returns(() => Task.FromResult(true));

            var repository = new Mock<IRepository>();
            repository.Setup(x => x.SavePurchase(It.IsAny<Purchase>()))
                .Returns(() => Task.FromResult("123456"));

            var bank = new Mock<IBankRequest>();
            bank.Setup(x => x.BankAccepts(It.IsAny<Purchase>()))
                .Returns(() => Task.FromResult(true));

            var controller = new CheckOutController(logger.Object, options.Object, cardVerifier.Object, repository.Object, bank.Object);

            var expectedResult = new CheckoutResult
            {
                StatusCode = HttpStatusCode.OK,
                Message = null,
                Card = purchase.CreditCard,
                TransactionId = "123456"
            };
            
            var result = await controller.AcceptCardPayment(purchase);

            Assert.Equal(HttpStatusCode.OK, expectedResult.StatusCode);
        }

        [Fact]
        public async void Pass_Model_to_Controller_And_It_Fails_For_Bank()
        {
            var purchase = new Purchase
            {
                CreditCard = new CreditCard
                {
                    CardHolder = "Mr Jai Holloway",
                    CardNumber = "378 7344 9367 1000",
                    CVV = "423",
                    Month = "",
                    Year = "25"
                },
                Amount = 100
            };

            var logger = new Mock<ILogger<CheckOutController>>();

            var endPointConfig = new EndPointConfig
            {
                CardVerificationEndPoint = "https://localhost:7050"
            };
            var options = new Mock<IOptions<EndPointConfig>>();
            options.Setup(x => x.Value).Returns(endPointConfig);

            var cardVerifier = new Mock<ICardVerification>();
            cardVerifier.Setup(x => x.CheckCard(It.IsAny<Purchase>()))
                .Returns(() => Task.FromResult(false));

            var repository = new Mock<IRepository>();
            var guid = Guid.NewGuid();
            repository.Setup(x => x.SavePurchase(It.IsAny<Purchase>()))
                .Returns(() => Task.FromResult("123456"));

            var bank = new Mock<IBankRequest>();
            bank.Setup(x => x.BankAccepts(It.IsAny<Purchase>()))
                .Returns(() => Task.FromResult(false));

            var controller = new CheckOutController(logger.Object, options.Object, cardVerifier.Object, repository.Object, bank.Object);

            var expectedResult = HttpStatusCode.BadRequest;

            var result = await controller.AcceptCardPayment(purchase);
            var transaction = new TransactionResult
            {
                Amount = null,
                CardHolder = null,
                CardNumber = null,
                ExpiryMonth = null,
                ExpiryYear = null,
                TransactionId = null
            };
            Assert.Equal(HttpStatusCode.BadRequest, expectedResult);
            Assert.Null(transaction.Amount);
            Assert.Null(transaction.CardHolder);
            Assert.Null(transaction.CardNumber);
            Assert.Null(transaction.ExpiryMonth);
            Assert.Null(transaction.ExpiryYear);
            Assert.Null(transaction.TransactionId);
        }

        [Fact]
        public async void Pass_Model_to_Controller_And_It_Passes_To_Bank()
        {
            var purchase = new Purchase
            {
                CreditCard = new CreditCard
                {
                    CardHolder = "Mr Jai Holloway",
                    CardNumber = "378 7344 9367 1000",
                    CVV = "423",
                    Month = "09",
                    Year = "25"
                },
                Amount = 100
            };

            var logger = new Mock<ILogger<CheckOutController>>();
            var endPointConfig = new EndPointConfig
            {
                CardVerificationEndPoint = "https://localhost:7050"
            };

            var options = new Mock<IOptions<EndPointConfig>>();
            options.Setup(x => x.Value).Returns(endPointConfig);

            var cardVerifier = new Mock<ICardVerification>();
            cardVerifier.Setup(x => x.CheckCard(It.IsAny<Purchase>()))
                .Returns(() => Task.FromResult(true));

            var repository = new Mock<IRepository>();
            repository.Setup(x => x.SavePurchase(It.IsAny<Purchase>()))
                .Returns(() => Task.FromResult("123456"));

            var bank = new Mock<IBankRequest>();
            bank.Setup(x => x.BankAccepts(It.IsAny<Purchase>()))
                .Returns(() => Task.FromResult(true));

            var controller = new CheckOutController(logger.Object, options.Object, cardVerifier.Object, repository.Object, bank.Object);

            var expectedResult = new CheckoutResult
            {
                StatusCode = HttpStatusCode.OK,
                Message = null,
                Card = purchase.CreditCard,
                TransactionId = "123456"
            };

            var result = await controller.AcceptCardPayment(purchase);

            Assert.Equal(HttpStatusCode.OK, expectedResult.StatusCode);
        }
    }
}