using UnityEngine;

namespace Script.CameraService
{
    public interface ICameraServiceSubscription
    {
        void SubscribeCamera(CameraType type, Camera camera);
        void UnsubscribeCamera(CameraType type);
    }
}