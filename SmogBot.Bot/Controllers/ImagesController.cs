using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using SmogBot.Bot.DatabaseAccessLayer;

namespace SmogBot.Bot.Controllers
{
    public class ImagesController : ApiController
    {
        private readonly BotAccessor _accessor;

        public ImagesController(BotAccessor accessor)
        {
            _accessor = accessor;
        }

        [HttpGet]
        [Route("api/images/station/{stationId}")]
        public async Task<HttpResponseMessage> Get(int stationId)
        {
            var measurements = (await _accessor.GetNewestMeasurements(stationId)).OrderByDescending(x => x.PercentNorm).ToArray();

            const int height = (int)(500 / 1.91);
            const int width = 500;

            using (var bitmap = new Bitmap(width, height))
            using (var graphics = Graphics.FromImage(bitmap))
            using (var ms = new MemoryStream())
            {
                var brushBackColor = new SolidBrush(Color.White);
                var displayRectangle = new Rectangle(new Point(0, 0), new Size(width - 1, height - 1));

                graphics.FillRectangle(brushBackColor, displayRectangle);

                const int boxWidth = (width - 40) / 3;
                const int boxHeight = (height - 30) / 2;

                if (measurements.Length >= 1)
                    DrawPollutant(graphics, 10, 10, boxWidth, boxHeight, measurements[0].PollutantName, measurements[0].Value, measurements[0].Norm, measurements[0].Unit, measurements[0].AqiValue);

                if (measurements.Length >= 2)
                    DrawPollutant(graphics, 20 + boxWidth, 10, boxWidth, boxHeight, measurements[1].PollutantName, measurements[1].Value, measurements[1].Norm, measurements[1].Unit, measurements[1].AqiValue);

                if (measurements.Length >= 3)
                    DrawPollutant(graphics, 30 + boxWidth * 2, 10, boxWidth, boxHeight, measurements[2].PollutantName, measurements[2].Value, measurements[2].Norm, measurements[2].Unit, measurements[2].AqiValue);

                if (measurements.Length >= 4)
                    DrawPollutant(graphics, 10, 20 + boxHeight, boxWidth, boxHeight, measurements[3].PollutantName, measurements[3].Value, measurements[3].Norm, measurements[3].Unit, measurements[3].AqiValue);

                if (measurements.Length >= 5)
                    DrawPollutant(graphics, 20 + boxWidth, 20 + boxHeight, boxWidth, boxHeight, measurements[4].PollutantName, measurements[4].Value, measurements[4].Norm, measurements[4].Unit, measurements[4].AqiValue);

                if (measurements.Length >= 6)
                    DrawPollutant(graphics, 30 + boxWidth * 2, 20 + boxHeight, boxWidth, boxHeight, measurements[5].PollutantName, measurements[5].Value, measurements[5].Norm, measurements[5].Unit, measurements[5].AqiValue);
                
                bitmap.Save(ms, ImageFormat.Png);

                var result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(ms.ToArray())
                };

                result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");

                return result;
            }
        }

        static void DrawPollutant(Graphics graphics, int x, int y, int width, int height, string name, decimal value, decimal norm, string unit, int aqiValue)
        {
            var displayRectangle = new Rectangle(new Point(x, y), new Size(width, height));

            var brushBackColor = new SolidBrush(GetBackgroundColorByAqiValue(aqiValue));
            var font = new Font("Segoe UI", 22);
            var textBrush = new SolidBrush(GetTextColorByAqiValue(aqiValue));

            graphics.FillRectangle(brushBackColor, displayRectangle);

            graphics.DrawString(name, font, textBrush, x + 5, y + 5);

            var percentFont = new Font("Segoe UI", 24);
            var percentText = $"{value / norm * 100:0}%";
            var percentMeasure = graphics.MeasureString(percentText, percentFont);
            graphics.DrawString(percentText, percentFont, textBrush, x + width - percentMeasure.Width - 5, y + 40);

            var valueNormFont = new Font("Segoe UI", 12);
            var valueNormText = $"{value:0} / {norm:0} {unit}";
            var valueNormMeasure = graphics.MeasureString(valueNormText, valueNormFont);

            graphics.DrawString(valueNormText, valueNormFont, textBrush, x + width - valueNormMeasure.Width - 5, y + 85);
        }
        
        private static Color GetTextColorByAqiValue(int aqiValue)
        {
            switch (aqiValue)
            {
                case 1:
                    return Color.DarkSlateGray;
                    
                default:
                    return Color.White;
            }
        }

        private static Color GetBackgroundColorByAqiValue(int aqiValue)
        {
            switch (aqiValue)
            {
                case 0:
                    return Color.FromArgb(0, 153, 102);

                case 1:
                    return Color.FromArgb(255, 222, 51);

                case 2:
                    return Color.FromArgb(255, 153, 51);

                case 3:
                    return Color.FromArgb(204, 0, 51);

                case 4:
                    return Color.FromArgb(102, 0, 153);

                case 5:
                    return Color.FromArgb(126, 0, 35);

                default:
                    return Color.Gray;
            }
        }
    }
}