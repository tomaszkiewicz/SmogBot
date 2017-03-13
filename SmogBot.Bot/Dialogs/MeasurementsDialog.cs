using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using SmogBot.Bot.DatabaseAccessLayer;
using SmogBot.Bot.Helpers;
using SmogBot.Bot.RepliesSets;
using Tomaszkiewicz.BotFramework.Extensions;
using Tomaszkiewicz.BotFramework.WebApi.Dialogs;

namespace SmogBot.Bot.Dialogs
{
    [Serializable]
    public class MeasurementsDialog : AutoDeserializeDialog<object>
    {
        [NonSerialized]
        private readonly BotAccessor _accessor;

        [NonSerialized]
        private readonly MeasurementsRepliesSet _measurementsRepliesSet;

        [NonSerialized]
        private readonly Func<SelectCityDialog> _selectCityDialogFactory;
        
        private string _city;

        public MeasurementsDialog(BotAccessor accessor, Func<SelectCityDialog> selectCityDialogFactory, MeasurementsRepliesSet measurementsRepliesSet)
        {
            _accessor = accessor;
            _selectCityDialogFactory = selectCityDialogFactory;
            _measurementsRepliesSet = measurementsRepliesSet;
        }

        public override async Task StartAsync(IDialogContext context)
        {
            if (!context.PrivateConversationData.TryGetValue(ConversationDataKeys.City, out _city))
                context.Call(_selectCityDialogFactory(), OnCitySelected);
            else
                await ShowMeasurements(context);
        }

        private async Task OnCitySelected(IDialogContext context, IAwaitable<string> result)
        {
            _city = await result;

            await ShowMeasurements(context);
        }

        private async Task ShowMeasurements(IDialogContext context)
        {
            await context.PostAsync(_measurementsRepliesSet.GetReply(context, _measurementsRepliesSet.CurrentMeasurementsKey, _city));
            await context.SendTypingMessage();

            var measurements = await _accessor.GetNewestMeasurements(_city);

            // TODO check if measurements are current (from last X hours)

            var reply = context.MakeCarousel();

            var measurementsByStation = measurements.GroupBy(x => x.StationName).OrderByDescending(x => x.Max(y => y.PercentNorm));

            foreach (var stationMeasurements in measurementsByStation)
            {
                var sb = new StringBuilder();

                var overNormMeasurements = stationMeasurements.Where(x => x.PercentNorm > 1).OrderByDescending(x => x.PercentNorm).ToArray();

                if (!overNormMeasurements.Any())
                    continue;

                foreach (var measurement in overNormMeasurements)
                    sb.AppendLine($"*** {measurement.PollutantName}: {measurement.PercentNorm * 100:#####}% normy ({measurement.Value:######} {measurement.Unit})");

                var heroCard = new HeroCard
                {
                    Title = stationMeasurements.Key,
                    Subtitle = $"Odczyt z godziny {stationMeasurements.Max(x => x.Time):HH:mm}",
                    Text = sb.ToString(),
                    //Images = new List<CardImage>()
                    //{
                    //    new CardImage($"http://powietrze.gios.gov.pl/pjp/current/station_picture/{measurement.StationCode}")
                    //},
                };

                reply.Attachments.Add(heroCard.ToAttachment());
            }

            if (!reply.Attachments.Any())
            {
                var heroCard = new HeroCard
                {
                    Title = _measurementsRepliesSet.GetReply(context, _measurementsRepliesSet.LimitsNotExceededTitle),
                    Text = _measurementsRepliesSet.GetReply(context, _measurementsRepliesSet.LimitsNotExceededText),
                };

                reply.Attachments.Add(heroCard.ToAttachment());
            }

            await context.PostAsync(reply);

            context.Done(_city);
        }
    }
}