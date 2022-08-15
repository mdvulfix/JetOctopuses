using System;
using UnityEngine;
using UCamera = UnityEngine.Camera;

using SERVICE.Handler;

namespace APP.Camera
{

    public class CameraFollowPlayer : MonoBehaviour, ICamera, IConfigurable
    {
        private CameraConfig m_Config;


        [SerializeField] private Vector3 m_Position;
        [SerializeField] private Vector3 m_PositionOffset;

        [SerializeField] private Vector3 m_MoveVelocity;
        [SerializeField] private float m_MoveSpeed;
        [SerializeField] private float m_MoveTime;

        [SerializeField] private float m_ZoomMin;
        [SerializeField] private float m_ZoomMax;
        [SerializeField] private float m_ZoomSpeed;

        private int m_FieldOfView = 40;
        private bool m_Orthographic = true;
        private Color m_BackgroundColor = Color.blue;

        public CameraFollowPlayer () { }
        public CameraFollowPlayer (params object[] param) =>
            Configure (param);


        public UCamera UCamera { get; private set; }

        public bool IsConfigured { get; private set; }
        public bool IsInitialized { get; private set; }

        public event Action Configured;
        public event Action Initialized;
        public event Action Disposed;

        public Func<Vector3> GetPositionFunc;
        public Func<float> GetZoomFunc;

        public void Configure (params object[] param)
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

                        m_Position = m_Config.Position;
                        m_FieldOfView = m_Config.FieldOfView;

                        //Send($"{obj.GetName()} setup.");
                    }
                }
            }
            else
            {
                //Send("Params are empty. Config setup aborted!", LogFormat.Worning);
            }

            m_Position.z = transform.position.z;
            transform.position = m_Position;

            m_MoveVelocity = Vector3.zero;
            m_MoveSpeed = 1000f;
            m_MoveTime = 1f;

            m_ZoomMin = 5f;
            m_ZoomMax = 15f;
            m_ZoomSpeed = 1f;

            //var objCamera = GameObjectHandler.CreateGameObject ("PlayerCamera");
            //objCamera.SetActive (true);
            //UCamera = GameObjectHandler.SetComponent<UCamera> (objCamera);

            UCamera = GetComponent<UCamera>();
            UCamera.orthographicSize = m_ZoomMin;
            //UCamera.clearFlags = CameraClearFlags.SolidColor;
            //UCamera.backgroundColor = m_BackgroundColor;
            //UCamera.orthographic = m_Orthographic;
            //UCamera.fieldOfView = m_FieldOfView;
        }

        public void Init ()
        {

        }

        public void Dispose ()
        {

        }

        private void Update() 
        {
            HandleMove();
            HandleZoom();
        }

        public void Follow(Func<Vector3> getPositionFunc) 
        {
            GetPositionFunc = getPositionFunc;
        }

        public void Zoom(Func<float> getZoomFunc) 
        {
            GetZoomFunc = getZoomFunc;
        }



        public void HandleMove() 
        {
            var currentPosition = transform.position;
            var newPosition = GetPositionFunc();
            var finalPosition =  newPosition + m_PositionOffset;
            if(Vector3.Distance(currentPosition, finalPosition) >= 10f)
                m_Position = finalPosition;
            else
                m_Position = Vector3.SmoothDamp(currentPosition, finalPosition, ref m_MoveVelocity, m_MoveTime, m_MoveSpeed * Time.deltaTime);

            transform.position = m_Position;
            //transform.LookAt(targetPosition);
        }

        public void HandleZoom() 
        {
            var zoomTarget = GetZoomFunc();
            
            if(zoomTarget < m_ZoomMin)
                zoomTarget = m_ZoomMin;
            else if(zoomTarget > m_ZoomMax)
                zoomTarget = m_ZoomMax;
            
            var zoomDifference = zoomTarget - UCamera.orthographicSize;
            UCamera.orthographicSize += zoomDifference * m_ZoomSpeed * Time.deltaTime;


        }

    }

    public struct CameraConfig : IConfig
    {
        public CameraConfig(Vector2 position, int fieldOfView)
        {

            FieldOfView = fieldOfView;
            Position = position;
        }

        public Vector2 Position { get; private set; }
        public int FieldOfView { get; private set; }
    }
}

namespace APP
{
    public interface ICamera
    {

    }
}