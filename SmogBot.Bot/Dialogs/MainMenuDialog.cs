using System;
using SmogBot.Bot.Tools;

namespace SmogBot.Bot.Dialogs
{
    [Serializable]
    public class MainMenuDialog : MenuDialogDispatcher
    {
        public MainMenuDialog(Func<MeasurementsDialog> measurementsDialogFactory, Func<NewManageNotificationsDialog> manageNotificationsDialogFactory, Func<FeedbackDialog> feedbackDialogFactory, Func<HelpDialog> helpDialogFactory)
        {
            RegisterMenuItem("SprawdŸ przekroczenia norm", measurementsDialogFactory);
            RegisterMenuItem("Zarz¹dzaj notyfikacjami", manageNotificationsDialogFactory);
            RegisterMenuItem("Wyœlij opiniê o tym bocie", feedbackDialogFactory);
            RegisterMenuItem("Pomoc", helpDialogFactory);
        }
    }
}