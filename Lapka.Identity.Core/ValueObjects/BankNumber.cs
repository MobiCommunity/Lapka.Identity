using System.Text.RegularExpressions;
using Lapka.Identity.Core.Exceptions;

namespace Lapka.Identity.Core.ValueObjects
{
    public class BankNumber
    {
        public string Value { get; }

        public BankNumber(string bankNumber)
        {
            Value = bankNumber;
            
            Validate();
        }

        private void Validate()
        {
            if (string.IsNullOrEmpty(Value)) return;
            
            if (Value.Length < BankNumberMinimumLetters)
            {
                throw new InvalidBankNumberException(Value);
            }
        }
        
        private const int BankNumberMinimumLetters = 5;
    }
}