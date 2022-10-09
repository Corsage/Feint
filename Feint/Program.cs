// See https://aka.ms/new-console-template for more information

using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Feint.Core.Models;
using Feint.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Feint.Core
{
    public class Program
    {
        private static IHostBuilder CreateHostBuilder()
        {
            var builder = Host.CreateDefaultBuilder();

            // Setup default json config -- always set this up first.
            builder.ConfigureAppConfiguration((context, configBuilder) =>
            {
                // config file for prefix YAML
                //configBuilder.AddJsonFile("config.json", false, true);
                configBuilder.AddJsonFile("secrets.json", false, true);
            });

            // Configure logging.
            builder.ConfigureLogging((context, logging) =>
            {
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .WriteTo.Console()
                    .WriteTo.File("log.txt",
                    rollingInterval: RollingInterval.Day,
                    rollOnFileSizeLimit: true)
                    .CreateLogger();

                // Clear default providers -- console, event log, etc.
                logging.ClearProviders();

                // Now only using Serilog as our default logger.
                // dispose: true, allow Serilog to be disposed of by the Host.
                logging.AddSerilog(dispose: true);
            });

            // Setup Discord Singleton.
            builder.ConfigureServices((context, services) =>
            {
                services.Configure<DiscordSettings>(context.Configuration.GetSection("Discord"));

                services
                .AddSingleton(x => new DiscordSocketClient(new DiscordSocketConfig
                {
                    // AllUnprivileged (privileged intent is messages intent--> switched to all) 
                    GatewayIntents = GatewayIntents.All,
                    AlwaysDownloadUsers = true
                }))

                // adding in singletons

                // ADD interaction service 
                .AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>()))
                .AddSingleton<InteractionHandler>()

                // prefix commands 
                .AddSingleton(x => new CommandService())
                .AddSingleton<PrefixHandler>();

                // Let the host manage this service -- it controls the StartAsync, StopAsync, and ExecuteAsync.
                // StartAsync is meant for setup.
                // ExecuteASync is meant for body.
                // StopAsync is meant for stopping/ending/disposing.
                services.AddHostedService<DiscordBotService>();
            });

            return builder;
        }

        public static void Main()
        {
            // Application
            AppException.Install();

            var host = CreateHostBuilder().Build();
            host.Run();
        }
    }
}


