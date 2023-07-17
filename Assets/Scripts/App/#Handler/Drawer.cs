using System;
using UnityEngine;

namespace Core
{
    public static class Drawer
    {
        private static int m_ButtonWidth = 200;
        private static int m_ButtonHeight = 100;
        private static int m_ButtonLeft = 25;
        private static int m_ButtonTop = 25;

        private static Color m_ButtonColorOnNormalState = Color.black;
        private static Color m_ButtonColorOnHoverState = Color.grey;
        private static Color m_ButtonColorOnActiveState = Color.cyan;

        private static Color m_ButtonTextColorOnNormalState = Color.white;
        private static Color m_ButtonTextColorOnHoverState = Color.white;
        private static Color m_ButtonTextColorOnActiveState = Color.black;

        private static Texture2D m_ButtonTextureOnNormalState =
            new Texture2D(m_ButtonWidth, m_ButtonHeight).Set(m_ButtonColorOnNormalState);

        private static Texture2D m_ButtonTextureOnHoverState =
            new Texture2D(m_ButtonWidth, m_ButtonHeight).Set(m_ButtonColorOnHoverState);

        private static Texture2D m_ButtonTextureOnActiveState =
            new Texture2D(m_ButtonWidth, m_ButtonHeight).Set(m_ButtonColorOnActiveState);

        private static GUIStyleState m_ButtonGUIStyleOnHoverState =
            new GUIStyleState().Set(m_ButtonTextureOnHoverState,
                                    m_ButtonTextColorOnHoverState);

        private static GUIStyleState m_ButtonGUIStyleOnActiveState =
            new GUIStyleState().Set(m_ButtonTextureOnActiveState,
                                    m_ButtonTextColorOnActiveState);

        private static GUIStyle m_ButtonGUIStyle =
            new GUIStyle().Set(m_ButtonTextureOnNormalState,
                               m_ButtonTextColorOnNormalState,
                               30,
                               TextAnchor.MiddleCenter,
                               m_ButtonGUIStyleOnHoverState,
                               m_ButtonGUIStyleOnActiveState);

        private static Rect m_ButtonRect =
            new Rect(m_ButtonLeft,
                     m_ButtonTop,
                     m_ButtonWidth,
                     m_ButtonHeight);



        public static void Button(Action action, string label)
        => Button(action, label, m_ButtonRect, 0, 0, m_ButtonGUIStyle);

        public static void Button(Action action, string label, int leftOffset = 0, int topOffset = 0)
        => Button(action, label, m_ButtonRect, leftOffset, topOffset, m_ButtonGUIStyle);

        public static void Button(Action action, string label, Rect position, int leftOffset, int topOffset, GUIStyle style = null)
        {
            if (GUI.Button(new Rect(position.x + leftOffset, position.y + topOffset, position.width, position.height), label, style))
                action.Invoke();
        }

    }
}