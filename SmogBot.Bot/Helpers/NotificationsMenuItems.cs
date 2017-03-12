using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using SmogBot.Bot.DatabaseAccessLayer;
using Tomaszkiewicz.BotFramework.Dialogs;
using Tomaszkiewicz.BotFramework.Extensions;

namespace SmogBot.Bot.Helpers
{
    public class NotificationsMenuItems : Dictionary<string, Func<IDialogContext, ResumeAfter<object>, Task>>
    {
        private readonly BotAccessor _accessor;
        private readonly List<string> _notificationTimes;

        public NotificationsMenuItems(BotAccessor accessor)
        {
            _accessor = accessor;

            Add("Dodaj powiadomienie", AddNotification);

            _notificationTimes = new List<string>
            {
                "8:30",
                "10:30"
            };

            foreach (var time in _notificationTimes)
                Add($"Usuñ powiadomienie o {time}", async (ctx, resume) => await DeleteNotification(ctx, resume, time));

            Add("Menu g³ówne", Exit);

        }

        private Task Exit(IDialogContext context, ResumeAfter<object> resume)
        {
            context.Done("");

            return Task.CompletedTask;
        }

        private async Task DeleteNotification(IDialogContext context, ResumeAfter<object> resume, string time)
        {
            _notificationTimes.Remove(time);

            await context.PostAsync($"Usuniêto powiadomienie o {time}");

            context.Wait(resume);
        }

        private Task AddNotification(IDialogContext context, ResumeAfter<object> resume)
        {
            context.Call(new PromptTimeDialog("Podaj godzinê (np. 8:30) o której chcesz byæ informowany o ew. przekroczeniach norm."), (new ResumeHelper(resume, OnAddNofiticationTimeProvided)).Resume);

            return Task.CompletedTask;
        }

        private async Task OnAddNofiticationTimeProvided(IDialogContext context, IAwaitable<DateTime> result, ResumeAfter<object> resume)
        {
            var time = await result;

            await context.SendTypingMessage();

            //await Accessor.AddNotificationTime(_conversationId, time.ToString("HH:mm"));

            await context.PostAsync($"Dodano powiadomienie o {time:HH:mm}");

            context.Wait(resume);
        }    
    }

    [Serializable]
    class ResumeHelper
    {
        private readonly ResumeAfter<object> _resume;
        private readonly Func<IDialogContext, IAwaitable<DateTime>, ResumeAfter<object>, Task> _action;

        public ResumeHelper(ResumeAfter<object> resume, Func<IDialogContext, IAwaitable<DateTime>, ResumeAfter<object>, Task> action)
        {
            _resume = resume;
            _action = action;
        }

        public Task Resume(IDialogContext context, IAwaitable<DateTime> result)
        {
            return _action(context, result, _resume);
        }
    }
}