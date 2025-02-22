namespace Morkilian.Helper
{
    public interface IEvent
    {
        void SubscribeEvents();
        void UnsubscribeEvents();
    }
}