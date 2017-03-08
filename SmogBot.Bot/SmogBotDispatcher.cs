using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SmogBot.Bot.Dialogs;

namespace SmogBot.Bot
{
    internal class SmogBotDispatcher : ActivityDispatcher
    {
        private readonly Func<BasicProactiveEchoDialog> _rootDialogFactory;

        public SmogBotDispatcher(Func<BasicProactiveEchoDialog> rootDialogFactory)
        {
            _rootDialogFactory = rootDialogFactory;
        }

        public override async Task OnMessage(Activity activity)
        {
            await Conversation.SendAsync(activity, () => new BasicProactiveEchoDialog(new SampleDependency()));
        }

        public override async Task OnConversationUpdate(IConversationUpdateActivity activity)
        {
            var client = new ConnectorClient(new Uri(activity.ServiceUrl));

            if (activity.MembersAdded.Any())
            {
                var reply = ((Activity)activity).CreateReply();

                var newMembers = activity.MembersAdded?.Where(t => t.Id != activity.Recipient.Id);

                if (newMembers != null)
                    foreach (var newMember in newMembers)
                    {
                        reply.Text = "Welcome";

                        if (!string.IsNullOrEmpty(newMember.Name))
                            reply.Text += $" {newMember.Name}";

                        reply.Text += " from compiled function!";

                        await client.Conversations.ReplyToActivityAsync(reply);
                    }
            }
        }

        public override async Task OnTrigger(ITriggerActivity activity)
        {
            // handle proactive Message from function
            //log.Info("Trigger start");

            var message = JsonConvert.DeserializeObject<Message>(((JObject)activity.Value).GetValue("Message").ToString());
            var messageActivity = message.ResumptionCookie.GetMessage();

            var client = new ConnectorClient(new Uri(messageActivity.ServiceUrl));

            var triggerReply = messageActivity.CreateReply();

            triggerReply.Text = $"This is coming back from the trigger! {message.Text}";

            await client.Conversations.ReplyToActivityAsync(triggerReply);

            //log.Info("Trigger end");
        }
    }
}