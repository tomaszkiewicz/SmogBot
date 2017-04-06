using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using SmogBot.Bot.DatabaseAccessLayer;
using SmogBot.Bot.RepliesSets;
using SmogBot.Common;
using Tomaszkiewicz.BotFramework.Extensions;
using Tomaszkiewicz.BotFramework.WebApi.Dialogs;

namespace SmogBot.Bot.Dialogs
{
    [Serializable]
    public class MeasurementsDialog : AutoDeserializeDialog<object>
    {
        [NonSerialized]
        private readonly BotAccessor _accessor;

        [NonSerialized]
        private readonly MeasurementsRepliesSet _measurementsRepliesSet;

        [NonSerialized]
        private readonly Func<SelectCityDialog> _selectCityDialogFactory;

        private string _city;

        public MeasurementsDialog(BotAccessor accessor, Func<SelectCityDialog> selectCityDialogFactory, MeasurementsRepliesSet measurementsRepliesSet)
        {
            _accessor = accessor;
            _selectCityDialogFactory = selectCityDialogFactory;
            _measurementsRepliesSet = measurementsRepliesSet;
        }

        public override async Task StartAsync(IDialogContext context)
        {
            _city = await _accessor.GetUserCity(context.Activity);
            
            if (string.IsNullOrWhiteSpace(_city))
                context.Call(_selectCityDialogFactory(), OnCitySelected);
            else
                await ShowMeasurements(context);
        }

        private async Task OnCitySelected(IDialogContext context, IAwaitable<string> result)
        {
            _city = await result;

            await ShowMeasurements(context);
        }

        private async Task ShowMeasurements(IDialogContext context)
        {
            await context.PostAsync(_measurementsRepliesSet.GetReply(context, _measurementsRepliesSet.CurrentMeasurementsKey, _city));
            await context.SendTypingMessage();

            var measurements = await _accessor.GetNewestMeasurements(_city);
            
            // TODO check if measurements are current (from last X hours)

            var reply = context.MakeCarousel();

            var measurementsByStation = measurements.GroupBy(x => x.StationName).OrderByDescending(x => x.Max(y => y.PercentNorm));
            
            var cards = MeasurementsCardBuilder.GetMeasurementsCards(measurementsByStation, GetBaseUrl());

            foreach (var card in cards)
                reply.Attachments.Add(card.ToAttachment());

            if (!reply.Attachments.Any())
            {
                var heroCard = new HeroCard
                {
                    Title = _measurementsRepliesSet.GetReply(context, _measurementsRepliesSet.LimitsNotExceededTitle),
                    Text = _measurementsRepliesSet.GetReply(context, _measurementsRepliesSet.LimitsNotExceededText),
                };

                reply.Attachments.Add(heroCard.ToAttachment());
            }

            await context.PostAsync(reply);

            context.Done(_city);
        }
        
        public string GetBaseUrl()
        {
            var request = HttpContext.Current.Request;
            var appUrl = HttpRuntime.AppDomainAppVirtualPath;

            if (appUrl != "/")
                appUrl = "/" + appUrl;

            var baseUrl = $"{request.Url.Scheme}://{request.Url.Authority}{appUrl}";

            return baseUrl;
        }
    }
}