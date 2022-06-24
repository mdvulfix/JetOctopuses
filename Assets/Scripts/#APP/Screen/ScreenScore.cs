using System;
using UnityEngine;
using APP.Button;

namespace APP.Screen
{
    [Serializable]
    public class ScreenScore : ScreenModel<ScreenScore>, IScreen
    {

        [SerializeField] private ButtonScoreMenu m_Menu;
        [SerializeField] private ButtonScoreExit m_Exit;

        protected override void Init()
        {
            var buttons = new IButton[]
            {
                m_Menu,
                m_Exit
            };

            Configure(new ScreenConfig(this, buttons));
            base.Init();
        }
    }

}