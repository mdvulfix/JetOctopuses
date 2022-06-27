using System;
using UnityEngine;
using APP.Player;

namespace APP.Button
{
    [Serializable]
    public class ButtonScoreExit : ButtonModel<ButtonScoreExit>, IButton
    {
        [SerializeField] private PlayerAction m_PlayerAction;

        protected override void Init()
        {
            if (m_PlayerAction == PlayerAction.None)
            {
                Send("Player action not assigned!", true);
                return;
            }

            var info = new InstanceInfo(this);
            var config = new ButtonConfig(info);

            base.Configure(config);
            base.Init();
        }
    }

}