using System;
using System.Collections.Generic;
using UnityEngine;

namespace Script.CameraService
{
    public class CameraService : ICameraServiceSubscription, ICameraService
    {
        private readonly Dictionary<CameraType, Camera> _cameras = new();
        private readonly Dictionary<CameraType, int> _camerasCullingMaks = new();
       
        private static readonly Lazy<CameraService> LazyInstance = new(() => new CameraService());    
     
        private CameraService()    
        {    
        }    

        public static ICameraServiceSubscription CameraSubscriptionInstance => LazyInstance.Value;
        public static ICameraService CameraServiceInstance => LazyInstance.Value;
        

        public void SubscribeCamera(CameraType type, Camera camera)
        {
            bool cameraSubscribed = _cameras.ContainsKey(type);

            if (cameraSubscribed)
            {
                Debug.LogError("Camera already subscribed!");
            }
            else
            {
                _cameras.Add(type, camera);
                _camerasCullingMaks.Add(type, camera.cullingMask);
            }
        }

        public void UnsubscribeCamera(CameraType type)
        {
            bool cameraSubscribed = _cameras.ContainsKey(type);

            if (cameraSubscribed)
            {
                _cameras.Remove(type);
                _camerasCullingMaks.Remove(type);
            }
            else
            {
                Debug.LogError("Camera not subscribed!");
            }
        }

public void TurnCameraCullingMasksOn(CameraType type)
{
    if (!_cameras.TryGetValue(type, out var camera))
    {
        Debug.LogError("camera not found");

        return;
    }

    camera.cullingMask = _camerasCullingMaks[type];
}

public void TurnCameraCullingMasksOff(CameraType type)
{
    if (!_cameras.TryGetValue(type, out var camera))
    {
        Debug.LogError("camera not found");

        return;
    }

    camera.cullingMask = 0;
}

        public void TurnCameraRenderingOn(CameraType type)
        {
            if (!_cameras.TryGetValue(type, out var camera))
            {
                Debug.LogError("camera not found");

                return;
            }

            camera.enabled = true;
        }

        public void TurnCameraRenderingOff(CameraType type)
        {
            if (!_cameras.TryGetValue(type, out var camera))
            {
                Debug.LogError("camera not found");
                return;
            }

            camera.enabled = false;
        }

        public Vector3 WorldToScreenPoint(CameraType type, Vector3 worldPoint)
        {
            if (_cameras.TryGetValue(type, out var camera))
            {
                return camera.WorldToScreenPoint(worldPoint);
            }

            Debug.LogError("camera not found");

            return Vector3.zero;
        }
        
        public Vector3 WorldToViewPortPoint(CameraType type, Vector3 worldPoint)
        {
            if (!_cameras.TryGetValue(type, out var camera))
            {
                Debug.LogError("camera not found");

                return Vector3.zero;
            }

            return camera.WorldToViewportPoint(worldPoint);
        }

        public Vector3 ScreenToWorldPoint(CameraType type, Vector3 screenPoint)
        {
            if (_cameras.TryGetValue(type, out var camera))
            {
                return camera.ScreenToWorldPoint(screenPoint);
            }

            Debug.LogError("camera not found");

            return Vector3.zero;
        }

        public Vector3 ScreenToViewPort(CameraType type, Vector3 screenPoint)
        {
            if (_cameras.TryGetValue(type, out var camera))
            {
                return camera.ScreenToViewportPoint(screenPoint);
            }

            Debug.LogError("camera not found");

            return Vector3.zero;
        }

        public bool ScreenPointToRay(CameraType type, out RaycastHit hit)
        {
            if (_cameras.TryGetValue(type, out var camera))
            {
                return Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity);
            }

            Debug.LogError("camera not found");
            hit = new RaycastHit();

            return false;
        }

        public bool ScreenCenterToRay(CameraType type, out RaycastHit hit)
        {
            if (_cameras.TryGetValue(type, out var camera))
            {
                return Physics.Raycast(camera.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0)),
                    out hit, Mathf.Infinity);
            }

            Debug.LogError("camera not found");
            hit = new RaycastHit();

            return false;
        }

        public void SetCanvasCamera(CameraType type, Canvas canvas)
        {
            if (!_cameras.TryGetValue(type, out var camera))
            {
                Debug.LogError("camera not found");
            }

            canvas.worldCamera = camera;
        }

        public Vector2 ScreenPointToCanvasInCameraSpacePoint(RectTransform rect, Vector2 screenPoint, CameraType cameraType)
        {
            bool wasSuccess = RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, screenPoint, _cameras[cameraType], out var localPoint);

            return wasSuccess ? localPoint : Vector2.zero;
        }

        public float GetAspectRatio(CameraType type)
        {
            if (_cameras.TryGetValue(type, out var camera))
            {
                return camera.aspect;
            }

            Debug.LogError("camera not found");
            return -1;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public Vector3 GetCameraPosition(CameraType type)
        {
            if (_cameras.TryGetValue(type, out var camera))
            {
                return camera.transform.position;
            }

            Debug.LogError("camera not found");

            return Vector3.zero;
        }

        public Vector3 GetCameraForward(CameraType type)
        {
            if (_cameras.TryGetValue(type, out var camera))
            {
                return camera.transform.forward;
            }

            Debug.LogError("camera not found");

            return Vector3.zero;
        }

        public Vector3 GetCameraRight(CameraType type)
        {
            if (_cameras.TryGetValue(type, out var camera))
            {
                return camera.transform.right;
            }

            Debug.LogError("camera not found");

            return Vector3.zero;
        }

        public bool IsPointOnScreen(CameraType type, Vector3 point)
        {
            if (!_cameras.ContainsKey(type))
            {
                Debug.LogError("camera not found");

                return false;
            }

            var screenPoint = _cameras[type].WorldToScreenPoint(point);

            bool isOnScreen = screenPoint.x > 0f
                              && screenPoint.x < Screen.width
                              && screenPoint.y > 0f
                              && screenPoint.y < Screen.height
                              && screenPoint.z > 0f;

            return isOnScreen;
        }
    }
}