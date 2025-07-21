namespace ApiLayer.Job
{
    public interface IJobDispatcher
    {
        /// <summary>
        /// 佇列一個工作任務
        /// </summary>
        void Enqueue<TJob, T>(T dto) where TJob : IJob<T>;

        /// <summary>
        /// 註冊一個週期性任務（如每日、每月等）
        /// </summary>
        void ScheduleRecurring<TJob>(string recurringJobId, string cronExpression)
           where TJob : IJob;
    }
}
