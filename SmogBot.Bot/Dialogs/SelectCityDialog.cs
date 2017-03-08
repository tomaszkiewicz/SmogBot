using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using SmogBot.Bot.DatabaseAccessLayer;
using SmogBot.Bot.Helpers;
using Tomaszkiewicz.BotFramework.Dialogs;

namespace SmogBot.Bot.Dialogs
{
    [Serializable]
    public class SelectCityDialog : IDialog<string>
    {
        [NonSerialized]
        private readonly BotAccessor _accessor;

        public SelectCityDialog(BotAccessor accessor)
        {
            _accessor = accessor;
        }

        public Task StartAsync(IDialogContext context)
        {
            context.Call(new SearchDialog(
                SearchFunc,
                "Wygl¹da na to, ¿e jesteœ tutaj pierwszy raz i nie wybra³eœ jeszcze swojego miasta. Wpisz miasto, dla którego dane chcesz otrzymywaæ:",
                "Niestety, nie mamy danych dla tego miasta, spróbuj wpisaæ jakieœ inne w okolicy:",
                "Czy mo¿esz doprecyzowaæ, o jakie miasto Ci chodzi?",
                "Nie uda³o siê znaleŸæ miasta."
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

            await context.PostAsync("Ok, Twoje miasto zosta³o zapamiêtane :)\r\nW przysz³oœci automatycznie poka¿ê dane dla Twojego miasta.");

            context.Done(city);
        }
    }
}