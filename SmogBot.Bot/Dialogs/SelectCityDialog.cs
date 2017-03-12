using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using SmogBot.Bot.DatabaseAccessLayer;
using SmogBot.Bot.Helpers;
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

        private static async Task OnCitySelected(IDialogContext context, IAwaitable<string> result)
        {
            var city = await result;

            context.PrivateConversationData.SetValue(ConversationDataKeys.City, city);

            await context.PostAsync("Ok, Twoje miasto zostało zapamiętane :)\r\nW przyszłości automatycznie pokażę dane dla Twojego miasta.");

            context.Done(city);
        }
    }
}