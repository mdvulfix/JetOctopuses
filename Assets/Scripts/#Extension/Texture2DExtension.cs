using UnityEngine;

public static class Texture2DExtension
{    
    
    public static Texture2D Set(this Texture2D instance, Color color)
    {       
        for (int x = 0; x <= instance.width; x++)
            for (int y = 0; y <= instance.height; y++)
                instance.SetPixel(x, y, color);
        
        instance.Apply();
        
        return instance;
    }

}
