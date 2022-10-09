using Discord.Interactions;


namespace Feint.Core.Modules
{
    public class InteractionModule: InteractionModuleBase<SocketInteractionContext>
    {
       [SlashCommand("hello", "Receive a pinged response")]
       public async Task HandlePingCommand()
        {
            await RespondAsync("wagwan!");
        }

    }
}
