using System;
using UnityEngine.SceneManagement;

public class SceneController: IConfigurable
{
    public event Action SceneLoaded;
    public event Action SceneUnloaded;

    public event Action SceneActivated;


    private SceneIndex m_SceneActive;

    public SceneController()
    {
        Configure(new SceneControllerConfig());

    }


    public void Configure(IConfig config)
    {

    }


    public void Init()
    {
        SetActive(SceneIndex.Core);
    }

    public void Dispose()
    {

    }
    
    

    public void Load(SceneIndex scene)
    {
        SceneManager.LoadSceneAsync((int)scene, LoadSceneMode.Additive);
        Send ($"{scene} loaded...");
    }

    public void Unload(SceneIndex scene)
    {
        
        SceneManager.UnloadSceneAsync((int)scene);
        Send ($"{scene} unloaded...");
        SetActive(SceneIndex.Core);
    }

    public void Reload(SceneIndex scene)
    {
        Unload(scene);
        Load(scene);
        Send ($"{scene} reloaded...");
    }

    
    private void SetActive(SceneIndex scene) 
    {
        m_SceneActive = scene;
        
        SceneActivated?.Invoke();
        Send ($"{scene} activated...");


    }

    private string Send(string text, bool worning = false)
    { 
        return Messager.Send(this, true, text, worning);
    }
}


public class SceneControllerConfig: IConfig
{
    public SceneControllerConfig()
    {

    }
}


