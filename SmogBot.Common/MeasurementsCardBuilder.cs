using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Bot.Connector;
using SmogBot.Common.DatabaseAccessLayer;

namespace SmogBot.Common
{
    public static class MeasurementsCardBuilder
    {
        public static IEnumerable<HeroCard> GetMeasurementsCards(IOrderedEnumerable<IGrouping<string, Measurement>> measurementsByStation, string baseUrl)
        {
            foreach (var stationMeasurements in measurementsByStation)
            {
                var sb = new StringBuilder();

                var overNormMeasurements = stationMeasurements.Where(x => x.PercentNorm > 1).OrderByDescending(x => x.PercentNorm).ToArray();

                if (!overNormMeasurements.Any())
                    continue;

                foreach (var measurement in overNormMeasurements)
                    sb.AppendLine($"{measurement.PollutantName}: {measurement.PercentNorm * 100:#####}% normy ({measurement.Value:######} {measurement.Unit})");

                var stationAqi = stationMeasurements.Max(x => x.AqiValue);

                var heroCard = new HeroCard
                {
                    Title = stationMeasurements.Key,
                    Subtitle = $"Odczyt z godziny {stationMeasurements.Max(x => x.Time):HH:mm}",
                    Text = sb.ToString(),
                    Images = new List<CardImage>()
                    {
                        new CardImage(baseUrl + $"Images/aqi{stationAqi}.jpg")
                    },
                };

                yield return heroCard;
            }
        }
    }
}
