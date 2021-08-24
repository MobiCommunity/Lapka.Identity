using System;
using Convey.CQRS.Commands;

namespace Lapka.Identity.Application.Commands
{
    public class SignInGoogle : ICommand
    { 
        public string AccessToken { get; }

        public SignInGoogle(string accessToken)
        {
            AccessToken = accessToken;
        }
    }
}