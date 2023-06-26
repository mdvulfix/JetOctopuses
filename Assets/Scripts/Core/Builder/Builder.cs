using System.Threading.Tasks;

namespace Core.Builder
{
    public static class Builder
    {
        public static async Task Execute(IBuildScheme scheme) =>
            await scheme.Execute();
    }


}