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
            await context.PostAsync("Moim zadaniem jest pomaganie Ci przetrwać w zasmogowanych polskich miastach :)");
            await context.SendTypingMessage();
            await Task.Delay(3000);
            await context.PostAsync("Do tego celu jestem wyposażony w kilka narzędzi:");
            await context.SendTypingMessage();
            await Task.Delay(3000);
            await context.PostAsync("Mogę na żądanie sprawdzić poziom zanieczyszczeń w danym mieście - wystarczy, że klikniesz opcję 'Sprawdź przekroczenia norm' w menu głównym.");
            await Task.Delay(5000);
            await context.SendTypingMessage();
            await Task.Delay(4000);
            await context.PostAsync("Jeśli nie masz czasu na regularne klikanie - nie ma sprawy, mogę działąć także pasywnie, powiadamiając Cię o przekroczeniach, gdy tylko wystąpią, jak również o zadanych przez Ciebie godzinach :)");
            await Task.Delay(5000);
            await context.SendTypingMessage();
            await Task.Delay(4000);
            await context.PostAsync("Ustawisz to klikając opcję 'Powiadomienia i ostrzeżenia' w menu głównym.");
            await Task.Delay(3000);
            
            await context.PostAsync($"[A moja wersja to: {Assembly.GetExecutingAssembly().GetName().Version} - to tak dla ułatwienia diagnostyki :)]");

            context.Done(new object());
        }
    }
}