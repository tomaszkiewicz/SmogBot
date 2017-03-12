using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Connector;

namespace SmogBot.Bot.Controllers
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        private readonly Tomaszkiewicz.BotFramework.Bot _bot;

        public MessagesController(Tomaszkiewicz.BotFramework.Bot bot)
        {
            _bot = bot;
        }

        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            switch (activity.GetActivityType())
            {
                case ActivityTypes.Message:
                    await _bot.OnMessage(activity);
                    break;

                case ActivityTypes.ConversationUpdate:
                    await _bot.OnConversationUpdate(activity);
                    break;

                case ActivityTypes.Trigger:
                    await _bot.OnConversationUpdate(activity);
                    break;

                case ActivityTypes.ContactRelationUpdate:
                    await _bot.OnContactRelationUpdate(activity);
                    break;


                case ActivityTypes.DeleteUserData:
                    await _bot.OnDeleteUserData(activity);
                    break;

                //case ActivityTypes.Typing:
                //case ActivityTypes.Ping:
                default:
                    await _bot.OnUnknownActivity(activity);
                    break;
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}