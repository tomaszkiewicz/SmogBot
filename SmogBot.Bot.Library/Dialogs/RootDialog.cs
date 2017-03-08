using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using SmogBot.Bot.DatabaseAccessLayer;
using SmogBot.Bot.Helpers;

namespace SmogBot.Bot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        [NonSerialized]
        private readonly BotAccessor _accessor;

        [NonSerialized]
        private readonly Func<MeasurementsDialog> _measurementsDialogFactory;

        [NonSerialized]
        private readonly Func<ManageNotificationsDialog> _manageNotificationsDialogFactory;

        private bool _conversationDataSaved;
        private const string CheckMeasurements = "Sprawdź przekroczenia norm";
        private const string ManageNotifications = "Zarządzaj notyfikacjami";
        private const string GiveFeedback = "Wyślij opinię o tym bocie";
        private const string Help = "Pomoc";

        public RootDialog(BotAccessor accessor, Func<MeasurementsDialog> measurementsDialogFactory, Func<ManageNotificationsDialog> manageNotificationsDialogFactory)
        {
            _accessor = accessor;
            _measurementsDialogFactory = measurementsDialogFactory;
            _manageNotificationsDialogFactory = manageNotificationsDialogFactory;
        }

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            if (!_conversationDataSaved)
            {
                context.PrivateConversationData.SetValue(ConversationDataKeys.ChannelId, message.ChannelId);
                context.PrivateConversationData.SetValue(ConversationDataKeys.FromId, message.From.Id);
                context.PrivateConversationData.SetValue(ConversationDataKeys.FromName, message.From.Name);
                context.PrivateConversationData.SetValue(ConversationDataKeys.ConversationId, message.Conversation.Id);

                await _accessor.EnsureUser(message.ChannelId, message.From.Id, message.From.Name, message.Conversation.Id);

                _conversationDataSaved = true;
            }

            if (message.Text == null)
                ShowMenu(context);
        }

        private void ShowMenu(IDialogContext context)
        {
            PromptDialog.Choice(context, OnOptionSelected, new List<string>
            {
                CheckMeasurements,
                ManageNotifications,
                GiveFeedback,
                Help
            }, "", "", 1);
        }

        private async Task OnOptionSelected(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                var selected = await result;

                switch (selected)
                {
                    case CheckMeasurements:
                        context.Call(_measurementsDialogFactory(), Resume);
                        break;

                    case ManageNotifications:
                        context.Call(_manageNotificationsDialogFactory(), Resume);
                        break;

                    case GiveFeedback:
                        context.Call(new FeedbackDialog(), Resume);
                        break;

                    case Help:
                        context.Call(new HelpDialog(), Resume);
                        break;

                    default:
                        await context.PostAsync($"Nieprawidłowa opcja : {selected}");

                        ShowMenu(context);
                        break;
                }
            }
            catch (TooManyAttemptsException)
            {
                //await context.PostAsync($"Ooops! Too many attemps :(. But don't worry, I'm handling that exception and you can try again!");
                //context.Wait(MessageReceivedAsync);

                ShowMenu(context);
            }
        }

        private async Task Resume(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                if (result != null)
                    await result;
            }
            catch (Exception ex)
            {
                await context.PostAsync($"Wystąpił problem z przetwarzaniem ostatniej wiadomości: {ex.Message}");
            }
            finally
            {
                ShowMenu(context);
            }
        }
    }
}