using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Bot.Connector;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using SmogBot.Common;

namespace SmogBot.Notifier
{
    public class Runner
    {
#if DEBUG
        public static Task RunTimer(TimerInfo timer, TextWriter log)
        {
            return Run(timer, new ConsoleTextWriter(log, TraceLevel.Verbose));
        }
#else
        public static Task RunTimer(TimerInfo timer, TraceWriter log)
        {
            return Run(timer, log);
        }
#endif
        public static async Task Run(TimerInfo timer, TraceWriter log)
        {
            var connStr = ConfigurationManager.ConnectionStrings["Notifier"].ConnectionString;
            var baseUrl = ConfigurationManager.AppSettings["BaseUrl"];

            var accessor = new NotifierAccessor(connStr);
            var lastCheck = timer.ScheduleStatus.Last;
            var lastCheckCest = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(lastCheck, "Central European Standard Time");
            var nowCest = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Central European Standard Time");

            var sw = Stopwatch.StartNew();

            // get data from db

            var usersToNotify = (await accessor.GetUsersToNotify(lastCheckCest, nowCest)).ToArray();
            var usersToWarn = (await accessor.GetActiveWarnings()).ToArray();

            var users = usersToNotify.Concat(usersToWarn).Distinct().ToArray();

            log.Info($"Found {users.Length} users to notify.");

            // get cities and measurements

            var cities = users.Select(x => x.CityName).Distinct().ToArray();
            var measurements = (await accessor.GetNewestMeasurements(cities)).ToArray();

            // process notifications and warnings

            foreach (var user in users)
            {
                log.Info($"Notifying user with UserId = {user.UserId}.");

                var conversationReference = JsonConvert.DeserializeObject<ConversationReference>(user.ConversationReference);

                var measurementsByStation = measurements.Where(x => x.CityName == user.CityName)
                    .GroupBy(x => x.StationName)
                    .OrderByDescending(x => x.Max(y => y.PercentNorm));

                var cards = MeasurementsCardBuilder.GetMeasurementsCards(measurementsByStation, baseUrl).ToArray();

                var reply = conversationReference.GetPostToUserMessage();

                if (cards.Any())
                {
                    reply.AttachmentLayout = "carousel";
                    reply.Attachments = new List<Attachment>();

                    foreach (var card in cards)
                        reply.Attachments.Add(card.ToAttachment());
                }
                else
                {
                    reply.Text = "Powietrze znów jest czyste - żadne normy nie są obecnie przekroczone :)";
                }

                if (!usersToWarn.Contains(user) && !cards.Any()) // only send info about clean air when user has warnings enabled
                    continue;

                await AddMessageToQueueAsync(JsonConvert.SerializeObject(reply));

                await accessor.UpdateWarnings(user.UserId);
            }

            sw.Stop();

            log.Info($"Notifications and warnings check completed in {sw.Elapsed.TotalMilliseconds} ms");
        }

        public static async Task AddMessageToQueueAsync(string message)
        {
            var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["AzureWebJobsStorage"]);

            var queueClient = storageAccount.CreateCloudQueueClient();

            var queue = queueClient.GetQueueReference("bot-queue");

            await queue.CreateIfNotExistsAsync();

            var queueMessage = new CloudQueueMessage(message);

            await queue.AddMessageAsync(queueMessage);
        }
    }
}