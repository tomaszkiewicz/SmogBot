using System;
using System.Collections.Generic;
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

        [NonSerialized]
        private readonly Func<SelectCityDialog> _selectCityDialogFactory;

        private const string AddNotification = "Dodaj powiadomienie";
        private const string EndConfiguration = "Menu główne";
        private const string EnableWarnings = "Włącz ostrzeżenia";
        private const string DisableWarnings = "Wyłącz ostrzeżenia";
        private const string DeleteConfigurationTemplate = "Usuń powiadomienie o {0}";

        public ManageNotificationsDialog(BotAccessor accessor, Func<SelectCityDialog> selectCityDialogFactory)
        {
            _accessor = accessor;
            _selectCityDialogFactory = selectCityDialogFactory;
        }

        public override async Task StartAsync(IDialogContext context)
        {
            var city = await _accessor.GetUserCity(context.Activity);

            if (string.IsNullOrWhiteSpace(city))
                context.Call(_selectCityDialogFactory(), OnCitySelected);
            else
                await ShowMenu(context);
        }

        private async Task ShowMenu(IDialogContext context)
        {
            await context.SendTypingMessage();

            var menu = new List<string>
            {
                EndConfiguration
            };

            // and warnings menu items

            var warningsEnabled = await _accessor.GetWarningsStatus(context.Activity);

            menu.Add(warningsEnabled ? DisableWarnings : EnableWarnings);

            // add notifications menu items

            menu.Add(AddNotification);

            var notifications = await _accessor.GetNotificationTimes(context.Activity.Conversation.Id);

            menu.AddRange(notifications.Select(notification => string.Format(DeleteConfigurationTemplate, notification)));

            // show menu

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
                    ShowPromptTimeDialog(context);
                    break;

                case EnableWarnings:
                    await CallEnableWarnings(context);
                    break;

                case DisableWarnings:
                    await CallDisableWarnings(context);
                    break;

                default:
                    await DeleteNotification(context, message);
                    break;
            }
        }

        private async Task CallDisableWarnings(IDialogContext context)
        {
            await _accessor.DisableWarnings(context.Activity);

            await context.PostAsync("Ok, wyłączyłem ostrzeżenia - od teraz _nie będziesz_ otrzymywać automatyczych ostrzeżeń o zmianie stanu powietrza.");
            await context.PostAsync("Jeśli zechcesz ponownie je włączyć, daj znać używając stosownej opcji w menu :)");

            await ShowMenu(context);
        }

        private async Task CallEnableWarnings(IDialogContext context)
        {
            await _accessor.EnableWarnings(context.Activity);

            await context.PostAsync("Ok, włączyłem ostrzeżenia - od teraz będę Ci wysyłać powiadomienie za każdym razem, gdy zmieni się stan powietrza, a ściślej mówiąc - poziom AQI (Air Quality Index) dla dowolnego z zanieczyszczeń na dowolnej stacji w Twoim mieście :)");
            await context.PostAsync("Oczywiście możesz ten mechanizm wyłączyć w dowolnym momencie :)");

            await ShowMenu(context);
        }

        private async Task DeleteNotification(IDialogContext context, string message)
        {
            var notifications = await _accessor.GetNotificationTimes(context.Activity.Conversation.Id);

            foreach (var notification in notifications)
            {
                // ugly...
                if (message != string.Format(DeleteConfigurationTemplate, notification))
                    continue;

                await context.SendTypingMessage();

                await _accessor.DeleteNotificationTime(context.Activity.Conversation.Id, notification);

                await context.PostAsync($"Ok, _nie będę_ Cię już powiadamiać o godzinie {notification}");

                break;
            }

            await ShowMenu(context);
        }
        
        private async Task OnCitySelected(IDialogContext context, IAwaitable<string> result)
        {
            await result;

            await ShowMenu(context);
        }

        private void ShowPromptTimeDialog(IDialogContext context)
        {
            context.Call(new PromptTimeDialog("Podaj godzinę (np. 8:30) o której chcesz być informowany o ew. przekroczeniach norm."), OnAddNofiticationTimeProvided);
        }

        private async Task OnAddNofiticationTimeProvided(IDialogContext context, IAwaitable<DateTime> result)
        {
            var time = await result;

            await context.SendTypingMessage();

            await _accessor.AddNotificationTime(context.Activity.Conversation.Id, time.ToString("HH:mm"));

            await context.PostAsync($"Przyjąłem, od teraz będę Cię powiadamiać codziennie o godzinie {time:HH:mm}, jeśli tylko normy powietrza będą przekroczone w Twoim mieście :)");

            await ShowMenu(context);
        }
    }
}