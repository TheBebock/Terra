using System;
using UnityEngine.Events;

namespace Terra.AnimationEvent
{
    [Serializable]
    public class AnimationEvent
    {
        public string eventName;
        public UnityEvent OnAnimationEvent;
    }
}