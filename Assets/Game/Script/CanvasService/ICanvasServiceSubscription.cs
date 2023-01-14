namespace Script.CanvasService
{
    public interface ICanvasServiceSubscription
    {
        void SubscribeCanvas(CanvasSubscriberData canvasSubscriberData);
        void UnsubscribeCanvas(CanvasSubscriberData canvasSubscriberData);
    }
}