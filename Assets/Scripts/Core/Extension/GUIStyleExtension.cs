using UnityEngine;

public static class GUIStyleExtension
{
    public static GUIStyle Set(this GUIStyle instance,
                                    Texture2D background,
                                    Color textColor,
                                    int fontSize,
                                    TextAnchor alignment,
                                    GUIStyleState hover,
                                    GUIStyleState active)
    {
        instance.normal.background = background;
        instance.normal.textColor = textColor;
        instance.fontSize = fontSize;
        instance.alignment = alignment;
        instance.hover = hover;
        instance.active = active;
        
        return instance;
    }
}
