using System;
using SmogBot.Bot.Helpers;
using SmogBot.Bot.Tools;

namespace SmogBot.Bot.Dialogs
{
    [Serializable]
    public class MainMenuDialog : MenuDialogDispatcher
    {
        public MainMenuDialog(MainMenuItems menuItems, string promptText = "") : base(menuItems, promptText)
        {
        }
    }
}