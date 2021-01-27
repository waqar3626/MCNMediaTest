using System;
using System.Threading;
using System.Threading.Tasks;
using MCNMedia_Dev.Controllers;
using Microsoft.Extensions.Logging;

namespace MCNMedia_Dev.CronJobs
{
    public class CronJobEveryFiveMinute : CronJobService
    {
        private readonly ILogger<CronJobEveryMinute> _logger;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public CronJobEveryFiveMinute(IScheduleConfig<CronJobEveryMinute> config, ILogger<CronJobEveryMinute> logger)
            : base(config.CronExpression, config.TimeZoneInfo)
        {
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            log.Info("CronJob Every 5 Minute starts.");
            return base.StartAsync(cancellationToken);
        }

        public override Task DoWork(CancellationToken cancellationToken)
        {
            log.Info($"{DateTime.Now:hh:mm:ss} CronJob Every Minute is working.");
            ScheduleController schedule = new ScheduleController();
            try
            {
                schedule.StartRecording();
            }
            catch(Exception ex)
            {
                log.Info("CronJob Every 5 Minute - Start Recording - Error - " + ex.Message.ToString());
                _Helper.Common.SaveToXXX("CronJob Every 5 Minute - Start Recording - Error - " + ex.Message.ToString());
            }
            try
            {
                schedule.StopRecording();
            }
            catch (Exception ex)
            {
                log.Info("CronJob Every 5 Minute - Stop Recording - Error - " + ex.Message.ToString());
                _Helper.Common.SaveToXXX("CronJob Every 5 Minute - Stop Recording - Error - " + ex.Message.ToString());
            }
            
            try
            {
                CameraController camera = new CameraController();
                camera.SyncAllCamerasWithWowza();
            }
            catch (Exception ex)
            {
                log.Info("CronJob Every 5 Minute - Sync All Cam - Error - " + ex.Message.ToString());
                _Helper.Common.SaveToXXX("CronJob Every 5 Minute - Sync All Cam - Error - " + ex.Message.ToString());
            }
            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            log.Info("CronJob Every 5 Minute is stopping.");
            return base.StopAsync(cancellationToken);
        }
    }
}