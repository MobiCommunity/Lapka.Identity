using System;
using Newtonsoft.Json;

namespace Lapka.Identity.Core.ValueObjects
{
    public class GoogleUser
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string Picture { get; set; }
    }
}