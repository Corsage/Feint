using Discord.WebSocket;
using Microsoft.Extensions.Hosting;
using Serilog;
using Feint.Core.Models;
using Microsoft.Extensions.Options;
using Discord;

namespace Feint.Core.Services
{
    public class DiscordBotService : BackgroundService
    {
        private static readonly ILogger logger = Log.ForContext<DiscordBotService>();

        private readonly IOptionsMonitor<DiscordSettings> _config;
        private readonly DiscordSocketClient _client;

        private DiscordSettings settings => _config.CurrentValue;

        public DiscordBotService(IOptionsMonitor<DiscordSettings> config, DiscordSocketClient client)
        {
            _config = config;
            _client = client;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            logger.Information("Starting service!");
            _client.Log += async (LogMessage msg) => { logger.Information(msg.Message); };

            _client.Ready += async () =>
            {
                logger.Information("Discord bot initialized.");
            };

            await _client.LoginAsync(TokenType.Bot, settings.BotToken);
            await _client.StartAsync();


            await base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            logger.Information("Stopping service!");
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                logger.Information("EXECUTING ASYNC BODY");
                try
                {
                    logger.Information(settings.BotToken);
                    await Task.Delay(1000, stoppingToken);
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "SAD");
                }

            }

            logger.Information("Loop is finished.");
        }
    }
}
