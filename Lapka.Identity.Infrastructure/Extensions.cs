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
using Convey.Auth;
using Convey.Persistence.MongoDB;
using Lapka.Identity.Application.Events.Abstract;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Infrastructure.Auth;
using Lapka.Identity.Infrastructure.Documents;
using Lapka.Identity.Infrastructure.Exceptions;
using Lapka.Identity.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;


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
                // .AddRabbitMq()
                .AddJwt()
                .AddMongo()
                .AddMongoRepository<ShelterDocument, Guid>("Shelters")
                .AddMongoRepository<UserDocument, Guid>("Users")
                .AddMongoRepository<JsonWebTokenDocument, Guid>("RefreshTokens")
                // .AddConsul()
                // .AddFabio()
                // .AddMessageOutbox()
                // .AddMetrics()
                ;

            builder.Services.Configure<KestrelServerOptions>
                (o => o.AllowSynchronousIO = true);

            builder.Services.Configure<IISServerOptions>(o => o.AllowSynchronousIO = true);

            IServiceCollection services = builder.Services;


            services.AddSingleton<IExceptionToResponseMapper, ExceptionToResponseMapper>();
            services.AddSingleton<IDomainToIntegrationEventMapper, DomainToIntegrationEventMapper>();

            services.AddTransient<IEventProcessor, EventProcessor>();
            services.AddTransient<IMessageBroker, DummyMessageBroker>();

            services.AddSingleton<IShelterRepository, ShelterRepository>();
            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddSingleton<IJwtProvider, JwtProvider>();
            services.AddSingleton<IPasswordService, PasswordService>();
            services.AddSingleton<IPasswordHasher<IPasswordService>, PasswordHasher<IPasswordService>>();
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<IRefreshTokenService, RefreshTokenService>();
            services.AddSingleton<IRng, Rng>();
            services.AddTransient<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            // services.AddIdentity<UserDocument, List<string>>().AddDefaultTokenProviders();

            builder.Services.Scan(s => s.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
                .AddClasses(c => c.AssignableTo(typeof(IDomainEventHandler<>)))
                .AsImplementedInterfaces().WithTransientLifetime());

            return builder;
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
        {
            app
                .UseErrorHandler()
                .UseConvey()
                .UseAuthentication()
                //.UseMetrics()
                //.UseRabbitMq()
                ;


            return app;
        }
    }
}