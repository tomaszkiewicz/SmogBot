using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using SmogBot.Bot.Extensions;

namespace SmogBot.Bot.Tools
{
    [Serializable]
    public abstract class AutoDeserializeDialog<T> : IDialog<T>
    {
        [OnDeserialized]
        private void OnDeserialized(StreamingContext ctx)
        {
            this.RestoreAllDependencies();
        }

        public abstract Task StartAsync(IDialogContext context);
    }
}