using Convey.CQRS.Events;
using Convey.MessageBrokers;
using Convey.MessageBrokers.Outbox;
using Convey.MessageBrokers.RabbitMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lapka.Identity.Application.Services;

// ReSharper disable NotAccessedField.Local

namespace Lapka.Identity.Infrastructure.Services
{
    internal sealed class FakeMessageBroker : IMessageBroker
    {


        public FakeMessageBroker()
        {

        }

        public Task PublishAsync(params IEvent[] events) => Task.CompletedTask;

        public Task PublishAsync(IEnumerable<IEvent> events)
        {
            return Task.CompletedTask;
        }
    }
}