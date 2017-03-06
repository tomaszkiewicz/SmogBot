using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Microsoft.Bot.Builder.Azure;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SmogBot.Bot
{
    public class Runner
    {
        public static async Task<object> Run(HttpRequestMessage req)
        {
            return ActivityDispatcher.Dispatch<SmogBotDispatcher>(req, AutofacBootstrapper.Container);

            using (var scope = AutofacBootstrapper.Container.BeginLifetimeScope())
            {
                //log.Info($"Webhook was triggered in compiled function!");

                using (BotService.Initialize())
                {
                    var jsonContent = await req.Content.ReadAsStringAsync();
                    var activity = JsonConvert.DeserializeObject<Activity>(jsonContent);

                    if (!await BotService.Authenticator.TryAuthenticateAsync(req, new[] { activity }, CancellationToken.None))
                        return BotAuthenticator.GenerateUnauthorizedResponse(req);

                    if (activity == null)
                        return req.CreateResponse(HttpStatusCode.Accepted);

                    switch (activity.GetActivityType())
                    {
                        case ActivityTypes.Message:
                            await Conversation.SendAsync(activity, scope.Resolve<Func<BasicProactiveEchoDialog>>());
                            break;

                        case ActivityTypes.ConversationUpdate:
                            var client = new ConnectorClient(new Uri(activity.ServiceUrl));

                            IConversationUpdateActivity update = activity;

                            if (update.MembersAdded.Any())
                            {
                                var reply = activity.CreateReply();

                                var newMembers = update.MembersAdded?.Where(t => t.Id != activity.Recipient.Id);

                                if (newMembers != null)
                                    foreach (var newMember in newMembers)
                                    {
                                        reply.Text = "Welcome";

                                        if (!string.IsNullOrEmpty(newMember.Name))
                                            reply.Text += $" {newMember.Name}";

                                        reply.Text += " from compiled function!";

                                        await client.Conversations.ReplyToActivityAsync(reply);
                                    }
                            }

                            break;

                        case ActivityTypes.Trigger:
                            // handle proactive Message from function
                            //log.Info("Trigger start");

                            ITriggerActivity trigger = activity;

                            var message = JsonConvert.DeserializeObject<Message>(((JObject)trigger.Value).GetValue("Message").ToString());
                            var messageActivity = message.ResumptionCookie.GetMessage();

                            client = new ConnectorClient(new Uri(messageActivity.ServiceUrl));

                            var triggerReply = messageActivity.CreateReply();

                            triggerReply.Text = $"This is coming back from the trigger! {message.Text}";

                            await client.Conversations.ReplyToActivityAsync(triggerReply);

                            //log.Info("Trigger end");

                            break;
                        case ActivityTypes.ContactRelationUpdate:
                        case ActivityTypes.Typing:
                        case ActivityTypes.DeleteUserData:
                        case ActivityTypes.Ping:
                        default:
                            //log.Error($"Unknown activity type ignored: {activity.GetActivityType()}");
                            break;
                    }

                    return req.CreateResponse(HttpStatusCode.Accepted);
                }
            }
        }
    }
}