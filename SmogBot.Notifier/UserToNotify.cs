using System;

namespace SmogBot.Notifier
{
    public class UserToNotify
    {
        public string ChannelId { get; set; }
        public string FromId { get; set; }
        public string FromName { get; set; }
        public string ConversationId { get; set; }
        public string ConversationReference { get; set; }
        public string CityName { get; set; }
        public DateTime Time { get; set; }
    }
}