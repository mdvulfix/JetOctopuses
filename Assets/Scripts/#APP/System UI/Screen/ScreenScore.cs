using System;

namespace APP.Screen
{
    [Serializable]
    public class ScreenScore : ScreenModel<ScreenScore>, IScreen
    {

        //[SerializeField] private ButtonScoreMenu m_Menu;
        //[SerializeField] private ButtonScoreExit m_Exit;

        public override void Init()
        {
            var buttons = new IButton[]
            {
                //m_Menu,
                //m_Exit
            };

            var config = new ScreenConfig(this, buttons);
            
            base.Configure(config);
            base.Init();
        }
    }

}