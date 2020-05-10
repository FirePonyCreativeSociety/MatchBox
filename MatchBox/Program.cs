using MatchBox.Data;
using MatchBox.Data.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace MatchBox
{
    public class Program
    {
        public static int Main(string[] args)
        {
            Console.WriteLine("Starting MatchBox!");
            try
            {
                var host = CreateHostBuilder(args).Build();

                using (var scope = host.Services.CreateScope())
                {
                    new MatchBoxDbInitializer(scope.ServiceProvider)
                        .Initialize()
                        .Wait();
                }

                host.Run();
                Console.WriteLine("MatchBox terminated OK.");
                
                return 0;
            }
            catch (Exception err)
            {
                Console.WriteLine($"MatchBox terminated NOT OK: {err.Message}");

                return 1;
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
