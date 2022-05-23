using UnityEngine;

public class SceneMenu : AScene
{
    private SceneIndex m_SceneIndex;

    public SceneIndex SceneIndex => m_SceneIndex;
    
    protected override void OnAwake()
    {
        m_SceneIndex = SceneIndex.Menu;
    }  
    
    protected override void OnEnable()
    {
        Add(this);
    }
     
    protected override void OnDisable()
    {
        Remove(this);
    }
}
