using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration; 

namespace Feint.Core
{
    public class PrefixHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        //private readonly IConfiguration _config;

        public PrefixHandler(DiscordSocketClient client, CommandService commands)
        //, IConfigurationRoot config
        {
            _client = client;
            _commands = commands;
            //_config = config;
        }

        public async Task InitializeAsync()
        {
            _client.MessageReceived += HandleCommandAsync;
        }

        public void AddModule<T>()
        {
            _commands.AddModuleAsync<T>(null);
        }

        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            var message = messageParam as SocketUserMessage;
            if (message == null) return;

            int argPos = 0;

            // fix this after to read from .json file for config 
            if (!(message.HasCharPrefix("!"[0], ref argPos) || !message.HasMentionPrefix(_client.CurrentUser, ref argPos)) || message.Author.IsBot) return;

            var context = new SocketCommandContext(_client, message);

            await _commands.ExecuteAsync(
                context: context,
                argPos: argPos,
                services: null);
        }
    }


    







    

}
