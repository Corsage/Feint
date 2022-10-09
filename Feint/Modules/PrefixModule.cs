using Discord;
using Discord.Commands;


namespace Feint.Core.Modules
{
    public class PrefixModule : ModuleBase<SocketCommandContext>
    {
        [Command("hello")]
        public async Task HandlePingComment()
        {
            await Context.Message.ReplyAsync("wagwan!");
        }
    }
}
