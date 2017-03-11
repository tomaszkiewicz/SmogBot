using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Tomaszkiewicz.BotFramework.Extensions;

namespace SmogBot.Bot
{
    internal class SmogBot : Bot
    {
        private readonly Func<IDialog<object>> _rootDialogFactory;

        public SmogBot(Func<IDialog<object>> rootDialogFactory)
        {
            _rootDialogFactory = rootDialogFactory;
        }

        public override async Task OnMessage(Activity activity)
        {
            await Conversation.SendAsync(activity, _rootDialogFactory);
        }

        public override async Task OnConversationUpdate(IConversationUpdateActivity updateActivity)
        {
            var connector = updateActivity.CreateConnectorClient();
            var activity = ((Activity)updateActivity);

            if (updateActivity.AreMembersAdded())
            {
                await connector.Conversations.ReplyToActivityAsync(activity.CreateReply($"Czeœæ, {activity.MembersAdded[0].Name}!"));
                await connector.Conversations.ReplyToActivityAsync(activity.CreateReply("Jestem botem, który pomo¿e Ci monitorowaæ poziom zanieczyszczenia powietrza."));

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