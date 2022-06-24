using System;
using UnityEngine;
using APP.Player;

namespace APP.Button
{
    [Serializable]
    public class ButtonMenuLevel : ButtonModel<ButtonMenuLevel>, IButton
    {
        [SerializeField] private PlayerAction m_PlayerAction;
        //[SerializeField] private LevelIndex m_LevelIndex;

        protected override void Init()
        {
            if (m_PlayerAction == PlayerAction.None)
            {
                Send("Player action not assigned!", true);
                return;
            }

            //if (m_LevelIndex == LevelIndex.None)
            //{
            //    Send ("Level not assigned!", true);
            //    return;
            //}

            //var info = new ActionLevelLoadInfo (m_PlayerAction, m_LevelIndex);
            //var config = new ButtonConfig<ActionInfo> (this, info);

            //Configure (config);
            //base.Init ();
        }

    }

}