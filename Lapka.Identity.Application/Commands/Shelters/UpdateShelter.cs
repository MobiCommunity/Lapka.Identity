using System;
using Convey.CQRS.Commands;
using Lapka.Identity.Api.Models;
using Lapka.Identity.Core.ValueObjects;

namespace Lapka.Identity.Application.Commands.Shelters
{
    public class UpdateShelter : ICommand
    {
        public Guid Id { get; }
        public UserAuth UserAuth { get; }
        public string Name { get; }
        public string PhoneNumber { get; }
        public string Email { get; }
        public string BankNumber { get; }
        
        public UpdateShelter(Guid id, UserAuth userAuth, string name, string phoneNumber, string email, string bankNumber)
        {
            Id = id;
            UserAuth = userAuth;
            Name = name;
            PhoneNumber = phoneNumber;
            Email = email;
            BankNumber = bankNumber;
        }
    }
}