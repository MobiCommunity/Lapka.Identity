using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using Lapka.Identity.Application.Dto;
using Lapka.Identity.Infrastructure.Elastic.Options;
using Lapka.Identity.Infrastructure.Mongo.Documents;
using Lapka.Identity.Infrastructure.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nest;

namespace Lapka.Identity.Infrastructure.Elastic.Services
{
    public class ElasticSearchSeeder : IHostedService
    {
        private readonly ILogger<ElasticSearchSeeder> _logger;
        private readonly IMongoRepository<UserDocument, Guid> _userRepository;
        private readonly IMongoRepository<ShelterOwnerApplicationDocument, Guid> _shelterOwnerApplicationRepository;
        private readonly IMongoRepository<ShelterDocument, Guid> _shelterRepository;
        private readonly IElasticClient _client;
        private readonly ElasticSearchOptions _elasticOptions;

        public ElasticSearchSeeder(ILogger<ElasticSearchSeeder> logger,
            IMongoRepository<UserDocument, Guid> userRepository,
            IMongoRepository<ShelterOwnerApplicationDocument, Guid> shelterOwnerApplicationRepository,
            IMongoRepository<ShelterDocument, Guid> shelterRepository,
            IElasticClient client, ElasticSearchOptions elasticOptions)
        {
            _logger = logger;
            _userRepository = userRepository;
            _shelterOwnerApplicationRepository = shelterOwnerApplicationRepository;
            _shelterRepository = shelterRepository;
            _client = client;
            _elasticOptions = elasticOptions;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await SeedUsersAsync();
            await SeedSheltersAsync();
            await SeedShelterOwnerApplicationAsync();
        }

        private async Task SeedUsersAsync()
        {
            IReadOnlyList<UserDocument> userDocuments = await _userRepository.FindAsync(_ => true);

            await _client.Indices.DeleteAsync(_elasticOptions.Aliases.Users);

            BulkAllObservable<UserDto> bulkUsers =
                _client.BulkAll(userDocuments.Select(x => x.AsDto()), b => b.Index(_elasticOptions.Aliases.Users));

            bulkUsers.Wait(TimeSpan.FromMinutes(5), x => _logger.LogInformation("Users indexed"));
        }

        private async Task SeedSheltersAsync()
        {
            IReadOnlyList<ShelterDocument> shelterDocuments = await _shelterRepository.FindAsync(_ => true);

            await _client.Indices.DeleteAsync(_elasticOptions.Aliases.Shelters);

            BulkAllObservable<ShelterDocument> shelters = _client.BulkAll(shelterDocuments,
                b => b.Index(_elasticOptions.Aliases.Shelters));

            shelters.Wait(TimeSpan.FromMinutes(5), x => _logger.LogInformation("Shelter indexed"));
        }

        private async Task SeedShelterOwnerApplicationAsync()
        {
            IReadOnlyList<ShelterOwnerApplicationDocument> shelterOwnerApplicationDocuments =
                await _shelterOwnerApplicationRepository.FindAsync(_ => true);

            await _client.Indices.DeleteAsync(_elasticOptions.Aliases.ShelterOwnerApplications);

            BulkAllObservable<ShelterOwnerApplicationDocument> shelterOwnerApplications =
                _client.BulkAll(shelterOwnerApplicationDocuments,
                    b => b.Index(_elasticOptions.Aliases.ShelterOwnerApplications));

            shelterOwnerApplications.Wait(TimeSpan.FromMinutes(5),
                x => _logger.LogInformation("Shelter owner applications indexed"));
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return default;
        }
    }
}