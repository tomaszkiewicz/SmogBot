using System;
using System.Threading.Tasks;
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
            await _accessor.UpdateLastActivityTime(activity?.ChannelId ?? "No ChannelId", activity?.From?.Id ?? "No FromId", activity?.From?.Name ?? "No FromName", activity?.Conversation?.Id ?? "No ConversationId");

            await Conversation.SendAsync(activity, _rootDialogFactory);
        }

        public override async Task OnConversationUpdate(IConversationUpdateActivity updateActivity)
        {
            var connector = updateActivity.CreateConnectorClient();
            var activity = ((Activity)updateActivity);

            if (updateActivity.AreMembersAdded())
            {
                await connector.Conversations.ReplyToActivityAsync(activity.CreateReply($"Cześć, {activity.MembersAdded[0].Name}!"));
                await connector.Conversations.ReplyToActivityAsync(activity.CreateReply("Jestem botem, który pomoże Ci monitorować poziom zanieczyszczenia powietrza."));

                await _accessor.EnsureUser(activity?.ChannelId ?? "No ChannelId", activity?.From?.Id ?? "No FromId", activity?.From?.Name ?? "No FromName", activity?.Conversation?.Id ?? "No ConversationId");

                await OnMessage(activity);
            }
        }

        public override async Task OnTrigger(ITriggerActivity activity)
        {
            var message = JsonConvert.DeserializeObject<Message>(((JObject)activity.Value).GetValue("Message").ToString());
            var messageActivity = message.ResumptionCookie.GetMessage();

            var client = new ConnectorClient(new Uri(messageActivity.ServiceUrl));

            var triggerReply = messageActivity.CreateReply();

            triggerReply.Text = $"This is coming back from the trigger! {message.Text}";

            await client.Conversations.ReplyToActivityAsync(triggerReply);
        }
    }
}