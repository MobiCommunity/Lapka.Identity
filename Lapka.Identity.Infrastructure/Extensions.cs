using Convey;
using Convey.CQRS.Queries;
using Convey.HTTP;
using Convey.MessageBrokers.RabbitMQ;
using Convey.WebApi;
using Convey.WebApi.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Convey.Auth;
using Convey.MessageBrokers.CQRS;
using Convey.MessageBrokers.Outbox;
using Convey.Persistence.MongoDB;
using Lapka.Identity.Api.Models;
using Lapka.Identity.Application.Commands.ShelterOwnership;
using Lapka.Identity.Application.Commands.Shelters;
using Lapka.Identity.Application.Commands.Users;
using Lapka.Identity.Application.Events.Abstract;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Application.Services.Auth;
using Lapka.Identity.Application.Services.Elastic;
using Lapka.Identity.Application.Services.Grpc;
using Lapka.Identity.Application.Services.Repositories;
using Lapka.Identity.Core.Events.Concrete.Shelters;
using Lapka.Identity.Infrastructure.Auths;
using Lapka.Identity.Infrastructure.Elastic;
using Lapka.Identity.Infrastructure.Elastic.Options;
using Lapka.Identity.Infrastructure.Elastic.Services;
using Lapka.Identity.Infrastructure.Exceptions;
using Lapka.Identity.Infrastructure.Grpc;
using Lapka.Identity.Infrastructure.Grpc.Options;
using Lapka.Identity.Infrastructure.Mongo;
using Lapka.Identity.Infrastructure.Mongo.Documents;
using Lapka.Identity.Infrastructure.Mongo.Repositories;
using Lapka.Identity.Infrastructure.Options;
using Lapka.Identity.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Nest;


