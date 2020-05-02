using AutoMapper;
using MatchBox.Controllers;
using MatchBox.Data;
using MatchBox.Data.Models;
using MatchBox.Internal;
using MatchBox.Models.Mapping;
using MatchBox.Services.Email;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace MatchBox.Tests
{
    public class IocFixture : IAsyncLifetime
    {
        public const string FileName = "TestData.db";
        public const string ConnectionString = "Filename=.\\" + FileName;

        public IServiceProvider ServiceProvider { get; private set; }        

        IConfigurationRoot GetConfiguration()
        {
            return new ConfigurationBuilder()
                        .AddJsonFile("testsettings.json", optional: false)
                        .AddEnvironmentVariables()
                        .Build();
        }

        public async Task InitializeAsync()
        {
            var serviceCollection = new ServiceCollection();
            var cfg = GetConfiguration();

            serviceCollection.AddSingleton(cfg);

            var startup = new MatchBox.Startup(cfg);

            //serviceCollection
            //    .AddDbContext<MatchBoxDbContext>(options => options.UseSqlite(ConnectionString),
            //        ServiceLifetime.Transient);
            //
            serviceCollection.AddLogging(builder =>
            {
                builder.AddConsole();
            });
            //
            //serviceCollection.AddIdentity<DbUser, DbRole>(options =>
            //{
            //    options.Password.RequireLowercase = true;
            //    options.Password.RequireUppercase = true;
            //    options.Password.RequireNonAlphanumeric = true;
            //    options.Password.RequireDigit = true;
            //    
            //    options.Lockout.AllowedForNewUsers = true;
            //    options.User.RequireUniqueEmail = true;
            //    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
            //    options.Lockout.MaxFailedAccessAttempts = 3;
            //    //options.User.AllowedUserNameCharacters = settings.User.AllowedUserNameCharacters;
            //})
            //        .AddEntityFrameworkStores<MatchBoxDbContext>()
            //        .AddDefaultTokenProviders();
            //
            serviceCollection.AddTransient<IHttpContextAccessor>(
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



            //
            //serviceCollection.AddAutoMapper(typeof(APIAutoMapProfile));
            //serviceCollection.AddTransient<IJwtProducer, JwtProducer>();
            //serviceCollection.AddTransient<IEmailSender, EmailSender>();
            //
            //serviceCollection.AddTransient<UsersController>();
            //serviceCollection.AddTransient<EventsController>();
            //serviceCollection.AddTransient<GroupsController>();
            //serviceCollection.AddTransient<UtilityController>();

            ServiceProvider = serviceCollection.BuildServiceProvider();

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