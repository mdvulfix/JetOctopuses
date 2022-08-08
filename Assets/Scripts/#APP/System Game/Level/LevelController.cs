using UnityEngine;
using APP.Camera;
using APP.Player;


namespace APP.Game
{
    public class LevelController : MonoBehaviour
    {

        [SerializeField] private CameraFollowPlayer m_Camera;
        [SerializeField] private PlayerDefault m_Player;
        
        
        public ICamera CameraFollow => m_Camera;
        public IPlayer PLayer => m_Player;

        private void Start() 
        {

            var position = new Vector2(0, 0);
            var fieldOfView = 40;
            var cameraConfig = new CameraConfig(position, fieldOfView);
            m_Camera.Configure(cameraConfig);
            m_Camera.Follow(() => m_Player.transform.position);

        }


    }
}