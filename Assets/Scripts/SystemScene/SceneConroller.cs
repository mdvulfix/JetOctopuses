using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneIndex
{
    Core,
    Menu,
    Runtime,
    Totals
}


public class SceneController
{
    
    private ScreenController m_ScreenController;
    private SceneIndex m_ActiveScene;

    public SceneController()
    {
        m_ScreenController = new ScreenController();
    }

    public SceneController(ScreenController screenController)
    {
        m_ScreenController = screenController;
    }



    public bool SceneLoad(SceneIndex scene)
    {
        SceneManager.LoadSceneAsync((int)scene, LoadSceneMode.Additive);
        m_ActiveScene = scene;
        return true;
    }

    public bool SceneUnload(SceneIndex scene)
    {
        
        SceneManager.UnloadSceneAsync((int)scene);
        m_ActiveScene = SceneIndex.Core;
        return true;
    }

    public bool SceneReload(SceneIndex scene)
    {
        if(SceneUnload(scene))
            return SceneLoad(scene);
        else   
            return false;
    }

    private void ScreenActivate<T>(bool active) 
        where T: MonoBehaviour, IScreen
    {
        m_ScreenController.AcrivateScreen<T>(active);

    }


}
