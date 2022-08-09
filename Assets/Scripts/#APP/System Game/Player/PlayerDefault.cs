using UnityEngine;

namespace APP.Game
{
    public class PlayerDefault : PlayerModel<PlayerDefault>, IPlayer
    {
        [SerializeField] private int m_Health;

    }

}
