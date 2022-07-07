using System;
using UnityEngine;
using APP.Player;

namespace APP.UI
{
    [Serializable]
    public class ButtonSignIn : ButtonModel<ButtonSignIn>, IButton
    {
        [SerializeField] private PlayerAction m_PlayerAction;

        public override void Init()
        {
            if (m_PlayerAction == PlayerAction.None)
            {
                Send("Player action not assigned!", LogFormat.Worning);
                return;
            }


            var config = new ButtonConfig(this);

            base.Configure(config);
            base.Init();
        }
    }

}