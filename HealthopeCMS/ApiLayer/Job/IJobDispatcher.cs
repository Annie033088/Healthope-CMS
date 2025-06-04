namespace ApiLayer.Job
{
    public interface IJobDispatcher
    {
        void Enqueue<TJob, T>(T dto) where TJob : IJob<T>;
    }
}
