using UnityEngine;
using Core;
using Core.Factory;

//using App.Signal;


namespace Core.State
{

    public class StateMenu : StateModel, IState
    {
        //private SignalSceneActivate m_SceneMenuActivate;
        //private SignalStateSet m_StateMenuSet;

        public StateMenu() { }
        public StateMenu(params object[] args)
            => Init(args);


        public override void Init(params object[] args)
        {
            if (args.Length > 0)
            {
                base.Init(args);
                return;
            }

            // CONFIGURE BY DEFAULT //


            //var signals = new List<ISignal>();
            //signals.Add(m_SceneMenuActivate = new SignalSceneActivate(СacheProvider<SceneMenu>.Get()));
            //signals.Add(m_StateMenuSet = new SignalStateSet(СacheProvider<StateMenu>.Get()));

            //var config = new StateConfig(this, signals.ToArray());
            //base.Init(config);



            var config = new StateConfig();
            base.Init(config);
            Debug.Log($"{this.GetName()} was configured by default!");
        }



        public override void Enter()
        {
            Debug.Log($"{this.GetName()} enter...");
            //await Task.Delay(1);
            //return null;
        }

        public override void Run()
        {
            Debug.Log($"{this.GetName()} run...");
            //Send("System start loading...");
            //await Task.Delay(1);

            //var signal = new SignalSceneActivate<SceneCore>();
            //SignalSend(signal);

            //await Builder.Execute(new CoreBuildScheme());

            //m_SceneMenuActivate.Call();
            //return null;
        }

        public override void Fail()
        {
            Debug.Log($"{this.GetName()} fail...");
            //Send("System complete loading...");
            //await Task.Delay(1);
            //return null;
        }

        public override void Exit()
        {
            Debug.Log($"{this.GetName()} exit...");
            //Send("System complete loading...");
            //await Task.Delay(1);
            //return null;
        }

        // FACTORY //
        public static StateMenu Get(params object[] args)
            => Get<StateMenu>(args);
    }


    public partial class StateFactory : Factory<IState>
    {
        private StateMenu GetStateMenu(params object[] args)
        {
            var instance = new StateMenu(args);
            return instance;
        }
    }

}
