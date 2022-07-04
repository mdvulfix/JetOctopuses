using System.Threading.Tasks;
using APP;

namespace SERVICE.Handler
{
    public static class BuildHandler
    {
        public static async Task Build(IBuildScheme scheme) => 
            await scheme.Execute();
    }


}