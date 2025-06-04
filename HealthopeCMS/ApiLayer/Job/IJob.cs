using System.Threading.Tasks;

namespace ApiLayer.Job
{
    public interface IJob<in T>
    {
        Task Execute(T dto);
    }
}
