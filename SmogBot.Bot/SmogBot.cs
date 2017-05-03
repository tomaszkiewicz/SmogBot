using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.ConnectorEx;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SmogBot.Bot.DatabaseAccessLayer;
using Tomaszkiewicz.BotFramework.Extensions;

namespace SmogBot.Bot
{
    internal class SmogBot : Tomaszkiewicz.BotFramework.Bot
    {
        private readonly Func<IDialog<object>> _rootDialogFactory;
        private readonly BotAccessor _accessor;

        public SmogBot(Func<IDialog<object>> rootDialogFactory, BotAccessor accessor)
        {
            _rootDialogFactory = rootDialogFactory;
            _accessor = accessor;
        }

        public override async Task OnMessage(Activity activity)
        {
            await _accessor.UpdateLastActivityTime(activity.ChannelId, activity.From.Id, activity.From.Name, activity.Conversation.Id, JsonConvert.SerializeObject(activity.ToConversationReference()));

            await Conversation.SendAsync(activity, _rootDialogFactory);
        }

        public override async Task OnConversationUpdate(IConversationUpdateActivity updateActivity)
        {
            var connector = updateActivity.CreateConnectorClient();
            var activity = ((Activity)updateActivity);

            if (updateActivity.AreMembersAdded())
            {
                var username = activity.MembersAdded[0].Name;
                var greeting = username != "You" ? $"Cześć, {username}! :)" : "Cześć! :)";

                await connector.Conversations.ReplyToActivityAsync(activity.CreateReply(greeting));
                await connector.Conversations.ReplyToActivityAsync(activity.CreateReply("Jestem botem, który pomoże Ci monitorować poziom zanieczyszczenia powietrza. 🏭"));

                if (activity.From.Name != null)
                {
                    await _accessor.EnsureUser(activity.ChannelId, activity.From.Id, activity.From.Name, activity.Conversation.Id, JsonConvert.SerializeObject(activity.ToConversationReference()));

                    await OnMessage(activity);
                }
            }
        }

        public override async Task OnEvent(IEventActivity activity)
        {
            var reply = JsonConvert.DeserializeObject<Activity>(((JObject)activity.Value).GetValue("Message").ToString());

            var connector = reply.CreateConnectorClient();

            await connector.Conversations.SendToConversationAsync(reply);
        }
    }
}