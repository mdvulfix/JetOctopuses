using System;
using APP.Button;

namespace APP.Screen
{
    [Serializable]
    public class ScreenLoading : ScreenModel<ScreenLoading>, IScreen
    {
        public override void Init()
        {
            var buttons = new IButton[]
            {

            };

            var info = new Instance(this);
            var config = new ScreenConfig(info, buttons);
            
            base.Configure(config);
            base.Init();
        }

    }
}