using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;

namespace SmogBot.Bot.Dialogs
{
    [Serializable]
    public class HelpDialog : IDialog
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync($"Wersja bota: {Assembly.GetExecutingAssembly().GetName().Version}");

            context.Done(new object());
        }
    }
}