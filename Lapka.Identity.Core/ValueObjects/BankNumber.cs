using System.Text.RegularExpressions;
using Lapka.Identity.Core.Exceptions;

namespace Lapka.Identity.Core.ValueObjects
{
    public class BankNumber
    {
        public string Value { get; }

        public BankNumber(string bankNumber)
        {
            ValidateBankNumber(bankNumber);

            Value = bankNumber;
        }

        private void ValidateBankNumber(string bankNumber)
        {
            if (string.IsNullOrEmpty(bankNumber)) return;
            
            if (bankNumber.Length < BankNumberMinimumLetters)
            {
                throw new InvalidBankNumberException(bankNumber);
            }
        }
        
        private const int BankNumberMinimumLetters = 5;
    }
}