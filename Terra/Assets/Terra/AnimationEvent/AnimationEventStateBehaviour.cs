using UnityEngine;

namespace Terra.AnimationEvent
{
    public class AnimationEventStateBehaviour:StateMachineBehaviour
    {
        public string eventName;
        [Range(0f, 1f)] public float triggerTime;
        bool hasTriggered;
        private AnimationEventReceiver receiver;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            hasTriggered = false;
            receiver = animator.GetComponentInParent<AnimationEventReceiver>();
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            
            float currentTime=stateInfo.normalizedTime % 1f;
            if(currentTime <= 0.1f) hasTriggered = false;
            
            if (!hasTriggered && currentTime >= triggerTime)
            {
                NotifyReceiver(animator);
                hasTriggered = true;
            }
        }

        void NotifyReceiver(Animator animator)
        {
            if (receiver != null)
            {
                receiver.OnAnimationEventTriggered(eventName);
            }
            else
            {
                Debug.LogError($"{this} : Animator {animator.name} has no receiver");
            }
        }
    }
}