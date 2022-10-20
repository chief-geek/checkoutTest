using PaymentGateway.Processor;
using PaymentGateway.Data;

namespace PaymentGatewayTests
{
    public class BankStubTest
    {
        BankStub bankStub = new BankStub();

        [Fact]
        public async void Card_Does_Not_Pass_Due_To_Invalid_Date()
        {
            var Purchase = new Purchase {
            CreditCard = new CreditCard
            {
                CardHolder = "Mr Jai Holloway",
                CardNumber = "1234 5678 4321 0987",
                CVV = "422",
                Month = "09",
                Year = "21"
            },
            Amount=100
            };

            var result = await bankStub.TransactionIsValid(Purchase);
            Assert.False(result);
        }

        [Fact]
        public async void Card_Does_Not_Pass_Due_To_Missing_Name()
        {
            var Purchase = new Purchase
            {
                CreditCard = new CreditCard
                {
                    CardHolder = "",
                    CardNumber = "1234 5678 4321 0987",
                    CVV = "422",
                    Month = "09",
                    Year = "22"
                },
                Amount = 100
            };

            var result = await bankStub.TransactionIsValid(Purchase);
            Assert.False(result);
        }

        [Fact]
        public async void Card_Does_Not_Pass_Due_To_No_Card()
        {
            var Purchase = new Purchase
            {
                CreditCard = new CreditCard
                {
                    CardHolder = "Mr Jai Holloway",
                    CardNumber = "",
                    CVV = "422",
                    Month = "09",
                    Year = "22"
                },
                Amount = 100
            };

            var result = await bankStub.TransactionIsValid(Purchase);
            Assert.False(result);
        }

        [Fact]
        public async void Card_Does_Not_Pass_Due_To_No_CVV()
        {
            var Purchase = new Purchase
            {
                CreditCard = new CreditCard
                {
                    CardHolder = "Mr Jai Holloway",
                    CardNumber = "",
                    CVV = null,
                    Month = "09",
                    Year = "22"
                },
                Amount = 100
            };

            var result = await bankStub.TransactionIsValid(Purchase);
            Assert.False(result);
        }

        [Fact]
        public async void Card_Does_Pass()
        {
            var Purchase = new Purchase
            {
                CreditCard = new CreditCard
                {
                    CardHolder = "Mr Jai Holloway",
                    CardNumber = "3787 3449 3671 000",
                    CVV = "427",
                    Month = "09",
                    Year = "25"
                },
                Amount = 100
            };

            var result = await bankStub.TransactionIsValid(Purchase);
            Assert.True(result);
        }
    }
}
