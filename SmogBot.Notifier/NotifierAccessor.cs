using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmogBot.Common.DatabaseAccessLayer;
using Tomaszkiewicz.DapperExtensions;

namespace SmogBot.Notifier
{
    public class NotifierAccessor
    {
        private readonly SqlConnectionFactory _database;

        public NotifierAccessor(string connStr)
        {
            _database = new SqlConnectionFactory(connStr);
        }

        public Task<IEnumerable<UserToNotify>> GetUsersToNotify(DateTime lastTime, DateTime timeNow)
        {
            return _database.Query<UserToNotify>("EXEC [Notifier].[GetNotifications] @lastTime, @timeNow", new
            {
                LastTime = lastTime,
                TimeNow = timeNow
            });
        }
        
        public Task<IEnumerable<Measurement>> GetNewestMeasurements(string[] cities)
        {
            return _database.Query<Measurement>("SELECT * FROM [Common].[Measurements] WHERE CityName IN @cities", new
            {
                Cities = cities
            });
        }
    }
}