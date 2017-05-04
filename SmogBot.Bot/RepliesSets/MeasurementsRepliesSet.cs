using Tomaszkiewicz.BotFramework.Tools;

namespace SmogBot.Bot.RepliesSets
{
    public class MeasurementsRepliesSet : RepliesSet
    {
        public readonly string LimitsNotExceededTitle = "limitsNotExceededTitle";
        public readonly string LimitsNotExceededText = "limitsNotExceededText";
        public readonly string CurrentMeasurementsKey = "currentMeasurements";

        public MeasurementsRepliesSet(InMemoryRepliesProvider provider) : base(provider)
        {
            provider.AddReplies(CurrentMeasurementsKey, new[]
            {
                "Bieżące przekroczenia norm w mieście {0}",
                "Stan w mieście {0}, już sprawdzam :)",
                "{0}, stan powietrza? Daj mi chwilę :)",
                "Sprawdzam, daj mi kilka sekund :)"
            });

            provider.AddReplies(LimitsNotExceededTitle, new[]
            {
                "Brak przekroczeń!"
            });
        
            provider.AddReplies(LimitsNotExceededText, new[]
            {
                "Pora wyjść na spacer :)",
                "Maski zdejmij!"
            });
        }
    }
}