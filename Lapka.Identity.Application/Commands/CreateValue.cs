using Convey.CQRS.Commands;
using System;

namespace Lapka.Identity.Application.Commands
{
    public class CreateValue : ICommand
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Description { get; }

        public CreateValue(string name, string description, Guid guid )
        {
            Id = guid;
            Name = name;
            Description = description;
        }
    }
}