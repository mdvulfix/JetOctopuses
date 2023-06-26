using UnityEngine;

public static class GUIStyleStateExtension
{    
    
    public static GUIStyleState Set(this GUIStyleState instance, Texture2D background, Color textColor)
    {
            instance.background = background;
            instance.textColor = textColor;

            return instance;

    }



}
