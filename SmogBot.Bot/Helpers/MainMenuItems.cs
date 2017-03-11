using System;
using System.Collections.Generic;
using Microsoft.Bot.Builder.Dialogs;
using SmogBot.Bot.Dialogs;

namespace SmogBot.Bot.Helpers
{
    public class MainMenuItems : Dictionary<string, Func<IDialog<object>>>
    {
        public MainMenuItems(Func<MeasurementsDialog> measurementsDialogFactory, Func<NewManageNotificationsDialog> manageNotificationsDialogFactory, Func<FeedbackDialog> feedbackDialogFactory, Func<HelpDialog> helpDialogFactory
            )
        {
            Add("SprawdŸ przekroczenia norm", measurementsDialogFactory);
            Add("Zarz¹dzaj notyfikacjami", manageNotificationsDialogFactory);
            Add("Wyœlij opiniê o tym bocie", feedbackDialogFactory);
            Add("Pomoc", helpDialogFactory);
        }
    }
}