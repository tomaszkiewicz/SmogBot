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
    public class SelectCityDialog : AutoDeserializeDialog<string>
    {
        [NonSerialized]
        private readonly BotAccessor _accessor;

        public SelectCityDialog(BotAccessor accessor)
        {
            _accessor = accessor;
        }

        public override Task StartAsync(IDialogContext context)
        {
            context.Call(new SearchDialog(
                SearchFunc,
                "Wygląda na to, że jesteś tutaj pierwszy raz i nie wybrałeś jeszcze swojego miasta. Wpisz miasto, dla którego dane chcesz otrzymywać:",
                "Niestety, nie mamy danych dla tego miasta, spróbuj wpisać jakieś inne w okolicy:",
                "Czy możesz doprecyzować, o jakie miasto Ci chodzi?",
                "Nie udało się znaleźć miasta."
                ), OnCitySelected);

            return Task.CompletedTask;
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