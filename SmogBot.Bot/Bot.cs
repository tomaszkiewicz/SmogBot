using System.Threading.Tasks;
using Microsoft.Bot.Connector;

namespace SmogBot.Bot
{
    public abstract class Bot
    {
        public virtual Task OnMessage(Activity message)
        {
            return Task.CompletedTask;
        }

        public virtual Task OnConversationUpdate(IConversationUpdateActivity activity)
        {
            return Task.CompletedTask;
        }

        public virtual Task OnTrigger(ITriggerActivity activity)
        {
            return Task.CompletedTask;
        }

        public virtual Task OnContactRelationUpdate(IContactRelationUpdateActivity activity)
        {
            return Task.CompletedTask;
        }

        public virtual Task OnDeleteUserData(Activity activity)
        {
            return Task.CompletedTask;
        }

        public Task OnUnknownActivity(Activity activity)
        {
            return Task.CompletedTask;
        }
    }
}