using System;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Terra.AnimationEvent
{
    [Serializable]
    public class AnimationEvent
    {
        public string eventName;
        [FormerlySerializedAs("OnAnimationEvent")] public UnityEvent onAnimationEvent;
    }
}