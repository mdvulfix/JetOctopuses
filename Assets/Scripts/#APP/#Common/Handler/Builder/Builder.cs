using System.Threading.Tasks;

namespace APP
{
    public static class Builder
    {
        public static async Task Execute(IBuildScheme scheme) => 
            await scheme.Execute();
    }


}