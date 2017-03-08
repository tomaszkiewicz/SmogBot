using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using SmogBot.Bot.DatabaseAccessLayer;
using SmogBot.Bot.Helpers;
using Tomaszkiewicz.BotFramework.Dialogs;
using Tomaszkiewicz.BotFramework.Extensions;

namespace SmogBot.Bot.Dialogs
{
    [Serializable]
    public class ManageNotificationsDialog : IDialog<object>
    {
        [NonSerialized]
        private readonly BotAccessor _accessor;

        private string _conversationId;

        private const string AddNotification = "Dodaj powiadomienie";
        private const string EndConfiguration = "Zakoñcz konfigurowanie";

        public ManageNotificationsDialog(BotAccessor accessor)
        {
            _accessor = accessor;
        }

        public async Task StartAsync(IDialogContext context)
        {
            context.PrivateConversationData.TryGetValue(ConversationDataKeys.ConversationId, out _conversationId);

            await ShowMenu(context);
        }

        private async Task ShowMenu(IDialogContext context)
        {
            await context.SendTypingMessage();

            var notifications = await _accessor.GetNotificationTimes(_conversationId);

            var menu = notifications.Select(notification => $"Usuñ powiadomienie o {notification}").ToList();

            menu.Add(AddNotification);
            menu.Add(EndConfiguration);

            PromptDialog.Choice(context, OnOptionSelected, menu, "", "", 1);
        }

        private async Task OnOptionSelected(IDialogContext context, IAwaitable<string> result)
        {
            var message = await result;

            if (message == EndConfiguration)
            {
                context.Done(new object());
                return;
            }

            if (message == AddNotification)
            {
                context.Call(new PromptTimeDialog("Podaj godzinê (np. 8:30) o której chcesz byæ informowany o ew. przekroczeniach norm."), OnAddNofiticationTimeProvided);
            }
            else
            {
                var notifications = await _accessor.GetNotificationTimes(_conversationId);

                foreach (var notification in notifications)
                {
                    if (message == $"Usuñ powiadomienie o {notification}")
                    {
                        await context.SendTypingMessage();

                        await _accessor.DeleteNotificationTime(_conversationId, notification);

                        await context.PostAsync($"Usuniêto powiadomienie o {notification}");

                        break;
                    }
                }

                await ShowMenu(context);
            }
        }

        private async Task OnAddNofiticationTimeProvided(IDialogContext context, IAwaitable<DateTime> result)
        {
            var time = await result;

            await context.SendTypingMessage();

            await _accessor.AddNotificationTime(_conversationId, time.ToString("HH:mm"));

            await context.PostAsync($"Dodano powiadomienie o {time:HH:mm}");

            await ShowMenu(context);
        }
    }
}