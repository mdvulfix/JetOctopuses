using System;
using APP.Button;

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

            Configure(new ScreenConfig(this, buttons));
            base.Init();
        }

    }
}