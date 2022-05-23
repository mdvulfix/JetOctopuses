using UnityEngine;

public class Map
{

    private int m_Hight;
    private int m_Width;
    private GameObject m_MapField;

    public Map()
    {
        m_Hight = 1480;
        m_Width = 720;
    }
    
    public Map(int hight, int width)
    {
        m_Hight = hight;
        m_Width = width;
    }

    public void Init()
    {
        CreateLayer("SpaceField", 0);
        CreateLayer("Stars", 1);



    }

    private void CreateLayer(string name, int layer)
    {
        m_MapField = CreateObject("SpaceField" + " " + m_Width.ToString() + ":" + m_Hight.ToString(), m_MapField);
        var sr = m_MapField.AddComponent<SpriteRenderer>();
        sr.sprite = CreateSprite(1, 1, Color.white);
        sr.color = Color.black;
        sr.sortingOrder = layer;
    } 

    public GameObject CreateObject(string name, GameObject parent = null)
    {
        var _obj = new GameObject(name);
        if (parent)
            _obj.transform.parent = parent.transform;
        
        return _obj;
    }   

    public Sprite CreateSprite(int width = 1, int height = 1, Color color = default(Color))
    {
        var texture = new Texture2D(width, height);

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                texture.SetPixel(x, y, color);

        texture.Apply();
        texture.filterMode = FilterMode.Point;

        Rect rect = new Rect(0, 0, width, height);
        return Sprite.Create(texture, rect, Vector2.zero, 1, 0, SpriteMeshType.FullRect);

    }         
    
}
