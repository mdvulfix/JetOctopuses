using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class MapUI : MonoBehaviour
{

    private RectTransform m_Transform;

    [SerializeField] private float m_Width = 100;
    [SerializeField] private float m_Hight =100;

    private void Awake() 
    {
        m_Transform = GetComponent<RectTransform>();
        SetSize(m_Width, m_Hight);
    }
    
    private void Update() 
    {
        
        
    }

    
    private void Start() 
    {
        m_Transform.sizeDelta = new Vector3(m_Width, m_Hight, 0);
    }
    
    
    public void SetSize(float width, float height)
    {
        m_Width = width;
        m_Hight = height;

    }


}
