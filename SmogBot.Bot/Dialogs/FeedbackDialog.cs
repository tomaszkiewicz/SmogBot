using System;
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

        private string _feedback = "";

        public FeedbackDialog(BotAccessor accessor)
        {
            _accessor = accessor;
        }

        public override async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Napisz swoją opinię, gdy skończysz wpisz 'koniec' w osobnej wiadomości.");

            context.Wait(MessageReceivedAsync);
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            if (message != null && message.Text.ToLower() == "koniec")
            {
                _feedback = _feedback.Trim();

                if (string.IsNullOrWhiteSpace(_feedback))
                {
                    await context.PostAsync("Nie podałeś/aś swojej opinii, no trudno :(");

                    context.Done("");

                    return;
                }

                await context.PostAsync($"Twoja opinia:\r\n{_feedback}");

                PromptDialog.Confirm(context, SendFeedback, "Wysłać opinię?");

                return;
            }

            if (message?.Text != null)
                _feedback += message.Text + "\r\n";

            context.Wait(MessageReceivedAsync);
        }

        private async Task SendFeedback(IDialogContext context, IAwaitable<bool> result)
        {
            var send = await result;

            if (send)
            {
                await _accessor.SendFeedback(context, _feedback);

                await context.PostAsync("Opinia wysłana. Dziękuję :)");
            }

            context.Done(_feedback);
        }
    }
}