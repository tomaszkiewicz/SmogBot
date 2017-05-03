using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using SmogBot.MessageSender.Properties;
using Tomaszkiewicz.DapperExtensions;

namespace SmogBot.MessageSender
{
    class Program
    {
        static void Main(string[] args)
        {
            Run(int.Parse(args[0]), string.Join(" ", args.Skip(1).ToArray())).Wait();
        }

        static async Task Run(int userId, string message)
        {
            var database = new SqlConnectionFactory(Settings.Default.ConnectionString);

            var conversationReferenceStr = await database.ExecuteScalar<string>("SELECT [ConversationReference] FROM [dbo].[Users] WHERE Id = @userId", new
            {
                UserId = userId
            });

            var conversationReference = JsonConvert.DeserializeObject<ConversationReference>(conversationReferenceStr);
            
            var reply = conversationReference.GetPostToUserMessage();

            reply.Text = message;
            
            await AddMessageToQueueAsync(JsonConvert.SerializeObject(reply));
        }

        public static async Task AddMessageToQueueAsync(string message)
        {
            var storageAccount = CloudStorageAccount.Parse(Settings.Default.AzureWebJobsStorage);

            var queueClient = storageAccount.CreateCloudQueueClient();

            var queue = queueClient.GetQueueReference("bot-queue");

            await queue.CreateIfNotExistsAsync();

            var queueMessage = new CloudQueueMessage(message);

            await queue.AddMessageAsync(queueMessage);
        }
    }
}