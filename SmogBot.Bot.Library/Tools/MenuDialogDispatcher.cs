using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Tomaszkiewicz.BotFramework.Extensions;

namespace SmogBot.Bot.Tools
{
    [Serializable]
    public class MenuDialogDispatcher : IDialog<object>
    {
        private readonly string _promptText;

        [NonSerialized]
        private readonly Dictionary<string, Func<IDialog<object>>> _menuFactories = new Dictionary<string, Func<IDialog<object>>>();

        public MenuDialogDispatcher(string promptText = "")
        {
            _promptText = promptText;
        }

        public void RegisterMenuItem(string label, Func<IDialog<object>> dialogFactory)
        {
            _menuFactories[label] = dialogFactory;
        }

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(OnSelected);

            return Task.CompletedTask;
        }

        private void ShowMenu(IDialogContext context)
        {
            var menu = context.MakeQuickReplies(_menuFactories.Keys, _promptText);

            context.PostAsync(menu);

            context.Wait(OnSelected);
        }

        private async Task OnSelected(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            if (_menuFactories.ContainsKey(message.Text))
            {
                var dialog = _menuFactories[message.Text]();

                context.Call(dialog, Resume);
            }
            else
            {
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
                await context.PostAsync($"Wyst¹pi³ problem z przetwarzaniem ostatniej wiadomoœci: {ex.Message}");
            }
            finally
            {
                ShowMenu(context);
            }
        }
    }
}