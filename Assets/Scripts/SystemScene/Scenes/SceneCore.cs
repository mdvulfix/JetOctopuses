using UnityEngine;

public class SceneCore : AScene
{
    private SceneIndex m_SceneIndex;

    public SceneIndex SceneIndex => m_SceneIndex;

    
    protected override void OnAwake()
    {
        m_SceneIndex = SceneIndex.Core;
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
