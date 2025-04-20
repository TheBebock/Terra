using System;
using UnityEngine.Events;

namespace Terra.AnimationEvents
{
    [Serializable]
    public class AnimationEvent
    {
        public string eventName;
        public UnityEvent OnAnimationEvent;
    }
}