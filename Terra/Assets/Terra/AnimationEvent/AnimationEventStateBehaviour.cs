using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;

namespace Terra.AnimationEvent
{
    public class AnimationEventStateBehaviour:StateMachineBehaviour
    {
        public string eventName;
        [Range(0f, 0.95f)] public float triggerTime;
        [SerializeField] private bool _resetOnLoop = true;
        [SerializeField] private bool _autoResetOnNotify = false;
        [SerializeField, ReadOnly] private bool _hasTriggered;
        private AnimationEventReceiver _receiver;
        private int _lastLoopCount = -1;
        private float _previousTime =-1f;
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _lastLoopCount = -1;
            _previousTime =-1f;
            
            _hasTriggered = false;
            _receiver = animator.GetComponentInParent<AnimationEventReceiver>();
            if (_receiver == null)
            {
                Debug.LogError($"{this} : Animator {animator.gameObject.name} has no receiver");
            }
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

            float currentNormalizedTime = stateInfo.normalizedTime;

            if (currentNormalizedTime < _previousTime)
            {
                _hasTriggered = false;
                _lastLoopCount = -1;
            }

            _previousTime = currentNormalizedTime;

            int currentLoop = Mathf.FloorToInt(currentNormalizedTime);
            float currentTime = currentNormalizedTime % 1f;
            
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
                if(_autoResetOnNotify) _ = ResetAfterDelay();
            }
            else
            {
                Debug.LogError($"{this} : Animator {animator.name} has no receiver");
            }
        }

        private async UniTaskVoid ResetAfterDelay()
        {
            await UniTask.WaitForEndOfFrame(cancellationToken: _receiver.destroyCancellationToken);
            ResetState();
        }
        public void ResetState()
        {
            _hasTriggered = false;
            _lastLoopCount = -1;
            _previousTime = -1f;
        }
    }
}