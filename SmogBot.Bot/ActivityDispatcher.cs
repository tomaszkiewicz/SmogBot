using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Microsoft.Bot.Builder.Azure;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;

namespace SmogBot.Bot
{
    public abstract class ActivityDispatcher
    {
        public static async Task<object> Dispatch<T>(HttpRequestMessage req, IContainer container) where T : ActivityDispatcher
        {
            using (var scope = AutofacBootstrapper.Container.BeginLifetimeScope())
            {
                using (BotService.Initialize())
                {
                    var jsonContent = await req.Content.ReadAsStringAsync();
                    var activity = JsonConvert.DeserializeObject<Activity>(jsonContent);

                    if (!await BotService.Authenticator.TryAuthenticateAsync(req, new[] { activity }, CancellationToken.None))
                        return BotAuthenticator.GenerateUnauthorizedResponse(req);

                    if (activity == null)
                        return req.CreateResponse(HttpStatusCode.Accepted);

                    var dispatcherTarget = scope.Resolve<T>();

                    switch (activity.GetActivityType())
                    {
                        case ActivityTypes.Message:
                            await dispatcherTarget.OnMessage(activity);
                            break;

                        case ActivityTypes.ConversationUpdate:
                            await dispatcherTarget.OnConversationUpdate(activity);
                            break;

                        case ActivityTypes.Trigger:
                            await dispatcherTarget.OnConversationUpdate(activity);
                            break;

                        case ActivityTypes.ContactRelationUpdate:
                            await dispatcherTarget.OnContactRelationUpdate(activity);
                            break;
                            
                        
                        case ActivityTypes.DeleteUserData:
                            await dispatcherTarget.OnDeleteUserData(activity);
                            break;

                        //case ActivityTypes.Typing:
                        //case ActivityTypes.Ping:
                        default:
                            await dispatcherTarget.OnUnknownActivity(activity);
                            break;
                    }

                    return req.CreateResponse(HttpStatusCode.Accepted);
                }
            }
        }

        public virtual Task OnMessage(Activity message)
        {
            return Task.CompletedTask;
        }

        public virtual Task OnConversationUpdate(IConversationUpdateActivity activity)
        {
            return Task.CompletedTask;
        }

        public virtual Task OnTrigger(ITriggerActivity activity)
        {
            return Task.CompletedTask;
        }

        public virtual Task OnContactRelationUpdate(IContactRelationUpdateActivity activity)
        {
            return Task.CompletedTask;
        }

        public virtual Task OnDeleteUserData(Activity activity)
        {
            return Task.CompletedTask;
        }

        public Task OnUnknownActivity(Activity activity)
        {
            return Task.CompletedTask;
        }
    }
}