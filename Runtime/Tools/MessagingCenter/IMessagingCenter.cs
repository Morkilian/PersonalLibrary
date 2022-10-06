using System;

namespace Morkilian.Helper
{
    public interface IMessagingCenter
    {
        void Subscribe<TSender, TArgs>(object subscriber, string message, Action<TSender, TArgs> callback, TSender source = default)
            where TSender : class;
        void Subscribe<TSender, TArgs1, TArgs2>(object subscriber, string message, Action<TSender, TArgs1, TArgs2> callback, TSender source = default)
            where TSender : class;
        void Subscribe<TSender>(object subscriber, string message, Action<TSender> callback, TSender source = default)
            where TSender : class;
        void Send<TSender, TArgs>(TSender sender, string message, TArgs args)
            where TSender : class;
        void Send<TSender, TArgs1, TArgs2>(TSender sender, string message, TArgs1 args1, TArgs2 args2)
            where TSender : class;

        void Send<TSender>(TSender sender, string message)
            where TSender : class;

        void UnsubscribeAll();
        void Unsubscribe<TSender>(object subscriber, string message, TSender subscriptionType = null)
            where TSender : class;
        void Unsubscribe<TSender>(object subscriber, string message, Type argsType, TSender subscriptionType = null)
            where TSender : class;
        void Unsubscribe<TSender>(object subscriber, string message, Type argsType1, Type argsType2, TSender subscriptionType = null)
            where TSender : class;
    }
}
