using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Tomaszkiewicz.BotFramework.Extensions;

namespace SmogBot.Bot.Dialogs
{
    [Serializable]
    public class HelpDialog : IDialog
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Moim zadaniem jest pomaganie Ci przetrwaæ w zasmogowanych polskich miastach :)");
            await context.SendTypingMessage();
            await Task.Delay(3000);
            await context.PostAsync("Do tego celu jestem wyposa¿ony w kilka narzêdzi:");
            await context.SendTypingMessage();
            await Task.Delay(3000);
            await context.PostAsync("Mogê na ¿¹danie sprawdziæ poziom zanieczyszczeñ w danym mieœcie - wystarczy, ¿e klikniesz opcjê 'SprawdŸ przekroczenia norm' w menu g³ównym.");
            await Task.Delay(5000);
            await context.SendTypingMessage();
            await Task.Delay(4000);
            await context.PostAsync("Jeœli nie masz czasu na regularne klikanie - nie ma sprawy, mogê dzia³¹æ tak¿e pasywnie, powiadamiaj¹c Ciê o przekroczeniach, gdy tylko wyst¹pi¹, jak równie¿ o zadanych przez Ciebie godzinach :)");
            await Task.Delay(5000);
            await context.SendTypingMessage();
            await Task.Delay(4000);
            await context.PostAsync("Ustawisz to klikaj¹c opcjê 'Powiadomienia i ostrze¿enia' w menu g³ównym.");
            await Task.Delay(3000);
            
            await context.PostAsync($"[A moja wersja to: {Assembly.GetExecutingAssembly().GetName().Version} - to tak dla u³atwienia diagnostyki :)]");

            context.Done(new object());
        }
    }
}