using System.Threading.Tasks;
using Hangfire.Server;

namespace ApiLayer.Job
{
    public interface IJob<in T>
    {
        Task Execute(T dto, PerformContext context);
    }
}
