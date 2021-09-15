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
using Convey.Persistence.MongoDB;
using Lapka.Identity.Api.Models;
using Lapka.Identity.Application.Events.Abstract;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Application.Services.Auth;
using Lapka.Identity.Application.Services.Shelter;
using Lapka.Identity.Application.Services.User;
using Lapka.Identity.Infrastructure.Auths;
using Lapka.Identity.Infrastructure.Documents;
using Lapka.Identity.Infrastructure.Exceptions;
using Lapka.Identity.Infrastructure.Options;
using Lapka.Identity.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;


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
                .AddMongoRepository<ShelterOwnerApplicationDocument, Guid>("ShelterOwnerApplications")
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

            services.AddSingleton<IFacebookAuthenticator, FacebookAuthenticator>();
            services.AddSingleton<IJwtProvider, JwtProvider>();
            services.AddSingleton<IPasswordService, PasswordService>();
            services.AddSingleton<IPasswordHasher<IPasswordService>, PasswordHasher<IPasswordService>>();
            services.AddSingleton<IRng, Rng>();
            services.AddSingleton<IExceptionToResponseMapper, ExceptionToResponseMapper>();
            services.AddSingleton<IDomainToIntegrationEventMapper, DomainToIntegrationEventMapper>();

            services.AddTransient<IMongoDbSeeder, MongoDbSeeder>();
            services.AddTransient<IEventProcessor, EventProcessor>();
            services.AddTransient<IMessageBroker, DummyMessageBroker>();
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