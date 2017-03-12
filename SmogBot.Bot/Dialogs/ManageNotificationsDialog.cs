using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using SmogBot.Bot.DatabaseAccessLayer;
using Tomaszkiewicz.BotFramework.Dialogs;
using Tomaszkiewicz.BotFramework.Extensions;
using Tomaszkiewicz.BotFramework.WebApi.Dialogs;

namespace SmogBot.Bot.Dialogs
{
    [Serializable]
    public class ManageNotificationsDialog : AutoDeserializeDialog<object>
    {
        [NonSerialized]
        private readonly BotAccessor _accessor;

        private const string AddNotification = "Dodaj powiadomienie";
        private const string EndConfiguration = "Zakoñcz konfigurowanie";
        private const string DeleteConfigurationTemplate = "Usuñ powiadomienie o {0}";

        public ManageNotificationsDialog(BotAccessor accessor)
        {
            _accessor = accessor;
        }

        public override async Task StartAsync(IDialogContext context)
        {
            await ShowMenu(context);
        }

        private async Task ShowMenu(IDialogContext context)
        {
            await context.SendTypingMessage();

            var notifications = await _accessor.GetNotificationTimes(context.Activity.Conversation.Id);

            var menu = notifications.Select(notification => string.Format(DeleteConfigurationTemplate, notification)).ToList();

            menu.Add(AddNotification);
            menu.Add(EndConfiguration);

            PromptDialog.Choice(context, OnOptionSelected, menu, "", "", 1);
        }

        private async Task OnOptionSelected(IDialogContext context, IAwaitable<string> result)
        {
            var message = await result;

            switch (message)
            {
                case EndConfiguration:
                    context.Done(new object());
                    return;

                case AddNotification:
                    context.Call(new PromptTimeDialog("Podaj godzinê (np. 8:30) o której chcesz byæ informowany o ew. przekroczeniach norm."), OnAddNofiticationTimeProvided);
                    break;

                default:
                    var notifications = await _accessor.GetNotificationTimes(context.Activity.Conversation.Id);

                    foreach (var notification in notifications)
                    {
                        // not so good...
                        if (message != string.Format(DeleteConfigurationTemplate, notification))
                            continue;

                        await context.SendTypingMessage();

                        await _accessor.DeleteNotificationTime(context.Activity.Conversation.Id, notification);

                        await context.PostAsync($"Usuniêto powiadomienie o {notification}");

                        break;
                    }

                    await ShowMenu(context);
                    break;
            }
        }

        private async Task OnAddNofiticationTimeProvided(IDialogContext context, IAwaitable<DateTime> result)
        {
            var time = await result;

            await context.SendTypingMessage();

            await _accessor.AddNotificationTime(context.Activity.Conversation.Id, time.ToString("HH:mm"));
            
            await context.PostAsync($"Dodano powiadomienie o {time:HH:mm}");

            await ShowMenu(context);
        }
    }
}