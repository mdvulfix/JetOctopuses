using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UScene = UnityEngine.SceneManagement.Scene;
using APP;
using System;

namespace SERVICE.Handler
{
    public static class SceneHandler
    {
        private static bool m_Debug = true;

        // SCENE TASK //
        public static async Task<ITaskResult> USceneLoad(SceneIndex? sceneIndex)
        {
            if(GetUSceneLoaded(sceneIndex, out var uScene))
                return new TaskResult (true, Send($"UScene {sceneIndex} was already loaded..."));


            var index = (int) sceneIndex;            
            var operation = SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);

            await TaskHandler.Run(() => 
                AwaitLoadingOperationComplete(operation),$"Await UScene {sceneIndex} loading operation complete...");
            
            return new TaskResult (true, Send($"UScene {sceneIndex} was successfully loaded..."));
        }

        public static async Task<ITaskResult> USceneActivate(SceneIndex? sceneIndex)
        {
            if(GetUSceneLoaded(sceneIndex, out var uScene))
            {
                if(uScene.isLoaded == false)
                {
                    await TaskHandler.Run(() =>
                        AwaitUSceneLoading(uScene),$"Await UScene {sceneIndex} loading...", 1000f);
                }

                SceneManager.SetActiveScene(uScene);
                return new TaskResult (true, Send($"UScene {sceneIndex} was successfully activated..."));
            }

            Send($"UScene {sceneIndex} not found. Activation failed!", LogFormat.Worning);
            
            var uSceneLoadTAskResult = await USceneLoad(sceneIndex);
            if(uSceneLoadTAskResult.Status == false)
                return new TaskResult (false, Send($"UScene {sceneIndex} can not be loaded!", LogFormat.Worning));
                

            var uSceneActivateTaskResult = await USceneActivate(sceneIndex);
            if(uSceneActivateTaskResult.Status == false)
                return new TaskResult (false, Send($"UScene {sceneIndex} can not be activated!", LogFormat.Worning));

            return new TaskResult (true, Send($"UScene {sceneIndex} was successfully activated..."));

        }

        public static async Task<ITaskResult> USceneUnload(SceneIndex? sceneIndex)
        {
            await Task.Delay(1);
            return new TaskResult (true, Send($"UScene {sceneIndex} was unloaded... Not implemented!", LogFormat.Worning));
        }

        public static async Task<ITaskResult> USceneReload(SceneIndex? sceneIndex)
        {
            await Task.Delay(1);
            return new TaskResult (true, Send($"UScene {sceneIndex} was eloaded... Not implemented!", LogFormat.Worning));
        }
        

        // SCENE OBJECT //
        public static GameObject SetGameObject(string name = "Unnamed", GameObject parent = null)
        {
            var obj = new GameObject(name);
            obj.SetActive(false);
        
            if (parent != null)
                obj.transform.SetParent(parent.transform);

            return obj;
        }
        
        public static void RemoveGameObject(GameObject gameObject)
        {
            MonoBehaviour.Destroy(gameObject);
        }

        public static TComponent SetComponent<TComponent>(GameObject gameObject)
        where TComponent : Component
        {
            return gameObject.AddComponent<TComponent>();
        }

        
        // HELPERS //
        private static bool AwaitLoadingOperationComplete(AsyncOperation loading)
        {           
            Send($"UScene loading progress {Math.Round(loading.progress * 100, 1)}%...");
            if (loading.progress >= 0.9f)
                return true;
                
            return false;
        }

        private static bool AwaitUSceneLoading(UScene scene)
        {            
            if (scene.isLoaded == true)
                return true;
                
            return false;
        }

        private static bool GetUSceneLoaded(SceneIndex? sceneIndex, out UScene scene)
        {
            scene = default(UScene);

            var index = (int)sceneIndex;
            var count = SceneManager.sceneCount;
            
            for (int i = 0; i < count; i++)
            {
                var uScene = SceneManager.GetSceneAt(i);
                if (uScene.buildIndex == index)
                {
                    Send($"UScene {sceneIndex} was already loaded...");
                    scene = uScene;
                    return true;
                }
            }
            
            return false;
        }
            

        private static UScene GetUScene(int index) =>
            SceneManager.GetSceneByBuildIndex(index);
        
        private static Message Send(string text, LogFormat worning = LogFormat.None) =>
            Messager.Send(m_Debug, "USceneHandler", text, worning);

    }
}