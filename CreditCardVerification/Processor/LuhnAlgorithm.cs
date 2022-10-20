using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway.Processor
{
    public  class LuhnAlgorithm : ILuhnAlgorithm
    {
        public  async Task<bool> Check(string? cardNumber)
        {
            if (cardNumber == null) return false;

            cardNumber = cardNumber.Replace(" ", "");
            if (cardNumber.Length < 13)
            {
                return false;
            }
            cardNumber = cardNumber.Substring(0, cardNumber.Length - 1);
            cardNumber.Reverse();
            var numbers = cardNumber.Select(x => x - 48).ToList();
            var result = await CalculateCheckDigit(numbers);
            return result;
        }

        private async Task<bool> CalculateCheckDigit(List<int> cardNumber)
        {
            var sum = 0;
            
            var isSecond = true;
            for (int i = cardNumber.Count - 1; i>= 0; i--)
            {
                if(isSecond)
                {
                    if (cardNumber[i] > 5)
                    {
                        sum += (cardNumber[i] * 2) - 9;
                    }
                    else
                    {
                        sum = cardNumber[i] * 2;
                    }
                }
                else
                {
                    sum += cardNumber[i];
                }
                isSecond = !isSecond;
            }
            
            return sum % 10 == 0;
        }

    }
}