using System;
using System.Collections.Generic;
using System.Linq;
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
                var overNormMeasurements = stationMeasurements.Where(x => x.AqiValue > 0);

                if (!overNormMeasurements.Any())
                    continue;
                
                var time = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(stationMeasurements.Max(x => x.Time), "Central European Standard Time");
                var heroCard = new HeroCard
                {
                    Title = $"{stationMeasurements.First().CityName}, {stationMeasurements.Key}",
                    Subtitle = $"Odczyt z godziny {time:HH:mm}",
                    Images = new List<CardImage>
                    {
                        new CardImage(baseUrl + $"api/images/station/{stationMeasurements.First().StationId}?time={time:s}")
                    },
                };

                yield return heroCard;
            }
        }
    }
}