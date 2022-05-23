using UnityEngine;

public class SceneRuntime : AScene
{
    private SceneIndex m_SceneIndex;
    private Map m_Map;

    public SceneIndex SceneIndex => m_SceneIndex;

    protected override void OnAwake()
    {
        m_SceneIndex = SceneIndex.Runtime;
    
        m_Map = new Map();
        m_Map.Init();
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
