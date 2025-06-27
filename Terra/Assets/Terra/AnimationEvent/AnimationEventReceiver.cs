using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Terra.AnimationEvent
{
    public class AnimationEventReceiver : MonoBehaviour
    {
        [FormerlySerializedAs("animationEvents")] [SerializeField] List<AnimationEvent> _animationEvents = new();

        public void OnAnimationEventTriggered(string eventName)
        {
            AnimationEvent matchingEvent = _animationEvents.Find(se => se.eventName == eventName);
            matchingEvent?.onAnimationEvent?.Invoke();
        }
    }
}