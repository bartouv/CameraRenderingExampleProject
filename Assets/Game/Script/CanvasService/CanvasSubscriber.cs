using System.Collections.Generic;
using UnityEngine;

namespace Script.CanvasService
{
    [RequireComponent(typeof(Canvas))]
public class CanvasSubscriber : MonoBehaviour
{
    [SerializeField] private List<CanvasTagType> _canvasTagTypes;
    
    private ICanvasServiceSubscription _canvasServiceSubscription;
    private Canvas _canvas;
    private CanvasSubscriberData _canvasSubscriberData;

    
    private void Awake()
    {
        _canvas = gameObject.GetComponent<Canvas>();
        _canvasServiceSubscription = CanvasService.CanvasServiceSubscriptionInstance;
        _canvasSubscriberData = new CanvasSubscriberData(_canvas, _canvasTagTypes);
        _canvasServiceSubscription.SubscribeCanvas(_canvasSubscriberData);
    }

    private void OnDestroy()
    {
        _canvasServiceSubscription?.UnsubscribeCanvas(_canvasSubscriberData);
    }
}
}