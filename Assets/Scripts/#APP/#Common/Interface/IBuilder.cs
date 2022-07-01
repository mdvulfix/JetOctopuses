using System.Threading.Tasks;

namespace SERVICE.Builder
{
    public interface IBuilder
    {
        Task Build(IBuildScheme scheme);
    }
}