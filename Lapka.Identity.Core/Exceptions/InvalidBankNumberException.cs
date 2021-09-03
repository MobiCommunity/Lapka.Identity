using Lapka.Identity.Core.Exceptions.Abstract;

namespace Lapka.Identity.Core.Exceptions
{
    public class InvalidBankNumberException : DomainException
    {
        public string BankNumber { get; }
        public InvalidBankNumberException(string bankNumber) : base($"Invalid bank number: {bankNumber}")
        {
            BankNumber = bankNumber;
        }

        public override string Code => "invalid_bank_number";
    }
}