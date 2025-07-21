using System;
using Hangfire;

namespace ApiLayer.Job
{
    public class JobDispatcher : IJobDispatcher
    {
        /// <summary>
        /// 佇列一個工作任務
        /// </summary>
        public void Enqueue<TJob, T>(T dto) where TJob : IJob<T>
        {
            try
            {
                BackgroundJob.Enqueue<TJob>(job => job.Execute(dto, null));
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 註冊一個週期性任務（如每日、每月等）
        /// </summary>
        public void ScheduleRecurring<TJob>(string recurringJobId, string cronExpression)
            where TJob : IJob
        {
            try
            {
                RecurringJob.AddOrUpdate<TJob>(
                    recurringJobId,
                    job => job.Execute(null),
                    cronExpression,
                    new RecurringJobOptions
                    {
                        TimeZone = TimeZoneInfo.FindSystemTimeZoneById("UTC"),
                    }
                );
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}