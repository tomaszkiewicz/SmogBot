using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Tomaszkiewicz.DapperExtensions;

namespace SmogBot.Bot.DatabaseAccessLayer
{
    public class BotAccessor
    {
        private readonly SqlConnectionFactory _database;

        public BotAccessor(SqlConnectionFactory database)
        {
            _database = database;
        }

        public Task<IEnumerable<Measurement>> GetNewestMeasurements(string city)
        {
            return _database.Query<Measurement>("SELECT * FROM [Bot].[Measurements] WHERE CityName = @city", new
            {
                City = city
            });
        }

        public Task<IEnumerable<string>> GetCities()
        {
            return _database.Query<string>("SELECT Name FROM [Bot].[Cities]");
        }

        public Task<IEnumerable<string>> GetNotificationTimes(string conversationId)
        {
            return _database.Query<string>("SELECT Time FROM [Bot].[NotificationTimes] WHERE ConversationId = @conversationId", new
            {
                ConversationId = conversationId
            });
        }

        public Task AddNotificationTime(string conversationId, string notificationTime)
        {
            return _database.Execute("EXEC [Bot].[AddNotificationTime] @conversationId, @notificationTime", new
            {
                ConversationId = conversationId,
                NotificationTime = notificationTime
            });
        }

        public Task DeleteNotificationTime(string conversationId, string notificationTime)
        {
            return _database.Execute("EXEC [Bot].[DeleteNotificationTime] @conversationId, @notificationTime", new
            {
                ConversationId = conversationId,
                NotificationTime = notificationTime
            });
        }
        
        public Task EnsureUser(string channelId, string fromId, string fromName, string conversationId)
        {
            return _database.Execute("EXEC [Bot].[EnsureUser] @channelId, @fromId, @fromName, @conversationId", new
            {
                ConversationId = conversationId,
                ChannelId = channelId,
                FromId = fromId,
                FromName = fromName
            });
        }

        public Task UpdateLastActivityTime(string channelId, string fromId, string fromName, string conversationId)
        {
            return _database.Execute("EXEC [Bot].[UpdateLastActivityTime] @channelId, @fromId, @fromName, @conversationId", new
            {
                ConversationId = conversationId,
                ChannelId = channelId,
                FromId = fromId,
                FromName = fromName
            });
        }

        public Task<IEnumerable<string>> SearchCity(string cityName)
        {
            return _database.Query<string>("EXEC [Bot].[SearchCity] @searchCity", new
            {
                SearchCity = cityName
            });
        }

        public Task SendFeedback(IDialogContext context, string feedback)
        {
            // TODO implement saving feedback

            return Task.CompletedTask;
        }
    }
}