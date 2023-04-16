using Discord.WebSocket;
using Microsoft.Extensions.Hosting;
using Serilog;
using Feint.Core.Models;
using Microsoft.Extensions.Options;
using Discord;
using Discord.Interactions;
using Feint.Core.Modules;

namespace Feint.Core.Services
{
    public class DiscordBotService : BackgroundService
    {
        private static readonly ILogger logger = Log.ForContext<DiscordBotService>();

        private readonly IOptionsMonitor<DiscordSettings> _config;
        private readonly DiscordSocketClient _client;

        // other singletons in the singleton service
        private readonly InteractionService _slashCommands;
        private readonly PrefixHandler _prefixCommands;
        private readonly InteractionHandler _handler;

        private DiscordSettings settings => _config.CurrentValue;

        public DiscordBotService(IOptionsMonitor<DiscordSettings> config, DiscordSocketClient client, InteractionService slashCommands, InteractionHandler handler, PrefixHandler prefixCommands)
        {
            _config = config;
            _client = client;

            // other singletons added  
            _slashCommands = slashCommands;
            _prefixCommands = prefixCommands;
            _handler = handler;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            // access our slash commands (interaction version of add module line below)
            await _handler.InitializeAsync();
            _prefixCommands.AddModule<PrefixModule>();
            // subscribes to message receieved event 
            await _prefixCommands.InitializeAsync();

            // log events
            _client.Log += async (LogMessage msg) => { Console.WriteLine(msg.Message); };
            _slashCommands.Log += async (LogMessage msg) => { Console.WriteLine(msg.Message); };


            logger.Information("Starting service!");
            _client.Log += async (LogMessage msg) => { logger.Information(msg.Message); };

            _client.Ready += async () =>
            {
                logger.Information("Discord bot initialized.");
                // testGuild id?
                await _slashCommands.RegisterCommandsToGuildAsync(UInt64.Parse(settings.TestGuild));
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
                //logger.Information("EXECUTING ASYNC BODY");
                try
                {
                    //logger.Information(settings.BotToken);
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
