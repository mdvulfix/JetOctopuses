using System.Threading.Tasks;

namespace APP
{
    public interface IBuilder
    {
        Task Build(IBuildScheme scheme);
    }

    public interface IBuildScheme
    {
        Task Execute();
    }
}