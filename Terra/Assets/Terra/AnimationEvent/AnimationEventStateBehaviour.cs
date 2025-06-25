using UnityEngine;

namespace Terra.AnimationEvent
{
    public class AnimationEventStateBehaviour:StateMachineBehaviour
    {
        public string eventName;
        [Range(0f, 1f)] public float triggerTime;
        [SerializeField]
        private bool _resetOnLoop = true;
        private bool _hasTriggered;
        private AnimationEventReceiver _receiver;
        private int _lastLoopCount = -1;
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _lastLoopCount = -1;
            _hasTriggered = false;
            _receiver = animator.GetComponentInParent<AnimationEventReceiver>();
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            
            float currentTime = stateInfo.normalizedTime % 1f;
            int currentLoop = Mathf.FloorToInt(stateInfo.normalizedTime);
            
            if (_resetOnLoop && stateInfo.loop && currentLoop != _lastLoopCount)
            {
                _lastLoopCount = currentLoop;
                _hasTriggered = false;
            }

            if (!_hasTriggered && currentTime >= triggerTime)
            {
                _hasTriggered = true;
                NotifyReceiver(animator);
            }
        }

        void NotifyReceiver(Animator animator)
        {
            if (_receiver != null)
            {
                _receiver.OnAnimationEventTriggered(eventName);
            }
            else
            {
                Debug.LogError($"{this} : Animator {animator.name} has no receiver");
            }
        }
    }
}