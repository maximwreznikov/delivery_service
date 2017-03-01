using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;

namespace DeliveryService.Scheduling
{
    public class SchedulerRunner
    {
        public const int DefaultInterval = 10;
        public const string DefaultGroup = "default_group";
        public const string DefaultTrigger = "default_trigger";
        public const string DefaultJob = "default_job";

        private readonly IScheduler _scheduler;

        #region Singleton implementation
        private static readonly Lazy<SchedulerRunner> Lazy = new Lazy<SchedulerRunner>(() => new SchedulerRunner());

        public static SchedulerRunner Instance => Lazy.Value;
        #endregion

        public SchedulerRunner()
        {
            // Grab the Scheduler instance from the Factory
            NameValueCollection props = new NameValueCollection
            {
                ["quartz.jobStore.type"] = "Quartz.Simpl.RAMJobStore, Quartz",
                // "json" is alias for "Quartz.Simpl.JsonObjectSerializer, Quartz.Serialization.Json" 
                ["quartz.serializer.type"] = "json"
            };
            StdSchedulerFactory factory = new StdSchedulerFactory(props);
            _scheduler = factory.GetScheduler().GetAwaiter().GetResult();
        }

        public async Task RunJob<TJob>(int periodInSeconds = DefaultInterval, 
            string jobName = DefaultJob, 
            string triggerName = DefaultTrigger,
            string group = DefaultGroup) where TJob : IJob
        {
            try
            {
                // and start it off
                await _scheduler.Start();

                // define the job and tie it to our HelloJob class
                IJobDetail job = JobBuilder.Create<TJob>()
                    .WithIdentity(jobName, group)
                    .Build();

                // Trigger the job to run now, and then repeat every 10 seconds
                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity(triggerName, group)
                    .StartNow()
                    .WithSimpleSchedule(x => x
                        .WithIntervalInSeconds(periodInSeconds)
                        .RepeatForever())
                    .Build();

                // Tell quartz to schedule the job using our trigger
                await _scheduler.ScheduleJob(job, trigger);
            }
            catch (SchedulerException se)
            {
                Console.WriteLine(se);
            }
        }

        public async Task Shutdown()
        {
            await Console.Out.WriteLineAsync("Gracefull shutdown scheduler.");

            // some sleep to show what's happening
            await Task.Delay(TimeSpan.FromSeconds(10));

            // and last shut down the _scheduler when you are ready to close your program
            await _scheduler.Shutdown();
        }
    }
}
