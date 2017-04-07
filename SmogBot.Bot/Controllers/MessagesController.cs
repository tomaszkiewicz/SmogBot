using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Connector;
using Tomaszkiewicz.BotFramework.Extensions;

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
            try
            {
                await _bot.DispatchActivity(activity);
            }
            catch (Exception ex)
            {
                var connector = activity.CreateConnectorClient();

                connector.Conversations.SendToConversation(activity.CreateReply("Controller exception: " + ex.Message + ex.StackTrace));

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message + ex.StackTrace);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}