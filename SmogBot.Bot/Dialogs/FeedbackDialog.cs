using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using SmogBot.Bot.DatabaseAccessLayer;
using Tomaszkiewicz.BotFramework.WebApi.Dialogs;

namespace SmogBot.Bot.Dialogs
{
    [Serializable]
    public class FeedbackDialog : AutoDeserializeDialog<string>
    {
        [NonSerialized]
        private readonly BotAccessor _accessor;

        public FeedbackDialog(BotAccessor accessor)
        {
            _accessor = accessor;
        }

        public override async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Napisz proszę swoją opinię o mnie :)");

            context.Wait(MessageReceivedAsync);
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            var feedback = message.Text.Trim();

            if (string.IsNullOrWhiteSpace(feedback))
            {
                await context.PostAsync("Nie podałeś/aś swojej opinii, no trudno :(");

                context.Done("");

                return;
            }

            await _accessor.SendFeedback(context, feedback, Assembly.GetExecutingAssembly().GetName().Version.ToString());

            await context.PostAsync("Opinia wysłana. Dziękuję :)");

            context.Done(feedback);

        }
    }
}