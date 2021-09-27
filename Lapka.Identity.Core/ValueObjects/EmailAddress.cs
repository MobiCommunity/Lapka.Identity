using System.Text.RegularExpressions;
using Lapka.Identity.Core.Exceptions;

namespace Lapka.Identity.Core.ValueObjects
{
    public class EmailAddress
    {
        public string Value { get; }

        public EmailAddress(string email)
        {
            Value = email;
            
            Validate();
        }

        private void Validate()
        {
            if (!EmailRegex.IsMatch(Value))
            {
                throw new InvalidEmailValueException(Value);
            }
        }

        private static readonly Regex EmailRegex = new Regex(EmailRegExPattern,
            RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);


        private const string EmailRegExPattern =
            @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
            @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";
    }
}