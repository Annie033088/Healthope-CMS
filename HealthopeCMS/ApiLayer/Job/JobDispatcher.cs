using System;
using Hangfire;

namespace ApiLayer.Job
{
    public class JobDispatcher : IJobDispatcher
    {
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
    }
}