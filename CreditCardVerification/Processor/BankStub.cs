
using PaymentGateway.Data;
using System;
using System.Threading.Tasks;

namespace PaymentGateway.Processor
{
    public  class BankStub : IBankStub
    {
        public async Task<bool> TransactionIsValid(Purchase purchase)
        {
            return await CheckCard(purchase.CreditCard);
        }

        private Task<bool> CheckCard(CreditCard? creditCard)
        {
            var result =
                (creditCard == null || string.IsNullOrEmpty(creditCard.CardNumber)) ||
                (string.IsNullOrEmpty(creditCard.CardHolder)) ||
                (creditCard.CVV == null || creditCard.CVV.Length != 3) ||
                (Convert.ToInt32(creditCard.Year.Substring(0, 2)) < Convert.ToInt32(DateTime.Now.Year.ToString().Substring(2, 2))) ||
                (Convert.ToInt32(creditCard.Month) == DateTime.Now.Year) && (Convert.ToInt32(DateTime.Now.Month) < Convert.ToInt32(creditCard.Month)) ||
                (Convert.ToInt32(creditCard.Month) > 12);

            return Task.FromResult(!result);

        }
    }
}
