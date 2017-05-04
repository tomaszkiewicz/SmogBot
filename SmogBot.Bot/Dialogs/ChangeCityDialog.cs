using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using SmogBot.Bot.DatabaseAccessLayer;
using Tomaszkiewicz.BotFramework.Dialogs;
using Tomaszkiewicz.BotFramework.WebApi.Dialogs;

namespace SmogBot.Bot.Dialogs
{
    [Serializable]
    public class ChangeCityDialog : AutoDeserializeDialog<string>
    {
        [NonSerialized]
        private readonly BotAccessor _accessor;

        public ChangeCityDialog(BotAccessor accessor)
        {
            _accessor = accessor;
        }

        public override async Task StartAsync(IDialogContext context)
        {
            var city = await _accessor.GetUserCity(context.Activity);

            context.Call(new YesNoDialog(
                $"Twoje obecnie wybrane miasto to {city} - czy na pewno chcesz je zmienić?",
                "Tak, zmień",
                "Nie, nie zmieniaj",
                "Niestety nie zrozumiałem, odpowiedz jeszcze raz."
                ), OnConfirmation);
        }

        private async Task OnConfirmation(IDialogContext context, IAwaitable<bool> result)
        {
            var confirmation = await result;

            if (!confirmation)
            {
                context.Done(new object());
                return;
            }

            context.Call(new SearchDialog(
                SearchFunc,
                "Wpisz nowe miasto, dla którego dane chcesz otrzymywać:",
                "Niestety, nie mamy danych dla tego miasta, spróbuj wpisać jakieś inne w okolicy:",
                "Czy możesz doprecyzować, o jakie miasto Ci chodzi?",
                "Nie udało się znaleźć miasta."
                ), OnCitySelected);
        }

        private Task<IEnumerable<string>> SearchFunc(string query)
        {
            return _accessor.SearchCity(query);
        }

        private async Task OnCitySelected(IDialogContext context, IAwaitable<string> result)
        {
            var city = await result;

            await _accessor.SetUserCity(context.Activity, city);

            await context.PostAsync($"Ok, {city} to teraz Twoje domyślne miasto :)\r\nW przyszłości automatycznie pokażę dla niego dane i powiadomienia.");

            context.Done(city);
        }
    }
}