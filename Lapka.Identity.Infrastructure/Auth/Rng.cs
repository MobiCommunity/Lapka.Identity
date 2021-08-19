using System;
using System.Linq;
using System.Security.Cryptography;
using Lapka.Identity.Application.Services;

namespace Lapka.Identity.Infrastructure.Auth
{
    internal sealed class Rng : IRng
    {
        private static readonly string[] SpecialChars = new[] {"/", "\\", "=", "+", "?", ":", "&"};

        public string Generate(int length = 50, bool removeSpecialChars = true)
        {
            using RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] bytes = new byte[length];
            rng.GetBytes(bytes);
            string result = Convert.ToBase64String(bytes);

            return removeSpecialChars
                ? SpecialChars.Aggregate(result, (current, chars) => current.Replace(chars, string.Empty))
                : result;
        }
    }
}