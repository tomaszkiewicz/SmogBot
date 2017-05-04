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
            Add("Stan powietrza", measurementsDialogFactory);
            Add("Powiadomienia", manageNotificationsDialogFactory);
            Add("Wyślij opinię", feedbackDialogFactory);
            Add("Pomoc", helpDialogFactory);
        }
    }
}