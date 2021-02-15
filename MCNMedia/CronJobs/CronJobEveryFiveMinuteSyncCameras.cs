using System;
using System.Threading;
using System.Threading.Tasks;
using MCNMedia_Dev._Helper;
using MCNMedia_Dev.Controllers;
using Microsoft.Extensions.Logging;

namespace MCNMedia_Dev.CronJobs
{
    public class CronJobEveryFiveMinuteSyncCameras : CronJobService
    {
        private readonly ILogger<CronJobEveryFiveMinuteSyncCameras> _logger;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public CronJobEveryFiveMinuteSyncCameras(IScheduleConfig<CronJobEveryFiveMinuteSyncCameras> config, ILogger<CronJobEveryFiveMinuteSyncCameras> logger)
            : base(config.CronExpression, config.TimeZoneInfo)
        {
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            log.Info("JOB - CronJob Every 5 Minute Syncing Camera starts.");
            return base.StartAsync(cancellationToken);
        }

        public override Task DoWork(CancellationToken cancellationToken)
        {
            log.Info($"{DateTime.Now:hh:mm:ss} CronJob Every 5 Minute Syncing Camera is working.");
            try
            {
                _Helper.Common.SaveToXXX("CronJob Every 5 Minute Syncing Camera Started ");
                Wowza wowza = new Wowza();
                wowza.SyncAllCamerasWithWowza();
                _Helper.Common.SaveToXXX("CronJob Every 5 Minute Syncing Camera Ended ");
            }
            catch (Exception ex)
            {
                log.Info("CronJob Every 5 Minute Syncing Camera - Error - " + ex.Message.ToString());
                _Helper.Common.SaveToXXX("CronJob Every 5 Minute Syncing Camera - Error - " + ex.Message.ToString());
            }
            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            log.Info("JOB - CronJob Every 5 Minute Syncing Camera is stopping.");
            return base.StopAsync(cancellationToken);
        }
    }
}