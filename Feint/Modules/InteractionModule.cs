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

        [SlashCommand("water", "Set the time you'd like to remind the Hydro Homies to drink water.")]
        public async Task setWaterReminderTime(string time, string timezone)
        {
            // Declare message to set 
            await Context.Channel.SendMessageAsync("Time Set");

            // get time and convert to UTC

            DateTime convertedTime = DateTime.Parse(time);
            TimeZoneInfo setTimeZone;

            switch (timezone.ToLower())
            {
                case "est":
                    setTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                    break;

                case "pst":
                    setTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
                    break;

                default:
                    setTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                    break;
            }
                
            DateTime dateUTC = TimeZoneInfo.ConvertTimeToUtc(convertedTime, setTimeZone);

            // message list
            var messages = new string[]
            {
                "0.5 L: Starting off tier",
                "1.0 L: Getting there but still slacking",
                "1.5 L: Decenttttt 👀",
                "2.0 L: Solidddd",
                "2.5 L: 💯",
                "3.0 L: Water bender",
                "3.5L+: Literally drowning"
            };

            // Loop that runs and checks the time 
            while (true)
            {
                var now = DateTime.UtcNow.TimeOfDay;

                if (now.Hours == dateUTC.Hour && now.Minutes == dateUTC.Minute)
                {
                    await Context.Channel.SendMessageAsync("Wagwan <@&1072239338089353216>");
                    foreach (var message in messages)
                    {
                        await Context.Channel.SendMessageAsync(message);
                    }
                } 
                await Task.Delay(TimeSpan.FromMinutes(1));
            }
        }
    }
}
