using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json.Linq;
using Tomaszkiewicz.BotFramework.Extensions;
using Tomaszkiewicz.BotFramework.WebApi.Dialogs;

namespace SmogBot.Bot.Tools
{
    [Serializable]
    public class MenuDialogDispatcher : AutoDeserializeDialog<object>
    {
        private readonly string _promptText;

        [NonSerialized]
        private readonly Dictionary<string, Func<IDialog<object>>> _menuFactories;

        public MenuDialogDispatcher(Dictionary<string, Func<IDialog<object>>> menuItems, string promptText = "")
        {
            _menuFactories = menuItems;
            _promptText = promptText;
        }

        public void RegisterMenuItem(string label, Func<IDialog<object>> dialogFactory)
        {
            _menuFactories[label] = dialogFactory;
        }

        public override Task StartAsync(IDialogContext context)
        {
            context.Wait(OnSelected);

            return Task.CompletedTask;
        }

        private async Task ShowMenu(IDialogContext context)
        {
            try
            {
                var menu = context.MakeQuickReplies(_menuFactories.Keys, _promptText);

                await context.PostAsync(menu);
            }
            catch (Exception ex)
            {
                await context.PostAsync(ex.Message);
            }

            context.Wait(OnSelected);
        }

        private async Task OnSelected(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            if (!string.IsNullOrWhiteSpace(message?.Text) && _menuFactories.ContainsKey(message.Text))
            {
                var dialog = _menuFactories[message.Text]();

                context.Call(dialog, Resume);
            }
            else
                await ShowMenu(context);
        }

        private async Task Resume(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                if (result != null)
                    await result;
            }
            finally
            {
                await ShowMenu(context);
            }
        }
    }
}