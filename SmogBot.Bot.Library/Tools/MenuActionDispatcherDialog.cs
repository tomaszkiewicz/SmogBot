using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Tomaszkiewicz.BotFramework.Extensions;

namespace SmogBot.Bot.Tools
{
    [Serializable]
    public class MenuActionDispatcherDialog : IDialog<object>
    {
        private readonly Dictionary<string, Func<IDialogContext, Task>> _menuActions = new Dictionary<string, Func<IDialogContext, Task>>();

        public void RegisterMenuItem(string label, Func<IDialogContext, Task> action)
        {
            _menuActions[label] = action;
        }

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            if (message.Text == null)
                await ShowMenu(context);
        }

        protected Task ShowMenu(IDialogContext context)
        {
            var menu = context.MakeQuickReplies(_menuActions.Keys);

            context.PostAsync(menu);

            context.Wait(OnSelected);

            return Task.CompletedTask;
        }

        private async Task OnSelected(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            if (_menuActions.ContainsKey(message.Text))
            {
                await _menuActions[message.Text](context);
            }
            else
            {
                await ShowMenu(context);
            }
        }
    }
}