using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using SmogBot.Common.DatabaseAccessLayer;
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
            return _database.Query<Measurement>("SELECT * FROM [Common].[Measurements] WHERE CityName = @city", new
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

        public Task EnsureUser(string channelId, string fromId, string fromName, string conversationId, string conversationReference)
        {
            return _database.Execute("EXEC [Bot].[EnsureUser] @channelId, @fromId, @fromName, @conversationId, @conversationReference", new
            {
                ConversationId = conversationId,
                ChannelId = channelId,
                FromId = fromId,
                FromName = fromName,
                ConversationReference = conversationReference
            });
        }

        public Task SetUserPreferences(string channelId, string fromId, string fromName, string conversationId, string cityName)
        {
            return _database.Execute("EXEC [Bot].[SetUserPreferences] @channelId, @fromId, @fromName, @conversationId, @cityName", new
            {
                ConversationId = conversationId,
                ChannelId = channelId,
                FromId = fromId,
                FromName = fromName,
                CityName = cityName
            });
        }

        public Task SetUserCity(IActivity activity, string city)
        {
            return SetUserCity(activity.ChannelId, activity.From.Id, activity.From.Name, activity.Conversation.Id, city);
        }

        public Task SetUserCity(string channelId, string fromId, string fromName, string conversationId, string cityName)
        {
            return _database.Execute("EXEC [Bot].[SetUserCity] @channelId, @fromId, @fromName, @conversationId, @cityName", new
            {
                ConversationId = conversationId,
                ChannelId = channelId,
                FromId = fromId,
                FromName = fromName,
                CityName = cityName
            });
        }

        public Task UpdateLastActivityTime(string channelId, string fromId, string fromName, string conversationId, string conversationReference)
        {
            return _database.Execute("EXEC [Bot].[UpdateLastActivityTime] @channelId, @fromId, @fromName, @conversationId, @conversationReference", new
            {
                ConversationId = conversationId,
                ChannelId = channelId,
                FromId = fromId,
                FromName = fromName,
                ConversationReference = conversationReference
            });
        }

        public async Task<string> GetUserCity(IActivity activity)
        {
            var preferences = await GetUserPreferences(activity.ChannelId, activity.From.Id, activity.From.Name, activity.Conversation.Id);

            return preferences?.CityName;
        }

        public Task<UserPreferences> GetUserPreferences(string channelId, string fromId, string fromName, string conversationId)
        {
            return _database.Single<UserPreferences>("SELECT * " +
                                     "FROM [Bot].[UsersPreferences] " +
                                     "WHERE ChannelId = @channelId " +
                                     "AND FromId = @fromId " +
                                     "AND FromName = @fromName " +
                                     "AND ConversationId = @conversationId", new
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
            return _database.Execute("EXEC [Bot].[AddFeedback] @conversationId, @message", new
            {
                ConversationId = context.Activity.Conversation.Id,
                Message = feedback
            });
        }

        public Task<bool> GetWarningsStatus(IActivity activity)
        {
            return _database.ExecuteScalar<bool>("SELECT WarningsEnabled " +
                                                 "FROM [Bot].[WarningsStatus] " +
                                                 "WHERE [ConversationId] = @conversationId", new
            {
                ConversationId = activity.Conversation.Id
            });
        }

        public Task EnableWarnings(IActivity activity)
        {
            return _database.Execute("EXEC [Bot].[EnableWarnings] @conversationId", new
            {
                ConversationId = activity.Conversation.Id
            });
        }

        public Task DisableWarnings(IActivity activity)
        {
            return _database.Execute("EXEC [Bot].[DisableWarnings] @conversationId", new
            {
                ConversationId = activity.Conversation.Id
            });
        }
    }
}