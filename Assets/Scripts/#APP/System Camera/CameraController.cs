using UnityEngine;

namespace APP.Camera
{

    public class CameraController : MonoBehaviour
    {

        public ICamera Camera { get; private set; }

        private void Awake ()
        {
            
            
            //var fieldOfView = 40;
            //var CameraConfig = new CameraConfig (fieldOfView);
            //Camera = new CameraFollowPlayer (CameraConfig);

        }

        public void Follow ()
        {

        }

        public void ZoomIn ()
        {

        }

        public void ZoomOut ()
        {

        }
    }

}