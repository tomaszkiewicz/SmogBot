using System;
using System.Collections.Generic;
using Microsoft.Bot.Builder.Dialogs;
using SmogBot.Bot.Dialogs;

namespace SmogBot.Bot.Helpers
{
    public class MainMenuItems : Dictionary<string, Func<IDialog<object>>>
    {
        public MainMenuItems(Func<MeasurementsDialog> measurementsDialogFactory, Func<ManageNotificationsDialog> manageNotificationsDialogFactory, Func<FeedbackDialog> feedbackDialogFactory, Func<HelpDialog> helpDialogFactory
            )
        {
            Add("Sprawdź przekroczenia norm", measurementsDialogFactory);
            Add("Powiadomienia i ostrzeżenia", manageNotificationsDialogFactory);
            Add("Wyślij opinię o tym bocie", feedbackDialogFactory);
            Add("Pomoc", helpDialogFactory);
        }
    }
}