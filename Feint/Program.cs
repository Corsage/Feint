// See https://aka.ms/new-console-template for more information
using Discord;
using Discord.WebSocket;
using Feint.Core.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Feint.Core
{
    public class Program
    {
        public IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddEnvironmentVariables()
            .AddJsonFile("secrets.json")
            .Build();

        public static Task Main() => new Program().MainAsync();

        public async Task MainAsync()
        {
            using IHost host = Host.CreateDefaultBuilder()
                .ConfigureServices((_, services) => services.AddSingleton(x => new DiscordSocketClient(new DiscordSocketConfig
                {
                    GatewayIntents = GatewayIntents.AllUnprivileged,
                    AlwaysDownloadUsers = true
                })))
                .Build();

            await RunAsync(host);
        }

        public async Task RunAsync(IHost host)
        {
            DiscordSettings settings = config.GetRequiredSection("Discord").Get<DiscordSettings>();

            using IServiceScope serviceScope = host.Services.CreateScope();
            IServiceProvider provider = serviceScope.ServiceProvider;

            var _client = provider.GetRequiredService<DiscordSocketClient>();
            _client.Log += async (LogMessage msg) => { Console.WriteLine(msg.Message); };

            _client.Ready += async () =>
            {
                Console.WriteLine("Bot ready!");
            };

            await _client.LoginAsync(TokenType.Bot, settings.BotToken);
            await _client.StartAsync();

            await Task.Delay(-1);
        }
    }
}


