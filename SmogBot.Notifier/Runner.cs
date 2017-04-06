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
using Newtonsoft.Json;
using SmogBot.Common;
using Tomaszkiewicz.BotFramework.Extensions;

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

            var nowCest = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Central European Standard Time");
            var accessor = new NotifierAccessor(connStr);

            var sw = Stopwatch.StartNew();
            
            var usersToNotify = (await accessor.GetUsersToNotify(timer.ScheduleStatus.Last, nowCest)).ToArray();

            var cities = usersToNotify.Select(x => x.CityName).Distinct().ToArray();
            var measurements = (await accessor.GetNewestMeasurements(cities)).ToArray();

            foreach (var user in usersToNotify)
            {
                var conversationReference = JsonConvert.DeserializeObject<ConversationReference>(user.ConversationReference);
                
                var measurementsByStation = measurements.Where(x => x.CityName == user.CityName)
                                                        .GroupBy(x => x.StationName)
                                                        .OrderByDescending(x => x.Max(y => y.PercentNorm));

                var cards = MeasurementsCardBuilder.GetMeasurementsCards(measurementsByStation, baseUrl).ToArray();
                
                if (!cards.Any())
                    continue;

                var reply = conversationReference.GetPostToUserMessage();

                reply.AttachmentLayout = "carousel";
                reply.Attachments = new List<Attachment>();

                foreach (var card in cards)
                    reply.Attachments.Add(card.ToAttachment());
                    
                var connector = reply.CreateConnectorClient();

                await connector.Conversations.SendToConversationAsync(reply);
            }

            sw.Stop();

            log.Info($"Notification check completed in {sw.Elapsed.TotalMilliseconds} ms");
        }
    }
}