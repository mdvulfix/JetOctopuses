using System;

namespace APP.Screen
{
    [Serializable]
    public class ScreenLoading : ScreenModel<ScreenLoading>, IScreen
    {
        protected override void Init()
        {
            var buttons = new IButton[]
            {

            };

            var config = new ScreenConfig(this, buttons);
            
            base.Configure(config);
            base.Init();
        }

    }
}