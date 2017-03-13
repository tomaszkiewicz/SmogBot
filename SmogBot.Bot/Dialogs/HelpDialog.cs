using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;

namespace SmogBot.Bot.Dialogs
{
    [Serializable]
    public class HelpDialog : IDialog
    {
        public Task StartAsync(IDialogContext context)
        {
            throw new NotImplementedException();
        }
    }
}