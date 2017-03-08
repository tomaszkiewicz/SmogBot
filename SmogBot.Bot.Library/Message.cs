using Microsoft.Bot.Builder.Dialogs;

namespace SmogBot.Bot
{
    public class Message
    {
        public ResumptionCookie ResumptionCookie { get; set; }
        public string Text { get; set; }
    }
}