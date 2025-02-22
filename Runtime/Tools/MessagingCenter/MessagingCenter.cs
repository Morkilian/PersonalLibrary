using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Morkilian.Helper
{
    public class MessagingCenter : MonoBehaviour, IMessagingCenter
    {
        private static IMessagingCenter _instance;

        public static IMessagingCenter Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<MessagingCenter>();

                    if (_instance == null)
                    {
                        var singleton = new GameObject();
                        _instance = singleton.AddComponent<MessagingCenter>();
                        singleton.name = "MessagingCenter";
                    }
                }

                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = Instance;
                DontDestroyOnLoad(this);
            }
        }

        private struct SubscriptionKey
        {
            public string SubscriptionType { get; set; }
            public string Message { get; set; }
            public string ArgType1 { get; set; }
            public string ArgType2 { get; set; }
        }

        private class Subscription
        {
            public object Subscriber { get; }
            public object Target { get; }
            public object Args1 { get; }
            public object Args2 { get; }
            public MethodInfo MethodInfo { get; }

            public Subscription(object subscriber, object target, MethodInfo methodInfo)
            {
                Subscriber = subscriber;
                Target = target;
                MethodInfo = methodInfo;
            }

            /*public void InvokeCallBack(object sender, object args)
            {
                MethodInfo.Invoke(Target, MethodInfo.GetParameters().Length == 1 ? new[] { sender } : new[] { sender, args });
            }*/

            public void InvokeCallBack(object sender, object args, object args2)
            {
                switch (MethodInfo.GetParameters().Length)
                {
                    case 1:
                        MethodInfo.Invoke(Target, new[] { sender });
                        break;

                    case 2:
                        MethodInfo.Invoke(Target, new[] { sender, args });
                        break;

                    case 3:
                        MethodInfo.Invoke(Target, new[] { sender, args, args2 });
                        break;
                }
            }
        }

        private Dictionary<SubscriptionKey, List<Subscription>> subscriptions;

        public MessagingCenter()
        {
            subscriptions = new Dictionary<SubscriptionKey, List<Subscription>>();
        }
        public void Send<TSender, TArgs>(TSender sender, string message, TArgs args) where TSender : class
        {
            InnerSend(message, typeof(TSender), sender, args, typeof(TArgs));
        }

        public void Send<TSender, TArgs1, TArgs2>(TSender sender, string message, TArgs1 args1, TArgs2 args2) where TSender : class
        {
            InnerSend(message, typeof(TSender), sender, args1, typeof(TArgs1), args2, typeof(TArgs2));
        }

        public void Send<TSender>(TSender sender, string message) where TSender : class
        {
            InnerSend(message, typeof(TSender), sender);
        }

        public void Subscribe<TSender, TArgs>(object subscriber, string message,
            Action<TSender, TArgs> callback, TSender source = null) where TSender : class
        {
            InnerSubscribe(subscriber, message, callback.Target, callback.GetMethodInfo(), typeof(TSender), typeof(TArgs));
        }

        public void Subscribe<TSender, TArgs1, TArgs2>(object subscriber, string message,
            Action<TSender, TArgs1, TArgs2> callback, TSender source = null) where TSender : class
        {
            InnerSubscribe(subscriber, message, callback.Target, callback.GetMethodInfo(), typeof(TSender), typeof(TArgs1), typeof(TArgs2));
        }

        public void Subscribe<TSender>(object subscriber, string message, Action<TSender> callback, TSender source = null) where TSender : class
        {
            InnerSubscribe(subscriber, message, callback.Target, callback.GetMethodInfo(), typeof(TSender));
        }

        public void UnsubscribeAll()
        {
            subscriptions.Clear();
        }

        public void Unsubscribe<TSender>(object subscriber, string message, TSender subscriptionType = null) where TSender : class
        {
            InnerUnsubscribe(typeof(TSender), subscriber, message);
        }

        public void Unsubscribe<TSender>(object subscriber, string message, Type argsType, TSender subscriptionType = null) where TSender : class
        {
            InnerUnsubscribe(typeof(TSender), subscriber, message, argsType); ;
        }

        public void Unsubscribe<TSender>(object subscriber, string message, Type argsType1, Type argsType2, TSender subscriptionType) where TSender : class
        {
            InnerUnsubscribe(typeof(TSender), subscriber, message, argsType1, argsType2);
        }

        private void InnerSubscribe(object subscriber, string message, object target,
            MethodInfo methodInfo, Type senderType, Type argType = null, Type argType2 = null)
        {
            var subscriptionKey = new SubscriptionKey()
            {
                Message = message,
                SubscriptionType = senderType.Name,
                ArgType1 = argType?.Name,
                ArgType2 = argType2?.Name
            };

            var subscriptionValue = new Subscription(subscriber, target, methodInfo);

            if (subscriptions.ContainsKey(subscriptionKey))
            {
                subscriptions[subscriptionKey].Add(subscriptionValue);
            }
            else
            {
                subscriptions.Add(subscriptionKey, new List<Subscription>() { subscriptionValue });
            }
        }

        private void InnerSend(string message, Type subscribtionType, object sender, object args1 = null, Type argType1 = null, object args2 = null, Type argType2 = null)
        {
            var subscriptionKey = new SubscriptionKey()
            {
                Message = message,
                SubscriptionType = subscribtionType.Name,
                ArgType1 = argType1?.Name,
                ArgType2 = argType2?.Name
            };

            if (subscriptions.ContainsKey(subscriptionKey))
            {
                List<Subscription> subs = subscriptions[subscriptionKey];

                for (int i = 0; i < subs.Count; i++)
                {
                    subs[i].InvokeCallBack(sender, args1, args2);
                }
            }
        }

        private void InnerUnsubscribe(Type subscriptionType, object subscriber, string message, Type argsType1 = null, Type argsType2 = null)
        {
            var subscriptionKey = new SubscriptionKey()
            {
                Message = message,
                SubscriptionType = subscriptionType.Name,
                ArgType1 = argsType1?.Name,
                ArgType2 = argsType2?.Name
            };

            if (subscriptions.ContainsKey(subscriptionKey))
            {
                List<Subscription> subs = subscriptions[subscriptionKey];

                for (int i = 0; i < subs.Count; i++)
                {
                    if (subs[i].Subscriber.Equals(subscriber))
                    {
                        subs.RemoveAt(i);
                    }
                }

                if (subs.Count == 0)
                {
                    subscriptions.Remove(subscriptionKey);
                }
            }
        }
    }
}
