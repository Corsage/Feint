
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feint.Core.Models
{
    public class DiscordSettings
    {
        public string BotToken { get; set; }
        public string BotPrefix { get; set; }

        public string TestGuild { get; set; }
    }
}
