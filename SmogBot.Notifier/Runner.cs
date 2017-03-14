using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
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

            var sw = Stopwatch.StartNew();

            var accessor = new NotifierAccessor(connStr);

            var usersToNotify = await accessor.GetUsersToNotify(timer.ScheduleStatus.Last, TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Central European Standard Time"));

            foreach (var user in usersToNotify)
            {
                var time = user.Time;
                var conversationReference = (ConversationReference) JsonConvert.DeserializeObject<ConversationReference>(user.ConversationReference);

                var reply = conversationReference.GetPostToUserMessage();

                reply.Text = "Hello from notifier!";

                var connector = reply.CreateConnectorClient();

                await connector.Conversations.SendToConversationAsync(reply);
            }

            sw.Stop();

            log.Info($"Notification check completed in {sw.Elapsed.TotalMilliseconds} ms");
        }
    }
}