namespace Lapka.Identity.Infrastructure
{
    public static class Extensions
    {
        public static IConveyBuilder AddInfrastructure(this IConveyBuilder builder)
        {
            builder
                .AddQueryHandlers()
                .AddInMemoryQueryDispatcher()
                .AddHttpClient()
                .AddErrorHandler<ExceptionToResponseMapper>()
                .AddExceptionToMessageMapper<ExceptionToMessageMapper>()
                .AddMongoRepository<ShelterDocument, Guid>("shelters")
                .AddMongoRepository<UserDocument, Guid>("users")
                .AddMongoRepository<JsonWebTokenDocument, Guid>("refreshTokens")
                .AddMongoRepository<ShelterOwnerApplicationDocument, Guid>("shelterOwnerApplications")
                .AddMongo()
                .AddJwt()
                .AddRabbitMq()
                .AddMessageOutbox()
                // .AddConsul()
                // .AddFabio()
                // .AddMessageOutbox()
                // .AddMetrics()
                ;

            builder.Services.Configure<KestrelServerOptions>
                (o => o.AllowSynchronousIO = true);

            builder.Services.Configure<IISServerOptions>(o => o.AllowSynchronousIO = true);

            IServiceCollection services = builder.Services;
            services.AddHttpClient();
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            ServiceProvider provider = services.BuildServiceProvider();
            IConfiguration configuration = provider.GetService<IConfiguration>();

            GoogleAuthSettings googleAuthSettings = new GoogleAuthSettings();
            configuration.GetSection("google").Bind(googleAuthSettings);
            services.AddSingleton(googleAuthSettings);

            FacebookAuthSettings facebookOptions = new FacebookAuthSettings();
            configuration.GetSection("FacebookAuthSettings").Bind(facebookOptions);
            services.AddSingleton(facebookOptions);

            FilesMicroserviceOptions filesMicroserviceOptions = new FilesMicroserviceOptions();
            configuration.GetSection("filesMicroservice").Bind(filesMicroserviceOptions);
            services.AddSingleton(filesMicroserviceOptions);

            PetsMicroserviceOptions petsMicroserviceOptions = new PetsMicroserviceOptions();
            configuration.GetSection("petsMicroservice").Bind(petsMicroserviceOptions);
            services.AddSingleton(petsMicroserviceOptions);

            ElasticSearchOptions elasticSearchOptions = new ElasticSearchOptions();
            configuration.GetSection("elasticSearch").Bind(elasticSearchOptions);
            services.AddSingleton(elasticSearchOptions);
            ConnectionSettings elasticConnectionSettings = new ConnectionSettings(new Uri(elasticSearchOptions.Url));

            services.AddSingleton<IFacebookAuthenticator, FacebookAuthenticator>();
            services.AddSingleton<IJwtProvider, JwtProvider>();
            services.AddSingleton<IPasswordService, PasswordService>();
            services.AddSingleton<IPasswordHasher<IPasswordService>, PasswordHasher<IPasswordService>>();
            services.AddSingleton<IRng, Rng>();
            services.AddSingleton<IExceptionToResponseMapper, ExceptionToResponseMapper>();
            services.AddSingleton<IDomainToIntegrationEventMapper, DomainToIntegrationEventMapper>();
            services.AddSingleton<IElasticClient>(new ElasticClient(elasticConnectionSettings));

            services.AddTransient<IUserElasticsearchUpdater, UserElasticsearchUpdater>();
            services.AddTransient<IShelterElasticsearchUpdater, ShelterElasticsearchUpdater>();
            services.AddTransient<IShelterOwnerApplicationElasticSearchUpdater,
                ShelterOwnerApplicationElasticSearchUpdater>();
            services.AddTransient<IMongoDbSeeder, MongoDbSeeder>();
            services.AddTransient<IEventProcessor, EventProcessor>();
            services.AddTransient<IMessageBroker, MessageBroker>();
            services.AddTransient<IShelterRepository, ShelterRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<IRefreshTokenService, RefreshTokenService>();
            services.AddTransient<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IGrpcPhotoService, GrpcPhotoService>();
            services.AddTransient<IGoogleAuthenticator, GoogleAuthenticator>();
            services.AddTransient<IGrpcPhotoService, GrpcPhotoService>();
            services.AddTransient<IGrpcPetService, GrpcPetService>();
            services.AddTransient<IShelterOwnerApplicationRepository, ShelterOwnerApplicationRepository>();

            services.AddHostedService<ElasticSearchSeeder>();

            services.AddGrpcClient<PhotoProto.PhotoProtoClient>(o =>
            {
                o.Address = new Uri(filesMicroserviceOptions.UrlHttp2);
            });

            services.AddGrpcClient<PetProto.PetProtoClient>(o =>
            {
                o.Address = new Uri(petsMicroserviceOptions.UrlHttp2);
            });

            builder.Services.Scan(s => s.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
                .AddClasses(c => c.AssignableTo(typeof(IDomainEventHandler<>)))
                .AsImplementedInterfaces().WithTransientLifetime());

            builder.Build();
            
            return builder;
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
        {
            app
                .UseErrorHandler()
                .UseConvey()
                .UseAuthentication()
                .UseRabbitMq()
                .SubscribeCommand<CreateShelter>()
                .SubscribeCommand<DeleteShelter>()
                .SubscribeCommand<UpdateShelter>()
                .SubscribeCommand<UpdateUserPhoto>()
                .SubscribeCommand<UpdateShelterPhoto>()
                .SubscribeCommand<AcceptShelterOwnerApplication>()
                .SubscribeCommand<RemoveUserFromShelterOwners>()
                //.UseMetrics()
                ;

            return app;
        }

        public static async Task<UserAuth> AuthenticateUsingJwtGetUserAuthAsync(this HttpContext context)
        {
            AuthenticateResult authentication = await context.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);
            if (!authentication.Succeeded) return null;

            UserAuth userAuth = new UserAuth(
                authentication.Principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value,
                Guid.Parse(authentication.Principal.Identity.Name));

            return userAuth;
        }

        public static async Task<Guid> AuthenticateUsingJwtGetUserIdAsync(this HttpContext context)
        {
            AuthenticateResult authentication = await context.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);

            return authentication.Succeeded ? Guid.Parse(authentication.Principal.Identity.Name) : Guid.Empty;
        }

        public static async Task<string> AuthenticateUsingJwtGetUserRoleAsync(this HttpContext context)
        {
            AuthenticateResult authentication = await context.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);

            return authentication.Succeeded
                ? authentication.Principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value
                : string.Empty;
        }
    }
}