using System;
using Core;
using Core.Signal;

/*
namespace App.Signal
{
    public class SignalSceneActivate : SignalModel<SignalSceneActivate>, ISignal
    {

        public SignalSceneActivate(IScene scene) => Configure(scene);
        public SignalSceneActivate(IConfig config) => Configure(config);

        public IScene Scene { get; private set; }

        public event Action<IScene> SceneRequied;

        public void Configure(IScene scene)
        {
            Scene = scene;

            var config = new SignalConfig(this);
            base.Configure(config);
        }

        public override void Execute()
        {

            SceneRequied?.Invoke(Scene);
        }
    }

}
*/