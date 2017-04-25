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
            await context.PostAsync("Moim zadaniem jest pomaganie Ci przetrwa� w zasmogowanych polskich miastach :)");
            await context.SendTypingMessage();
            await Task.Delay(3000);
            await context.PostAsync("Do tego celu jestem wyposa�ony w kilka narz�dzi:");
            await context.SendTypingMessage();
            await Task.Delay(3000);
            await context.PostAsync("Mog� na ��danie sprawdzi� poziom zanieczyszcze� w danym mie�cie - wystarczy, �e klikniesz opcj� 'Sprawd� przekroczenia norm' w menu g��wnym.");
            await Task.Delay(5000);
            await context.SendTypingMessage();
            await Task.Delay(4000);
            await context.PostAsync("Je�li nie masz czasu na regularne klikanie - nie ma sprawy, mog� dzia��� tak�e pasywnie, powiadamiaj�c Ci� o przekroczeniach, gdy tylko wyst�pi�, jak r�wnie� o zadanych przez Ciebie godzinach :)");
            await Task.Delay(5000);
            await context.SendTypingMessage();
            await Task.Delay(4000);
            await context.PostAsync("Ustawisz to klikaj�c opcj� 'Powiadomienia i ostrze�enia' w menu g��wnym.");
            await Task.Delay(3000);
            
            await context.PostAsync($"[A moja wersja to: {Assembly.GetExecutingAssembly().GetName().Version} - to tak dla u�atwienia diagnostyki :)]");

            context.Done(new object());
        }
    }
}