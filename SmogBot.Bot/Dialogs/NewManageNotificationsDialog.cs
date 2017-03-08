using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using SmogBot.Bot.Tools;
using Tomaszkiewicz.BotFramework.Dialogs;
using Tomaszkiewicz.BotFramework.Extensions;

namespace SmogBot.Bot.Dialogs
{
    [Serializable]
    public class NewManageNotificationsDialog : MenuActionDispatcherDialog
    {
        public NewManageNotificationsDialog()
        {
            RegisterMenuItem("Dodaj powiadomienie", AddNotification);

            var notificationTimes = new List<string>();

            notificationTimes.Add("8:30");
            notificationTimes.Add("10:30");

            foreach (var time in notificationTimes)
            {
                RegisterMenuItem(time, async ctx => await DeleteNotification(ctx, time));
            }

        }

        private async Task DeleteNotification(IDialogContext context, string time)
        {
            await context.PostAsync($"Usuniêto powiadomienie o {time}");

            await ShowMenu(context);
        }

        private Task AddNotification(IDialogContext context)
        {
            context.Call(new PromptTimeDialog("Podaj godzinê (np. 8:30) o której chcesz byæ informowany o ew. przekroczeniach norm."), OnAddNofiticationTimeProvided);

            return Task.CompletedTask;
        }

        private async Task OnAddNofiticationTimeProvided(IDialogContext context, IAwaitable<DateTime> result)
        {
            var time = await result;

            await context.SendTypingMessage();

            //await Accessor.AddNotificationTime(_conversationId, time.ToString("HH:mm"));

            await context.PostAsync($"Dodano powiadomienie o {time:HH:mm}");

            await ShowMenu(context);
        }
    }
}