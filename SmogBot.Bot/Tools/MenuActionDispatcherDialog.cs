using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Tomaszkiewicz.BotFramework.Extensions;
using Tomaszkiewicz.BotFramework.WebApi.Dialogs;

namespace SmogBot.Bot.Tools
{
    [Serializable]
    public class MenuActionDispatcherDialog : AutoDeserializeDialog<object>
    {
        [NonSerialized]
        private readonly Dictionary<string, Func<IDialogContext, ResumeAfter<object>, Task>> _menuActions;

        public MenuActionDispatcherDialog(Dictionary<string, Func<IDialogContext, ResumeAfter<object>, Task>> menuActions)
        {
            _menuActions = menuActions;
        }

        public override async Task StartAsync(IDialogContext context)
        {
            var menu = context.MakeQuickReplies(_menuActions.Keys);

            await context.PostAsync(menu);

            context.Wait(OnSelected);
        }

        private async Task OnSelected(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            if (!string.IsNullOrWhiteSpace(message?.Text) && _menuActions.ContainsKey(message.Text))
                await _menuActions[message.Text](context, Resume);
            else
                await StartAsync(context);
        }

        private async Task Resume(IDialogContext context, IAwaitable<object> result)
        {
            await StartAsync(context);
        }
    }
}