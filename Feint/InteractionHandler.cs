
using Discord.Interactions;
using Discord.WebSocket;
using System.Reflection;

namespace Feint.Core
{
    public class InteractionHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly InteractionService _commands;
        private readonly IServiceProvider _serviceProvider;

        public InteractionHandler(DiscordSocketClient client, InteractionService commands, IServiceProvider serviceProvider)
        {
            _client = client;
            _commands = commands;
            _serviceProvider = serviceProvider; 
        }

        // someone uses an interaction like /command, menu, modal

        public async Task InitializeAsync()
        {
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _serviceProvider);
            _client.InteractionCreated += HandleInteraction;
        }

        // Handle interaction 
        private async Task HandleInteraction(SocketInteraction arg)
        {
            try
            {
                var ctx = new SocketInteractionContext(_client, arg);
                await _commands.ExecuteCommandAsync(ctx, _serviceProvider);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

    }
}
