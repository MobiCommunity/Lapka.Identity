using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Identity.Application.Commands.ShelterOwnership;
using Lapka.Identity.Application.Exceptions;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Core.ValueObjects;

namespace Lapka.Identity.Application.Commands.Handlers.ShelterOwnership
{
    public class DeclineShelterOwnerApplicationHandler : ICommandHandler<DeclineShelterOwnerApplication>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IShelterOwnerApplicationRepository _shelterOwnerApplicationRepository;

        public DeclineShelterOwnerApplicationHandler(IEventProcessor eventProcessor,
            IShelterOwnerApplicationRepository shelterOwnerApplicationRepository)
        {
            _eventProcessor = eventProcessor;
            _shelterOwnerApplicationRepository = shelterOwnerApplicationRepository;
        }

        public async Task HandleAsync(DeclineShelterOwnerApplication command)
        {
            ShelterOwnerApplication application = await _shelterOwnerApplicationRepository.GetAsync(command.Id);
            if (application is null) throw new ShelterOwnerApplicationNotFoundException(command.Id.ToString());
            
            if (application.Status != OwnerApplicationStatus.Pending)
                throw new OwnerApplicationStatusHasToBePendingException(application.Id.ToString(), application.Status);

            application.DeclineApplication();

            await _shelterOwnerApplicationRepository.UpdateAsync(application);
        }
    }
}