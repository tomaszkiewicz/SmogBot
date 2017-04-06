using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using GiosAirPollutionClient;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace SmogBot.Updater
{
    public class Runner
    { 
#if DEBUG
        public static Task RunTimer(TimerInfo timer, TextWriter log)
        {
            return Run(new ConsoleTextWriter(log, TraceLevel.Verbose));
        }
#else
        public static Task RunTimer(TimerInfo timer, TraceWriter log)
        {
            return Run(log);
        }
#endif
        public static async Task Run(TraceWriter log)
        {
            var connStr = ConfigurationManager.ConnectionStrings["Updater"].ConnectionString;

            var sw = Stopwatch.StartNew();

            var accessor = new UpdaterAccessor(connStr);

            var stationsData = await GiosClient.DownloadData();

            foreach (var stationData in stationsData)
            {
                log.Verbose($"{stationData.CityName} {stationData.StationName}");

                await accessor.EnsureStation(stationData.CityName, stationData.StationName);

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