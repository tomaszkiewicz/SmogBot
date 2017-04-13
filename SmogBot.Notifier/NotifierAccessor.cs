using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmogBot.Common.DatabaseAccessLayer;
using SmogBot.Notifier.Entities;
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

        public Task<IEnumerable<NotificationContext>> GetUsersToNotify(DateTime lastTime, DateTime timeNow)
        {
            return _database.Query<NotificationContext>("EXEC [Notifier].[GetNotifications] @lastTime, @timeNow", new
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

        public Task<IEnumerable<NotificationContext>> GetActiveWarnings()
        {
            return _database.Query<NotificationContext>("SELECT DISTINCT [ConversationReference], [CityName], [UserId] FROM [Notifier].[ActiveWarnings]");
        }

        public Task UpdateWarnings(int userId)
        {
            return _database.Execute("EXEC [Notifier].[UpdateWarnings] @userId", new
            {
                UserId = userId
            });
        }
    }
}