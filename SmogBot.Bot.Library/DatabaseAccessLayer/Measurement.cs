using System;

namespace SmogBot.Bot.DatabaseAccessLayer
{
    public class Measurement
    {
        public string CityName { get; set; }
        public string StationName { get; set; }
        public DateTime Time { get; set; }
        public string PollutantName { get; set; }
        public decimal Value { get; set; }
        public decimal Norm { get; set; }
        public string Unit { get; set; }
        public decimal PercentNorm { get; set; }

        public override string ToString()
        {
            return $"{CityName} {StationName} {Time} {PollutantName} = {Value}";
        }
    }
}