using System;
using SmogBot.Bot.Helpers;
using SmogBot.Bot.Tools;

namespace SmogBot.Bot.Dialogs
{
    [Serializable]
    public class NewManageNotificationsDialog : MenuActionDispatcherDialog
    {
        public NewManageNotificationsDialog(NotificationsMenuItems menuActions) : base(menuActions)
        {
        }
    }
}