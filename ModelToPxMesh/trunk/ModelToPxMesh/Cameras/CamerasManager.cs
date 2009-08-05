using System;
using System.Collections.Generic;

namespace XnaPhysXLoader.Cameras
{
    public enum CameraTypes
    {
        GodCamera,
        FPSCamera,
        ChaseCamera
    }

    public sealed class CamerasManager
    {
        //Singleton - Thread-Safe
        private static readonly CamerasManager instance = new CamerasManager();
        private readonly Dictionary<string, Camera> _cameras = new Dictionary<string, Camera>();
        private Camera _activeCamera;
        private Dictionary<string, Camera>.Enumerator _camerasIter;


        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit

        private CamerasManager()
        {
        }

        public Camera PlayableCharCamera { get; set; }

        public static CamerasManager Instance
        {
            get { return instance; }
        }


        public void OnGameRestarted()
        {
            _cameras.Clear();
        }

        public void AddCamera(Camera camera)
        {
            camera.Enabled = false;
            _cameras.Add(camera.Id, camera);
            _camerasIter = _cameras.GetEnumerator();
            if (_cameras.Count == 1)
                _activeCamera = camera;
        }

        public void RemoveCamera(Camera camera)
        {
            if (_cameras.Count == 1)
                throw new Exception("Only exists one camera, without cameras this game cant be a game");

            _cameras.Remove(camera.Id);
            _camerasIter = _cameras.GetEnumerator();
        }

        public void Activate(Camera camera)
        {
            _activeCamera.Enabled = false;
            _activeCamera = _cameras[camera.Id];
            _activeCamera.Enabled = true;
            //SceneGraph.Instance.Camera = _activeCamera;
        }

        public void ChangeCamera()
        {
            if (_camerasIter.MoveNext())
            {
                if (_activeCamera != null)
                    _activeCamera.Enabled = false;
                _activeCamera = _camerasIter.Current.Value;
                _activeCamera.Enabled = true;
            }
            else if (_cameras.Count > 0)
            {
                _camerasIter = _cameras.GetEnumerator();
                _camerasIter.MoveNext();
                _activeCamera.Enabled = false;
                _activeCamera = _camerasIter.Current.Value;
                _activeCamera.Enabled = true;
            }
            else
                throw new Exception("No Cameras Exception");

            //SceneGraph.Instance.Camera = _activeCamera;
        }

        public Camera GetActiveCamera()
        {
            return _activeCamera;
        }

        public void ActivatePlayableCharCamera()
        {
            Activate(PlayableCharCamera);
        }
    }
}