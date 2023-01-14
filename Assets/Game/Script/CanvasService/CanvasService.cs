using System;
using System.Collections.Generic;
using System.Linq;
using Script.CameraService;
using UnityEngine;
using CameraType = Script.CameraService.CameraType;

namespace Script.CanvasService
{
    public interface ICanvasService
    {
        void UnlockRenderLock(CanvasRenderLock canvasRenderLock);
        CanvasRenderLock StopRenderingAllExcept(List<CanvasTagType> canvasTagType);
    }

    public class CanvasService : ICanvasServiceSubscription, ICanvasService
    {
        private readonly List<CanvasSubscriberData> _canvasSubscribers = new();
        private readonly ICameraService _cameraService;
        private readonly Dictionary<CanvasTagType, HashSet<string>> _renderLocks = new();
        private List<CanvasTagType> _canvasTagTypes; 
     
        private static readonly Lazy<CanvasService> LazyInstance = new(() => new CanvasService());    
        
        public static ICanvasServiceSubscription CanvasServiceSubscriptionInstance => LazyInstance.Value;
        public static ICanvasService CanvasServiceInstance => LazyInstance.Value;

        private CanvasService()
        {
            _cameraService = CameraService.CameraService.CameraServiceInstance;
            foreach (var canvasTypeValue in Enum.GetValues(typeof(CanvasTagType)))
            {
                _renderLocks.Add((CanvasTagType) canvasTypeValue, new HashSet<string>());
            }
        }

        public void SubscribeCanvas(CanvasSubscriberData canvasSubscriberData)
        {
            if (_canvasSubscribers.Contains(canvasSubscriberData))
            {
                Debug.LogError($"canvas: {canvasSubscriberData.Canvas.name} is already subscibed");
                return;
            }

            _canvasSubscribers.Add(canvasSubscriberData);
            _cameraService.SetCanvasCamera(CameraType.Ui, canvasSubscriberData.Canvas);
        }

        public void UnsubscribeCanvas(CanvasSubscriberData canvasSubscriberData)
        {
            if (_canvasSubscribers.Contains(canvasSubscriberData))
            {
                _canvasSubscribers.Remove(canvasSubscriberData);
            }
        }
        
public CanvasRenderLock StopRenderingAllExcept(List<CanvasTagType> canvasTagTypes)
{
    var canvasesToLock = Enum.GetValues(typeof(CanvasTagType)).Cast<CanvasTagType>().ToList();

    foreach (var canvasTagType in canvasTagTypes)
    {
        canvasesToLock.Remove(canvasTagType);
    }

    var canvasLock = new CanvasRenderLock(canvasesToLock);

    foreach (var canvasType in canvasesToLock)
    {
        _renderLocks[canvasType].Add(canvasLock.Guid);
    }

    UpdateRenderingOfSubscribers();
    return canvasLock;
}

public void UnlockRenderLock(CanvasRenderLock canvasRenderLock)
{
    if (canvasRenderLock == null)
    {
        Debug.LogError("canvasRenderLock is null");
        return;
    }
    
    foreach (var canvasTagType in canvasRenderLock.CanvasesToLockList)
    {
        if (_renderLocks[canvasTagType] != null)
        {
            _renderLocks[canvasTagType].Remove(canvasRenderLock.Guid);
        }
    }

    UpdateRenderingOfSubscribers();
}

private void UpdateRenderingOfSubscribers()
{
    foreach (var canvasSubscriberData in _canvasSubscribers)
    {
        var shouldRender = true;
        foreach (var tag in canvasSubscriberData.Tags)
        {
            var tagHasLockOnIt = _renderLocks[tag].Count != 0;
            if (tagHasLockOnIt)
            {
                shouldRender = false;
            }
            else
            {
                //if subscriber has a tag which doesnt have a lock then the subscriber will be rendered.
                shouldRender = true;
                break;
            }
        }

        canvasSubscriberData.Canvas.enabled = shouldRender;
    }
}
    }

    public class CanvasRenderLock
    {
        public readonly string Guid;
        public readonly List<CanvasTagType> CanvasesToLockList;

        public CanvasRenderLock(List<CanvasTagType> canvasesToLockList)
        {
            Guid = System.Guid.NewGuid().ToString();
            CanvasesToLockList = canvasesToLockList;
        }
    }

    public class CanvasSubscriberData
    {
        public readonly Canvas Canvas;
        public readonly List<CanvasTagType> Tags;

        public CanvasSubscriberData(Canvas canvas, List<CanvasTagType> tags)
        {
            Canvas = canvas;
            Tags = tags;
        }
    }
}