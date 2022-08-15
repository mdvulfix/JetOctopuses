using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class MapUI : MonoBehaviour
{
    [SerializeField] private float m_Width;
    [SerializeField] private float m_Height;

    private RectTransform m_Transform;

    
    public void SetSize(float width, float height)
    {
        m_Height = height;
        m_Width = width;
    }
    
    
    private void Awake() 
    {
        m_Transform = GetComponent<RectTransform>();
    }
    
    private void Update() 
    {
        m_Transform.sizeDelta = new Vector3(m_Width, m_Height, 0);
        
    }


}
