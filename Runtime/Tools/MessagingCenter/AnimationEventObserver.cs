using UnityEngine;

namespace Morkilian.Helper
{
    [RequireComponent(typeof(Animator))]
    public class AnimationEventObserver : MonoBehaviour
    {
        public Animator Anm { get; private set; }
        public bool sendEvent = true;
        public const string EVENT_NAME = "TRIGGER_EVENT";
        /// <summary>
        /// HashCode used to compare one animator with another. Value will stay 0 if this object has no animator
        /// </summary>
        public int AnmHashCode { get; private set; } = 0;
        private void Start()
        {
            Anm = GetComponent<Animator>();
            if (Anm != null)
                AnmHashCode = Anm.GetHashCode();
        }
        public void TriggerEvent(string message)
        {
            if (sendEvent)
            {
                MessagingCenter.Instance.Send(this, EVENT_NAME, message);
            }
        }
    }
}
