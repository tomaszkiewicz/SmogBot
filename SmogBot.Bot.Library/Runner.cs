using System.Net.Http;
using System.Threading.Tasks;

namespace SmogBot.Bot
{
    public class Runner
    {
        public static Task<object> Run(HttpRequestMessage req)
        {
            return ActivityDispatcher.Dispatch<SmogBotDispatcher>(req, AutofacBootstrapper.Container);
        }
    }
}