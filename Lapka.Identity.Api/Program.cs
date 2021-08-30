using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Convey;
using Convey.Logging;
using Lapka.Identity.Api.Attributes;
using Lapka.Identity.Application;
using Lapka.Identity.Infrastructure;
using Lapka.Pets.Api.gRPC.Controllers;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;
using Open.Serialization.Json.Newtonsoft;

namespace Lapka.Identity.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateWebHostBuilder(args).Build().RunAsync();
        }

        private static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args).ConfigureKestrel(options =>
                {
                    options.ListenAnyIP(5014, o => o.Protocols = 
                        HttpProtocols.Http2);
                    options.ListenAnyIP(5004, o => o.Protocols =
                        HttpProtocols.Http1);
                }).ConfigureServices(services =>
                {
                    services.AddControllers();

                    services.TryAddSingleton(new JsonSerializerFactory().GetSerializer());
                    services.AddGrpc();
                    
                    services
                        .AddConvey()
                        .AddInfrastructure()
                        .AddApplication();
                    

                    services.AddSwaggerGen(c =>
                    {
                        c.SwaggerDoc("v1", new OpenApiInfo
                        {
                            Version = "v1",
                            Title = "identity Microservice",
                            Description = ""
                        });
                        string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                        string xmlFile2 = "Lapka.Identity.Application.xml";
                        string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                        string xmlPath2 = Path.Combine(AppContext.BaseDirectory, xmlFile2);
                        c.OperationFilter<BasicAuthOperationsFilter>();
                        c.IncludeXmlComments(xmlPath);
                        c.IncludeXmlComments(xmlPath2);
                    });
                    

                    services.BuildServiceProvider();
                }).Configure(app =>
                {
                    app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

                    app
                        .UseConvey()
                        .UseInfrastructure()
                        .UseRouting()
                        .UseSwagger(c => { c.RouteTemplate = "api/identity/swagger/{documentname}/swagger.json"; })
                        .UseSwaggerUI(c =>
                        {
                            c.SwaggerEndpoint("/api/identity/swagger/v1/swagger.json", "My API V1");
                            c.RoutePrefix = "api/identity/swagger";
                        })
                        .UseEndpoints(e =>
                        {
                            e.MapControllers();
                            e.MapGrpcService<GrpcPetController>();
                            e.Map("ping", async ctx => { await ctx.Response.WriteAsync("Alive"); });
                        });
                })
                .UseLogging();
        }
    }
}