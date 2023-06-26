using System;

namespace App.Map
{
    public class MapGenerator
    {
        private int m_Width;
        private int m_Hight;
        private int m_Layer;
        //private SceneObject m_;

        public MapGenerator()
        {

        }

        /*
        private void CreateLayer(string name, int layer)
        {
            m_Layer = CreateObject("SpaceField" + " " + m_Width.ToString() + ":" + m_Hight.ToString(), m_MapField);
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
        */

    }

}