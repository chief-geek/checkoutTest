using PaymentGateway.Processor;

namespace PaymentGatewayTests
{
    public class LuhnAlgorithmTests
    {
        LuhnAlgorithm luhnAlgorithm = new LuhnAlgorithm();
        [Fact]
        public async void Amex_Luhn_Algorithm_Returns_True()
        {
            var result = await luhnAlgorithm.Check("378282246310005");
            Assert.True(result);
        }

        [Fact]
        public async void Amex_Luhn_Algorithm_Returns_False()
        {
            var result = await luhnAlgorithm.Check("3742 4545 5400 126");
            Assert.False(result);   
        }

        [Fact]
        public async void MasterCard_Luhn_Algorithm_Returns_True()
        {
            var result = await luhnAlgorithm.Check("5105 1051 0510 5100");
            Assert.True(result);
        }

        [Fact]
        public async void Card_Number_Not_Long_Enough()
        {            
            var result = await luhnAlgorithm.Check("5425 2334 3010 ");
            Assert.False(result);
        }
    }
}