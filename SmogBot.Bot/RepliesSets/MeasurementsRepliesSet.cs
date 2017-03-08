using Tomaszkiewicz.BotFramework.Tools;

namespace SmogBot.Bot.RepliesSets
{
    public class MeasurementsRepliesSet : RepliesSet
    {
        public readonly string CurrentMeasurementsKey = "currentMeasurements";

        public MeasurementsRepliesSet(InMemoryRepliesProvider provider) : base(provider)
        {
            provider.AddReplies(CurrentMeasurementsKey, new[]
            {
                "Bieżące przekroczenia norm w mieście {0}",
                "Stan w mieście {0}, już sprawdzam :)",
                "{0}, stan powietrza? Daj mi chwilę :)"
            });
        }
    }
}