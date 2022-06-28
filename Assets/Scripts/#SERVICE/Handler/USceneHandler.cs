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
                    return;
            }

            var operation = SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
            await TaskHandler.Run(() => AwaitSceneOperation(operation),"Await scene operation complete...");
            Send($"{sceneIndex} loaded...");
        }

        public static async Task SceneActivate(SceneIndex? sceneIndex)
        {
            var index = (int) sceneIndex;
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var uScene = GetUSceneByIndex(i);
                if (uScene.buildIndex == index)
                {
                    SceneManager.SetActiveScene(uScene);
                    Send($"{sceneIndex} activated...");
                    return;
                }
            }

            await SceneLoad(sceneIndex);
            await SceneActivate(sceneIndex);
            Send($"{sceneIndex} activated...");
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

        
        private static bool AwaitSceneOperation(AsyncOperation operation)
        {
            if (operation.isDone)
                return true;

            return false;
        }

        private static UScene GetUSceneByIndex(int index) =>
            SceneManager.GetSceneAt(index);

        private static string Send(string text, bool isWorning = false) =>
            LogHandler.Send("USceneHandler", true, text, isWorning);

    }

}