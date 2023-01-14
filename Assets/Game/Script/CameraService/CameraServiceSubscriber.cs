using UnityEngine;

namespace Script.CameraService
{
[RequireComponent(typeof(Camera))]
public class CameraServiceSubscriber : MonoBehaviour
{
    [SerializeField] private CameraType _cameraType;
    private ICameraServiceSubscription _cameraServiceSubscription;

    
    private void Awake()
    {
        var thisCamera = GetComponent<Camera>();
        _cameraServiceSubscription = CameraService.CameraSubscriptionInstance;
        _cameraServiceSubscription.SubscribeCamera(_cameraType, thisCamera);
    }

    private void OnDestroy()
    {
        _cameraServiceSubscription?.UnsubscribeCamera(_cameraType);
    }
}
}