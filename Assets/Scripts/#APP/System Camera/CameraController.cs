using System;
using UnityEngine;
using UCamera = UnityEngine.Camera;

using SERVICE.Handler;

namespace APP.Camera
{

    public class CameraController : MonoBehaviour
    {

        public ICamera Camera { get; private set; }

        private void Awake() 
        {
            var fieldOfView = 40;
            var CameraConfig =  new CameraConfig(fieldOfView);
            Camera = new CameraFollowPlayer(CameraConfig);

        }
        
        
        
        public void Follow()
        {

        }

        public void ZoomIn()
        {

        }

        public void ZoomOut()
        {

        }
    }

    public class CameraFollowPlayer : ICamera, IConfigurable
    {
        private CameraConfig m_Config;
        
        private int m_FieldOfView = 40;
        private bool m_Orthographic = true;
        private Color m_BackgroundColor = Color.blue;


        public CameraFollowPlayer() { }
        public CameraFollowPlayer(params object[] param) =>
            Configure(param);


        public UCamera UCamera {get; private set; }

        public bool IsConfigured {get; private set; }
        public bool IsInitialized {get; private set; }

        public event Action Configured;
        public event Action Initialized;
        public event Action Disposed;

        public void Configure(params object[] param)
        {
            if (IsConfigured == true)
            {
                //Send($"{this.GetName()} was already configured. The current setup has been aborted!", LogFormat.Worning);
                return;
            }
                
            if (param != null && param.Length > 0)
            {
                foreach (var obj in param)
                {
                    if (obj is IConfig)
                    {
                        m_Config = (CameraConfig) obj;

                        m_FieldOfView = m_Config.FieldOfView;

                        //Send($"{obj.GetName()} setup.");
                    }
                }
            }
            else
            {
                //Send("Params are empty. Config setup aborted!", LogFormat.Worning);
            }
            
                
            var objCamera = GameObjectHandler.CreateGameObject("PlayerCamera");
            objCamera.SetActive(true);
            UCamera = GameObjectHandler.SetComponent<UCamera>(objCamera);

            
            UCamera.clearFlags = CameraClearFlags.SolidColor;
            UCamera.backgroundColor = m_BackgroundColor;
            UCamera.orthographic = m_Orthographic;
            UCamera.fieldOfView = m_FieldOfView;
        }

        public void Init()
        {

        }

        public void Dispose()
        {

        }


    }

    public struct CameraConfig: IConfig
    {
        public CameraConfig(int fieldOfView)
        {
            FieldOfView = fieldOfView;
        }

        public int FieldOfView {get; private set;}
    }
}

namespace APP
{
    public interface ICamera
    {



    }
}