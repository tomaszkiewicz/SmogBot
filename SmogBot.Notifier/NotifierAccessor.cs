using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public Task<IEnumerable<dynamic>> GetUsersToNotify(DateTime lastTime, DateTime timeNow)
        {
            return _database.Query<dynamic>("EXEC [Notifier].[GetNotifications] @lastTime, @timeNow", new
            {
                LastTime = lastTime,
                TimeNow = timeNow
            });
        }
    }
}