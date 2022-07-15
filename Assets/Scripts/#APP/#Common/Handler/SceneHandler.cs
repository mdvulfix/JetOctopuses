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
        public static async Task USceneLoad(SceneIndex? sceneIndex)
        {
            if(GetUSceneLoaded(sceneIndex, out var uScene))
            {
                Send($"UScene {sceneIndex} was already loaded...");
                return;
            }

            var index = (int) sceneIndex;            
            var operation = SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);

            await TaskHandler.Run(() => 
                AwaitLoadingOperationComplete(operation),$"Await UScene {sceneIndex} loading operation complete...");
            
            Send($"UScene {sceneIndex} was successfully loaded...");
        }

        public static async Task USceneActivate(SceneIndex? sceneIndex)
        {
            if(GetUSceneLoaded(sceneIndex, out var uScene))
            {
                if(uScene.isLoaded == false)
                {
                    await TaskHandler.Run(() =>
                        AwaitUSceneLoading(uScene),$"Await UScene {sceneIndex} loading...", 1000f);
                }

                SceneManager.SetActiveScene(uScene);
                Send($"UScene {sceneIndex} was successfully activated...");
                return;
            }
            else
            {
                Send($"UScene {sceneIndex} not found. Activation failed!", LogFormat.Worning);
                await USceneLoad(sceneIndex);
                await USceneActivate(sceneIndex);
            }
        }

        public static async Task USceneUnload(SceneIndex? sceneIndex)
        {
            await Task.Delay(1);
            Send($"UScene {sceneIndex} was unloaded... Not implemented!", LogFormat.Worning);
        }

        public static async Task USceneReload(SceneIndex? sceneIndex)
        {
            await Task.Delay(1);
            Send($"UScene {sceneIndex} was eloaded... Not implemented!", LogFormat.Worning);
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