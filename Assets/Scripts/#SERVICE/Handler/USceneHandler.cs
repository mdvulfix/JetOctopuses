using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UScene = UnityEngine.SceneManagement.Scene;
using APP;


namespace SERVICE.Handler
{
    public static class USceneHandler
    {
        public static async Task SceneLoad(SceneIndex? sceneIndex)
        {
            var index = (int) sceneIndex;
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var uScene = GetUSceneByIndex(i);
                if (uScene.buildIndex == index)
                {
                    Send($"{sceneIndex} already loaded...");
                    return;
                }
            }

            var operation = SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
            await TaskHandler.Run(() => AwaitSceneLoading(operation),"Await scene loading complete...");

            


            Send($"{sceneIndex} loaded...");
        }

        public static async Task SceneActivate(SceneIndex? sceneIndex)
        {
            await SceneLoad(sceneIndex);
            
            var index = (int) sceneIndex;
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var uScene = GetUSceneByIndex(i);
                if (uScene.buildIndex == index)
                {
                    await Task.Delay(5);
                    SceneManager.SetActiveScene(uScene);
                    Send($"{sceneIndex} activated...");
                    return;
                }
            }

            Send($"{sceneIndex} not found. Activation failed!", true);
        }

        public static async Task SceneUnload(SceneIndex? sceneIndex)
        {
            await Task.Delay(1);
            Send($"{sceneIndex} unloaded... Not implemented!", true);
        }

        public static async Task SceneReload(SceneIndex? sceneIndex)
        {
            await Task.Delay(1);
            Send($"{sceneIndex} reloaded... Not implemented!", true);
        }

        
        private static bool AwaitSceneLoading(AsyncOperation loading)
        {
            if (loading.isDone)
                return true;

            return false;
        }

        private static UScene GetUSceneByIndex(int index) =>
            SceneManager.GetSceneAt(index);

        private static string Send(string text, bool isWorning = false) =>
            LogHandler.Send("USceneHandler", true, text, isWorning);

    }

}