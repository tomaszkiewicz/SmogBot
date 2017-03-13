using Microsoft.Bot.Connector;

namespace SmogBot.Bot
{
    public static class ActivityExtensions
    {
        public static ConversationReference GetConversationReference(this Activity activity)
        {
            return new ConversationReference(activity.Id, activity.From, activity.Recipient, activity.Conversation, activity.ChannelId, activity.ServiceUrl);
        }
    }
}