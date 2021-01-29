using System;
using System.Threading;
using System.Threading.Tasks;
using MCNMedia_Dev.Controllers;
using Microsoft.Extensions.Logging;

namespace MCNMedia_Dev.CronJobs
{
    public class CronJobEveryFiveMinute : CronJobService
    {
        private readonly ILogger<CronJobEveryFiveMinute> _logger;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public CronJobEveryFiveMinute(IScheduleConfig<CronJobEveryFiveMinute> config, ILogger<CronJobEveryFiveMinute> logger)
            : base(config.CronExpression, config.TimeZoneInfo)
        {
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            log.Info("JOB - CronJob Every 5 Minute starts.");
            return base.StartAsync(cancellationToken);
        }

        public override Task DoWork(CancellationToken cancellationToken)
        {
            log.Info($"{DateTime.Now:hh:mm:ss} CronJob Every 5 is working.");
            ScheduleController schedule = new ScheduleController();
            try
            {
                _Helper.Common.SaveToXXX("JOB - StopRecording - Begin");
                schedule.StopRecording();
                _Helper.Common.SaveToXXX("JOB - StopRecording - End");
            }
            catch (Exception ex)
            {
                log.Info("CronJob Every 5 Minute - Stop Recording - Error - " + ex.Message.ToString());
                _Helper.Common.SaveToXXX("CronJob Every 5 Minute - Stop Recording - Error - " + ex.Message.ToString());
            }
            try
            {
                _Helper.Common.SaveToXXX("JOB - StartRecording - Begin");
                schedule.StartRecording();
                _Helper.Common.SaveToXXX("JOB - StartRecording - End");
            }
            catch(Exception ex)
            {
                log.Info("CronJob Every 5 Minute - Start Recording - Error - " + ex.Message.ToString());
                _Helper.Common.SaveToXXX("CronJob Every 5 Minute - Start Recording - Error - " + ex.Message.ToString());
            }
            
            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            log.Info("JOB - CronJob Every 5 Minute is stopping.");
            return base.StopAsync(cancellationToken);
        }
    }
}