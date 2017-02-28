using System.Configuration;
using System.Diagnostics;
using System.Threading.Tasks;
using GiosAirPollutionClient;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace SmogBot.Updater
{
    public class Runner
    {
        public static async Task Run(TimerInfo timer, TraceWriter log)
        {
            var connStr = ConfigurationManager.ConnectionStrings["Updater"].ConnectionString;

            var sw = Stopwatch.StartNew();

            var accessor = new UpdaterAccessor(connStr);

            var stationsData = await GiosClient.DownloadData();

            foreach (var stationData in stationsData)
            {
                log.Verbose($"{stationData.CityName} {stationData.StationName}");

                await accessor.EnsureStation(stationData.CityName, stationData.StationName);

                await accessor.UpdateAqiMeasurement(stationData.CityName, stationData.StationName, stationData.Time, stationData.AirQualityIndex);

                foreach (var measurement in stationData.Measurements)
                {
                    log.Verbose($"\t{measurement.Key}\t{measurement.Value}");

                    await accessor.UpdateMeasurement(stationData.CityName, stationData.StationName, stationData.Time, measurement.Key, measurement.Value);
                }
            }

            sw.Stop();

            log.Info($"Download completed in {sw.Elapsed.TotalMilliseconds} ms");
        }
    }
}