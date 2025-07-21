using System.Threading.Tasks;
using Hangfire.Server;

namespace ApiLayer.Job
{
    public interface IJob<in T>
    {
        /// <summary>
        /// 執行任務
        /// </summary>
        Task Execute(T dto, PerformContext context);
    }

    public interface IJob
    {
        /// <summary>
        /// 執行任務
        /// </summary>
        Task Execute(PerformContext context);
    }
}
