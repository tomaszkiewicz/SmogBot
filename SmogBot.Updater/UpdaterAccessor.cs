using System;
using System.Threading.Tasks;
using Tomaszkiewicz.DapperExtensions;

namespace SmogBot.Updater
{
    public class UpdaterAccessor
    {
        private readonly SqlConnectionFactory _database;

        public UpdaterAccessor(string connStr)
        {
            _database = new SqlConnectionFactory(connStr);
        }
        
        public Task UpdateMeasurement(string cityName, string stationName, DateTime time, string pollutantName, decimal value)
        {
            return _database.Execute("EXEC [Updater].[UpdateMeasurement] @cityName, @stationName, @time, @pollutantName, @value", new
            {
                CityName = cityName,
                StationName = stationName,
                Time = time,
                PollutantName = pollutantName,
                Value = value
            });
        }
        
        public Task EnsureStation(string cityName, string stationName)
        {
            return _database.Execute("EXEC [Updater].[EnsureStation] @cityName, @stationName", new
            {
                CityName = cityName,
                StationName = stationName,
            });
        }
    }
}