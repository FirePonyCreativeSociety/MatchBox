using AutoMapper;
using MatchBox.Controllers;
using MatchBox.Data;
using MatchBox.Data.Models;
using MatchBox.Internal;
using MatchBox.Models.Mapping;
using MatchBox.Services.Email;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MatchBox.Tests
{
    public class IocFixture : IAsyncLifetime
    {
        public const string FileName = "TestData.db";
        public const string ConnectionString = "Filename=.\\" + FileName;

        public IServiceProvider ServiceProvider { get; private set; }        

        IConfigurationRoot BuildConfiguration()
        {
            return new ConfigurationBuilder()
                        .AddJsonFile("testsettings.json", optional: false)
                        .AddEnvironmentVariables()
                        .Build();
        }

        public async Task InitializeAsync()
        {
            // Creates the IConfiguration instance appropriately for tests
            var cfg = BuildConfiguration();
            var services = new ServiceCollection();
            services.AddSingleton(cfg);

            // Adds all controllers from main assembly. By default, the controllers are not loaded so this is necessary.
            // AddControllers() and AddApplicationPart().AddControllers() did not work.
            var controllers = typeof(AuthenticationController).Assembly.ExportedTypes.Where(x => !x.IsAbstract && typeof(ControllerBase).IsAssignableFrom(x)).ToList();
            controllers.ForEach(c => services.AddTransient(c));

            // Executes the same ConfigureServices() as the MatchBox application
            var startup = new MatchBox.Startup(cfg);
            startup.ConfigureServices(services);
            
            // Adds logging?
            services.AddLogging(builder =>
            {
                //
                builder.AddConsole();
            });
            
            // This is necessary for controller testing
            services.AddTransient<IHttpContextAccessor>(
                sp => 
                {
                    return new HttpContextAccessor 
                    { 
                        HttpContext = new DefaultHttpContext
                            {
                                RequestServices = ServiceProvider
                            }
                    };
                });

            // Builds the service provider
            ServiceProvider = services.BuildServiceProvider();

            // Now initializes the temporary db
            var init = new MatchBoxDbInitializer(ServiceProvider);
            await init.Initialize();
        }

        public Task DisposeAsync()
        {
            File.Delete(FileName);
            return Task.CompletedTask;
        }        
    }
}