using System.Text.RegularExpressions;
using Lapka.Identity.Core.Exceptions;

namespace Lapka.Identity.Core.ValueObjects
{
    public class PhoneNumber
    {
        public string Value { get; }
        public bool IsEmpty => string.IsNullOrEmpty(Value);
        
        public PhoneNumber(string phoneNumber)
        {
            Value = phoneNumber;
            
            Validate();
        }

        private void Validate()
        {
            if (IsEmpty) return;
            
            if (!PhoneNumberRegex.IsMatch(Value))
            {
                throw new InvalidPhoneNumberException(Value);
            }
        }
        
        private static readonly Regex PhoneNumberRegex =
            new Regex(@"(?<!\w)(\(?(\+|00)?48\)?)?[ -]?\d{3}[ -]?\d{3}[ -]?\d{3}(?!\w)",
                RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);
    }
}