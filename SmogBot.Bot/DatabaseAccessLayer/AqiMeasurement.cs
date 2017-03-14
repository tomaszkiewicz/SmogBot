using System;

namespace SmogBot.Bot.DatabaseAccessLayer
{
    public class AqiMeasurement
    {
        public string CityName { get; set; }
        public string StationName { get; set; }
        public DateTime Time { get; set; }
        public int Value { get; set; }
    }
}